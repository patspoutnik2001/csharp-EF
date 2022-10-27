``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2130/21H2/November2021Update)
Intel Core i5-5300U CPU 2.30GHz (Broadwell), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


```
| Method |      Mean |     Error |    StdDev |
|------- |----------:|----------:|----------:|
| Query1 |  8.842 ms | 0.1751 ms | 0.1798 ms |
| Query2 | 11.856 ms | 0.2310 ms | 0.2161 ms |
