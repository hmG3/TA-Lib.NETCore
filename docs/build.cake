#addin nuget:?package=Cake.DocFx&version=1.0.0
#addin nuget:?package=Cake.Yaml&version=6.0.0
#addin nuget:?package=YamlDotNet&version=16.3.0

using MetaFile = System.IO.File;
using System.Text.Encodings.Web;
using System.Text.Json;

Setup(context =>
{
    CleanDirectory("api");
    CleanDirectory("_site");
});

Task("ExtractMetadata").Does(DocFxMetadata);

Task("ProcessMetadata")
  .IsDependentOn("ExtractMetadata")
  .DoesForEach(() => new [] { "TALib.Candles.yml", "TALib.Functions.yml" }, (string fileName) => {
    var filePath = new FilePath($"api{System.IO.Path.DirectorySeparatorChar}{fileName}");

    if (!FileExists(filePath))
    {
        Error("Documentation meta file does not exists!");
        return;
    }

    Information("Processing file: {0}", fileName);

    var tocFilePath = new FilePath($"api{System.IO.Path.DirectorySeparatorChar}toc.yml");
    var manifestFilePath = new FilePath($"api{System.IO.Path.DirectorySeparatorChar}.manifest");

    var metaGraph = DeserializeYamlFromFile<Dictionary<string, List<dynamic>>>(filePath.FullPath);
    var tocGraph = DeserializeYamlFromFile<Dictionary<string, dynamic>>(tocFilePath.FullPath);

    var manifestJsonContent = MetaFile.ReadAllText(manifestFilePath.FullPath);
    var manifestGraph = JsonSerializer.Deserialize<Dictionary<string, string>>(manifestJsonContent)!;

    var functionToCChildren = new List<Dictionary<string, string>>();

    ProcessMetaGraph(metaGraph, manifestGraph, functionToCChildren);
    UpdateMetaFileData(metaGraph, tocGraph, functionToCChildren);

    string modifiedMetaGraph = $"### YamlMime:ManagedReference{Environment.NewLine}{SerializeYaml(metaGraph)}";
    MetaFile.WriteAllText(filePath.FullPath, modifiedMetaGraph);

    string modifiedToCGraph = $"### YamlMime:TableOfContent{Environment.NewLine}{SerializeYaml(tocGraph)}";
    MetaFile.WriteAllText(tocFilePath.FullPath, modifiedToCGraph);

    var modifiedManifestGraph = JsonSerializer.Serialize(manifestGraph, new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    });
    MetaFile.WriteAllText(manifestFilePath.FullPath, modifiedManifestGraph);

void ProcessMetaGraph(Dictionary<string, List<dynamic>> metaGraph, Dictionary<string, string> manifestGraph,
    List<Dictionary<string, string>> functionToCChildren)
{
    foreach (var value in metaGraph.Values.SelectMany(v => v))
    {
        RemoveVbNodes(value);
    }

    var metaMethods = metaGraph["items"];
    var metaReferences = metaGraph["references"];
    var metaRoot = metaMethods.First();
    metaRoot["source"] = null!;

    var metaMethodPairs = metaMethods
        .Skip(1)
        .Select((value, index) => new { value, index })
        .GroupBy(x => x.index / 2)
        .Select(g => ((Dictionary<object, dynamic>)g.First().value, (Dictionary<object, dynamic>)g.Last().value, g.Key))
        .ToList();

    foreach (var (function, lookBack, index) in metaMethodPairs)
    {
        ProcessFunctionPair(metaRoot, metaMethods, metaReferences, function, lookBack, index);

        var functionName = function["source"]["id"];
        var functionFullName = $"{metaRoot["uid"]}.{functionName}";

        functionToCChildren.Add(new Dictionary<string, string>
        {
            { "uid", functionFullName },
            { "name", functionName }
        });

        manifestGraph[function["uid"]] = $"{functionFullName}.yml";
        manifestGraph[lookBack["uid"]] = $"{functionFullName}.yml";
    }

    metaRoot["children"] = ((List<object>)metaRoot["children"]).Distinct().ToList();
}

void ProcessFunctionPair(dynamic metaRoot, List<dynamic> metaMethods, List<dynamic> metaReferences,
    Dictionary<object, dynamic> function, Dictionary<object, dynamic> lookBack, int index)
{
    var metaRootUid = metaRoot["uid"];
    var functionName = function["source"]["id"];
    var functionFullName = $"{metaRootUid}.{functionName}";

    UpdateMetaChildren(metaRoot, function, lookBack, functionFullName);

    var functionRoot = new Dictionary<object, dynamic>(metaRoot)
    {
        ["children"] = new List<string> { function["uid"], lookBack["uid"] },
        ["uid"] = functionFullName,
        ["commentId"] = $"T:{functionFullName}",
        ["inheritedMembers"] = null!
    };

    var functionReferences = GatherReferences(metaReferences, function, lookBack);

    var functionGraph = new Dictionary<string, List<Dictionary<object, dynamic>>>
    {
        { "items", [functionRoot, function, lookBack] },
        { "references", functionReferences }
    };

    var functionFilePath = new FilePath($"api{System.IO.Path.DirectorySeparatorChar}{functionFullName}.yml");
    string functionYamlContent = $"### YamlMime:ManagedReference{Environment.NewLine}{SerializeYaml(functionGraph)}";
    MetaFile.WriteAllText(functionFilePath.FullPath, functionYamlContent);

    metaMethods.RemoveAll(f => f["uid"] == function["uid"] || f["uid"] == lookBack["uid"]);

    metaReferences.Insert(index + 1, new Dictionary<string, string>
    {
        { "uid", functionFullName },
        { "commentId", $"T:{functionFullName}" },
        { "href", $"{functionFullName}.html" },
        { "name", metaRoot["id"] },
        { "nameWithType", metaRoot["id"] },
        { "fullName", metaRootUid }
    });
}

void UpdateMetaChildren(dynamic metaRoot, Dictionary<object, dynamic> function, Dictionary<object, dynamic> lookBack,
    string functionFullName)
{
    UpdateChild(metaRoot, function["uid"], functionFullName);
    UpdateChild(metaRoot, lookBack["uid"], functionFullName);
}

void UpdateChild(dynamic metaRoot, string uid, string fullName)
{
    var metaRootChildren = (List<object>)metaRoot["children"];
    var index = metaRootChildren.FindIndex(f => f.Equals(uid));
    metaRootChildren[index] = fullName;
}

List<Dictionary<object, dynamic>> GatherReferences(List<dynamic> metaReferences, Dictionary<object, dynamic> function,
    Dictionary<object, dynamic> lookBack)
{
    var functionReferences = new List<Dictionary<object, dynamic>>();

    GatherReference(metaReferences, function["uid"], functionReferences);
    GatherReference(metaReferences, lookBack["uid"], functionReferences);

    if (function.TryGetValue("overload", out var functionOverloadId))
    {
        GatherReference(metaReferences, functionOverloadId, functionReferences);
    }

    if (lookBack.TryGetValue("overload", out var lookBackOverloadId))
    {
        GatherReference(metaReferences, lookBackOverloadId, functionReferences);
    }

    return functionReferences;
}

void GatherReference(List<dynamic> metaReferences, string uid, List<Dictionary<object, dynamic>> references)
{
    var referenceIndex = metaReferences.FindIndex(f => f["uid"] == uid);
    if (referenceIndex != -1)
    {
        references.Add(metaReferences[referenceIndex]);
        metaReferences.RemoveAt(referenceIndex);
    }
}

void UpdateMetaFileData(Dictionary<string, List<dynamic>> metaGraph, Dictionary<string, dynamic> tocGraph,
    List<Dictionary<string, string>> functionToCChildren)
{
    var metaRoot = metaGraph["items"].First();
    var metaRootUid = metaRoot["uid"];
    var tocItems = (List<dynamic>)tocGraph["items"][0]["items"];

    var metaRootRef = tocItems.Find(f => f["uid"] == metaRootUid)!;
    tocItems.Remove(metaRootRef);
    tocItems.Insert(0, metaRootRef);
    metaRootRef["items"] = functionToCChildren;
}

void RemoveVbNodes(dynamic node)
{
    foreach (var kvp in node)
    {
        if (kvp is string k and "vb")
        {
            node.Remove(k);
            break;
        }

        if (kvp is not KeyValuePair<object, object>)
        {
            continue;
        }

        if (kvp.Key is string key && key.EndsWith(".vb"))
        {
            node.Remove(key);
        }
        else if (kvp.Value is not string)
        {
            RemoveVbNodes(kvp.Value);
        }
    }
}
});

Task("BuildMetadata")
  .IsDependentOn("ProcessMetadata")
  .Does(DocFxBuild);

RunTarget("BuildMetadata");
