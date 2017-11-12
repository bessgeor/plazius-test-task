``` ini

BenchmarkDotNet=v0.10.10, OS=Windows 10 Redstone 2 [1703, Creators Update] (10.0.15063.674)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531251 Hz, Resolution=395.0616 ns, Timer=TSC
  [Host] : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2115.0
  Job    : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2115.0

Job=Job  InvocationCount=20  LaunchCount=50  
RunStrategy=ColdStart  TargetCount=10  UnrollFactor=1  
WarmupCount=0  

```
|                     Method |         Mean |     Error |    StdDev |       Median |
|--------------------------- |-------------:|----------:|----------:|-------------:|
|                 SortSorted | 19,635.01 us | 58.129 us | 392.67 us | 19,576.17 us |
|        SortSortedAllocatey |     63.87 us |  9.939 us |  67.14 us |     41.19 us |
|          SortReverseSorted |  3,713.88 us | 24.424 us | 164.99 us |  3,680.54 us |
| SortReverseSortedAllocatey |     64.48 us | 10.260 us |  69.31 us |     40.98 us |
|               SortShuffled | 14,054.42 us | 85.895 us | 580.24 us | 13,917.39 us |
|      SortShuffledAllocatey |     82.74 us | 17.988 us | 121.52 us |     41.92 us |
