<Query Kind="Program">
  <NuGetReference>Mono.Cecil</NuGetReference>
  <Namespace>Mono.Cecil</Namespace>
  <Namespace>Mono.Cecil.Cil</Namespace>
  <Namespace>Mono.Collections.Generic</Namespace>
</Query>

void Main()
{
    String path = @"D:\Steam\SteamApps\Common\Gnomoria\";
    String workingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    
    String assemblyNameGnomoria = "Gnomoria.exe";
    String pathGnomoria = Path.Combine(path, assemblyNameGnomoria);
    String workingPathGnomoria = Path.Combine(workingPath, assemblyNameGnomoria);
    File.Copy(pathGnomoria, workingPathGnomoria, true);

    String assemblyNameInject = "GnomoriaInjection.dll";
    String pathInject = Path.Combine(path, assemblyNameInject);
    String workingPathInject = Path.Combine(workingPath, assemblyNameInject);
    File.Copy(pathInject, workingPathInject, true);

    var resolver = new DefaultAssemblyResolver();
    var searchDir = Path.GetDirectoryName(path);
    resolver.AddSearchDirectory(searchDir);

    var p = new ReaderParameters
    {
        AssemblyResolver = resolver,
    };

    AssemblyDefinition assemblyGnomoria = AssemblyDefinition.ReadAssembly(workingPathGnomoria, p);
    AssemblyDefinition assemblyInject = AssemblyDefinition.ReadAssembly(workingPathInject, p);

    var moduleGnomoria = assemblyGnomoria.MainModule;
    var moduleInject = assemblyInject.MainModule;

    var typeInject = moduleInject.Types.Single(obj => obj.FullName == "GnomoriaInjection.Inject");
    var methodHook = typeInject.Methods.Single(obj => obj.Name == "Hook");
    assemblyGnomoria.MainModule.Import(methodHook);

    var typeGnomanEmpire = moduleGnomoria.Types.Single(obj => obj.FullName == "Game.GnomanEmpire");
    var methodGetInstance = typeGnomanEmpire.Methods.Single(obj => obj.Name == "get_Instance");
    var ret = methodGetInstance.Body.Instructions.Last();
    var il = methodGetInstance.Body.GetILProcessor();
    var nextInstruction = il.Create(Mono.Cecil.Cil.OpCodes.Call, moduleGnomoria.Import(methodHook));
    il.InsertBefore(ret, nextInstruction);

    // Explicitly set the output folder.  For the time being, it's going to the desktop.
    String outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    
    // Set the name of the outputted executable file.  
    // To better indicate that this includes injected code, name it appropriately.
    String outputPath = Path.Combine(outputDirectory, "GnomoriaInjected.exe");
    
    // Write the modified assembly.
    assemblyGnomoria.Write(outputPath);
    
    // The location of the assembly may not be known to the user running the script.
    // To more clearly indicate this, show them the location of the modified assembly.
    Console.WriteLine("Assembly Written to {0}", outputPath);
}