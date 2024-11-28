## Unified method signature

Technical Analysis (TA) functions are simple mathematical functions.
Each function accepts input spans and writes the results directly into a caller-provided output span.
These functions do NOT allocate memory for the caller, ensuring efficient memory usage.
The number of output data points will NEVER exceed the number of elements specified in the input range (`inRange`), as explained in detail below.

The `Accbands<T>` method signature demonstrates the unified structure shared across all TA functions:

`public static Core.RetCode Accbands<T>(`\
<code>&emsp;<span style="color: #008080">ReadOnlySpan<T> inHigh,</span></code>\
<code>&emsp;<span style="color: #008080">ReadOnlySpan<T> inLow,</span></code>\
<code>&emsp;<span style="color: #008080">ReadOnlySpan<T> inClose,</span></code>\
<code>&emsp;<span style="color: #FF7F50">Range inRange,</span></code>\
<code>&emsp;<span style="color: #FFD700">Span<T> outRealUpperBand,</span></code>\
<code>&emsp;<span style="color: #FFD700">Span<T> outRealMiddleBand,</span></code>\
<code>&emsp;<span style="color: #FFD700">Span<T> outRealLowerBand,</span></code>\
<code>&emsp;<span style="color: #FF7F50">out Range outRange,</span></code>\
<code>&emsp;<span style="color: #708090">int optInTimePeriod = 20</span></code>\
`) where T : IFloatingPointIeee754<T>`

All functions are consistent and share the same parameter structure. The parameters are organized into four distinct sections to ensure consistency and flexibility:

* <span style="color: #008080">One or more spans of input data are specified, each prefixed with <u>*in*</u>. In the example, there are three input spans.<span>
* <span style="color: #FF7F50">The calculation is limited to the range specified by the <u>*inRange*</u> parameter.</span>
* <span style="color: #FFD700">One or more spans for output data are provided, each prefixed with <u>*out*</u>. In this example, three output spans are used.</span>
* <span style="color: #FF7F50"><u>*outRange*</u> represents a valid portion of values within the output spans.</span>
* <span style="color: #708090">Zero or more optional parameters are used to fine-tune the calculations. These parameters are prefixed with <u>*optIn*</u> to maintain clarity.</span>

This parameter structure provides significant flexibility, enabling the function to calculate only the required portion of data.
While slightly complex, it is designed to help advanced users efficiently manage both memory and CPU processing resources.

### Example 1

The following example demonstrates how to calculate a 30-day moving average using closing prices:

```csharp
const int numElements = 400;

var random = new Random();
var input = Enumerable
    .Repeat(0, numElements)
    .Select(_ => random.NextDouble() * 100)
    .ToArray();
var output = new double[numElements];

var retCode = TALib.Functions.Ma<double>(input, Range.All, output, out var outRange, 30, Core.MAType.Sma);
for (var i = 0; i < outRange.End.Value - outRange.Start.Value; i++)
{
    Console.WriteLine($"Day {outRange.Start.Value + i} = {output[i]:F}");
}
```

The important aspect of the output is the `outRange` variable. Even when calculations are requested for the entire input range (`0..399`, `Range.All` or `..`),
the moving average becomes valid only after sufficient data points are available. For a 30-day moving average, the first valid result appears on day 30.
Therefore, `outRange` will be `29..400` indicating that only the first 371 elements (400 - 29) of the output contain valid data, starting at the 30th input element.

As an alternative example, If calculations are restricted to a specific range, such as `125..255` for the `inRange`, the `outRange` will correspondingly adjust to `125..225`.
The minimum period requirement (30 days in this case) is not an issue here, as the input data contains sufficient preceding elements.
In this scenario, only the first 100 elements of the output span will be populated, leaving the remainder untouched.

### Example 2
This example demonstrates how to calculate a 14-period exponential moving average (EMA) for a single price bar, specifically the last day in a dataset of 300 price bars:

