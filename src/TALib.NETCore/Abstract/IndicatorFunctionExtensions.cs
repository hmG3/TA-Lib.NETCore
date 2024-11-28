using System.Text;

namespace TALib;

public static class IndicatorFunctionExtensions
{
    public static string GetInfo(this Abstract.IndicatorFunction f)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{nameof(f.Name)}: {f.Name}");
        sb.AppendLine($"{nameof(f.Description)}: {f.Description}");
        sb.AppendLine($"{nameof(f.Group)}: {f.Group}");
        sb.AppendLine($"{nameof(f.Inputs)}:");
        foreach (var input in f.Inputs)
        {
            sb.AppendLine($"  - {input}");
        }

        sb.AppendLine($"{nameof(f.Options)}:");
        foreach (var (displayName, hint) in f.Options)
        {
            sb.AppendLine($"  - {displayName} ({hint})");
        }

        sb.AppendLine($"{nameof(f.Outputs)}:");
        foreach (var output in f.Outputs)
        {
            sb.AppendLine($"  - {output.displayName} ({output.displayHint})");
        }

        return sb.ToString();
    }
}
