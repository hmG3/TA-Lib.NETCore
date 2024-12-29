# Abstract API
<br>

## Quick start

`TALib.Abstract.All` provides a centralized instance to access the full collection of functions supported by the library.
Each function is callable by name, and inputs, options, and outputs are passed as arrays.
This ensures that every function adheres to the same interface, regardless of the number of required inputs, options, or outputs.

For example, `Sma` function can be referenced using one of the following approaches:

```csharp
var sma = TALib.Abstract.All["SMA"];
```

alternatively:

```csharp
var sma = TALib.Abstract.Function("SMA");
```

Once a function reference is obtained, it can be executed by providing the required inputs, options, and outputs:

```csharp
const int numElements = 100;
var random = new Random();
var input = Enumerable
    .Repeat(0, numElements)
    .Select(_ => random.NextDouble() * 100)
    .ToArray();
var lookback = sma.Lookback(14);
var output = new double[numElements - lookback];
var retCode = sma.Run([input], [14], [output], Range.All, out _);
```
This approach ensures a consistent, flexible way to interact with all supported functions.

## Advanced usage

Each function reference in `TALib.Abstract` is represented by an instance of the `TALib.Abstract.IndicatorFunction` class.
This class contains the function's definition, which can be inspected using the `GetInfo()` extension method.
This feature is particularly useful for debugging and understanding the specific requirements of a function.

For example:
```csharp
var sma = TALib.Abstract.Function("SMA");
Console.WriteLine(sma.GetInfo());
```
The output provides detailed information about the function:
```text
Name: Sma
Description: Simple Moving Average
Group: Overlap Studies
Inputs:
  - Real
Options:
  - Time Period
Outputs:
  - Real (Line)
```

For a comprehensive guide to individual functions, refer to the [API](../api/TALib.html) section.

The `TALib.Abstract` class implements `IEnumerable<IndicatorFunction>`, enabling advanced querying and data manipulation using `LINQ`.
It is useful to create custom projections or summaries of available functions.
For instance, the `ToFormattedGroupList()` extension method groups all functions by category and outputs a formatted list:
```csharp
var formattedGroupList = Abstract.All.ToFormattedGroupList();
Console.WriteLine(formattedGroupList);
```
This functionality allows easily explore and organize available technical indicators.