```csharp
const int numElements = 300;

var random = new Random();
var input = Enumerable
    .Repeat(0, numElements)
    .Select(_ => random.NextDouble() * 100)
    .ToArray();
var output = new double[numElements];

var retCode = TALib.Functions.Ma<double>(input, 299..299, output, out var outRange, 14, Core.MAType.Sma);
for (var i = 0; i < outRange.End.Value - outRange.Start.Value; i++)
{
    Console.WriteLine($"Day {outRange.Start.Value + i} = {output[i]:F}");
}
```

In this case, the `outRange` will be `299..300`, indicating that only one value is written to the output.
If the input data does not contain enough elements to calculate even one valid result, the `outRange` will be `0..0`, signifying no valid output.

## Reusing memory

All TA functions are designed with memory efficiency in mind.
When the input and output spans of a function are of the same type, the input buffer can be reused to store one of the function's outputs,
reducing the need for temporary memory allocation. The following example demonstrates this:

```csharp
const int numElements = 100;

var random = new Random();
var input = Enumerable
    .Repeat(0, numElements)
    .Select(_ => random.NextDouble() * 100)
    .ToArray();

var retCode = TALib.Functions.Ma<double>(input, ..(input.Length - 1), input, out var outRange, 30, Core.MAType.Sma);
for (var i = 0; i < outRange.End.Value - outRange.Start.Value; i++)
{
    Console.WriteLine($"Day {outRange.Start.Value + i} = {input[i]:F}");
}
```

In this example, the input array is overwritten by the output values.
This approach minimizes memory usage, making it particularly useful for applications where temporary memory allocation needs to be kept to a minimum.

## Leveraging Span&lt;T&gt;

All TA functions are designed to be highly efficient, leveraging the `Span<T>` type to enable high-performance, memory-safe code.
By optimizing resource usage, these functions contribute to improved application performance.

In previous examples, arrays were used to store input and output data. However, any type that can be cast to a `Span<T>` is supported.
For instance, `MemoryPool<T>` can be utilized to reduce memory allocations and minimize garbage collection overhead, as shown in the example below:

```csharp
const int numElements = 300;

using var memoryOwner = MemoryPool<double>.Shared.Rent(numElements * (3 + 3));
var memorySpan = memoryOwner.Memory.Span;

const int highDataOffset = numElements * 0;
const int lowDataOffset = numElements * 1;
const int closeDataOffset = numElements * 2;
const int upperBandOffset = numElements * 3;
const int middleBandOffset = numElements * 4;
const int lowerBandOffset = numElements * 5;

var random = new Random();
for (var i = 0; i < numElements; i++)
{
    memorySpan[highDataOffset + i] = random.NextDouble() * 100;
    memorySpan[lowDataOffset + i] = random.NextDouble() * 100;
    memorySpan[closeDataOffset + i] = random.NextDouble() * 100;
}

var highDataInput = memorySpan.Slice(highDataOffset, numElements);
var lowDataInput = memorySpan.Slice(lowDataOffset, numElements);
var closeDataInput = memorySpan.Slice(closeDataOffset, numElements);
var upperBandOutput = memorySpan.Slice(upperBandOffset, numElements);
var middleBandOutput = memorySpan.Slice(middleBandOffset, numElements);
var lowerBandOutput = memorySpan.Slice(lowerBandOffset, numElements);

var retCode = TALib.Functions.Accbands(highDataInput, lowDataInput, closeDataInput, .., upperBandOutput, middleBandOutput, lowerBandOutput, out var outRange);

for (var i = 0; i < outRange.End.Value - outRange.Start.Value; i++)
{
    Console.WriteLine($"Day {outRange.Start.Value + i} = {upperBandOutput[i]:F}, {middleBandOutput[i]:F}, {lowerBandOutput[i]:F}");
}
```

For scenarios requiring critical performance optimizations, memory can be allocated on the stack to avoid heap allocations entirely.
This approach is demonstrated below:

