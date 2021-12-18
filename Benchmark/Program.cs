using System.Diagnostics;
using System.Reflection;

List<Assembly> assemblies = new();
for (int day = 2; day <= 18; day++)
    assemblies.Add(Assembly.LoadFile($@"D:\Repos\AdventOfCode2021\Day{day}\bin\Release\net6.0\Day{day}.dll"));


foreach (Assembly assembly in assemblies)
{
    Stopwatch sw = Stopwatch.StartNew();
    assembly.EntryPoint?.Invoke(null, new object[] { Array.Empty<string>() });
    sw.Stop();
    Console.WriteLine($"{assembly.GetName().Name} executed in {sw.ElapsedMilliseconds} ms");
}
    