```csharp
const int numElements = 100;
Span<double> input = stackalloc double[numElements];

var random = new Random();
for (var i = 0; i < numElements; i++)
{
    input[i] = random.NextDouble() * 100;
}

var retCode = TALib.Functions.Ma(input, .., input, out var outRange1, 30, Core.MAType.Ema);
for (var i = 0; i < outRange1.End.Value - outRange1.Start.Value; i++)
{
    Console.WriteLine($"Day {outRange1.Start.Value + i} = {input[i]:F}");
}
```

In some cases `List<T>` collections can also be provided as inputs and outputs to the TA functions:

```csharp
const int numElements = 200;

var random = new Random();
var input = Enumerable
    .Repeat(0, numElements)
    .Select(_ => random.NextDouble() * 100)
    .ToList();
var inputSpan = CollectionsMarshal.AsSpan(input);

var output = Enumerable
    .Repeat<double>(0, numElements)
    .ToList();
var outputSpan = CollectionsMarshal.AsSpan(output);

var retCode = TALib.Functions.Ma<double>(inputSpan, Range.All, outputSpan, out var outRange, 30, Core.MAType.Dema);
for (var i = 0; i < outRange.End.Value - outRange.Start.Value; i++)
{
    Console.WriteLine($"Day {outRange.Start.Value + i} = {output[i]:F}");
}
```

## Output size

Ensuring that the output span is sufficiently large is crucial for proper functionality.
Depending on specific requirements, one of the following methods can be used to determine the size of the output allocation.
These methods are consistent across all TA functions:

| Method           | Description                                                                                                                                                                                                                                                               |
|------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Input matching   | `var allocationSize = inRange.End.Value;`<br/>_Pros_: Simple and easy to implement.<br/>_Cons_: May result in unnecessarily large memory allocation when specifying a small range.                                                                                        |
| Range matching   | `var allocationSize = inRange.End.Value - inRange.Start.Value;`<br/>_Pros_: Straightforward to implement.<br/>_Cons_: Slightly larger allocation than required. For example, with a 30-period SMA, 29 elements are wasted due to the lookback.                            |
| Exact allocation | `var lookback = XXXLookback();`<br/>`var temp = Math.Max(lookback, inRange.Start.Value);`<br/>`var allocationSize = temp > inRange.End.Value ? 0 : inRange.End.Value - temp;`<br/>_Pros_: Optimal allocation algorithm.<br/>_Cons_: Slightly more complex implementation. |

Each TA function provides a corresponding `XXXLookback()` function to calculate the lookback period. Example: For `Sma()`, there is a `SmaLookback()`.

TThe lookback period represents the number of input elements required before the first valid output can be calculated.
For instance, a 10-period simple moving average (SMA) has a lookback period of 9, meaning the first valid result appears after 9 input values.

## Input type

All functions take advantage of [Generic math](https://learn.microsoft.com/en-us/dotnet/standard/generics/math) interfaces allowing them to work with any type that implements the `IFloatingPointIeee754<T>` interface.
This interface is implemented by all the standard floating-point types in .NET.
However, by the time being, only `double` and `float` types can be used with the TA functions.
The commonly used `decimal` type is not supported, as it does not conform to the IEEE standard for decimal floating-point types.
The following [GitHub issue](https://github.com/dotnet/runtime/issues/81376) provides additional context:
> The existing `System.Decimal` type does not conform to the IEEE standard for decimal floating-point types.
> We have no plans to rehash `System.Decimal`, but adding `Decimal32`, `Decimal64`, and `Decimal128` in addition would allow users
> to work within a standard that is being adopted by other languages and frameworks.
> There is also a future where hardware support for these types is more widely adopted,
> and having IEEE-conforming types will allow us to users to take advantage of performance gains.

The support for additional types will be considered in the future, based on community feedback and the evolution of the .NET platform.
