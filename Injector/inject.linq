<Query Kind="Program">
  <NuGetReference>Mono.Cecil</NuGetReference>
  <Namespace>Mono.Cecil</Namespace>
  <Namespace>Mono.Cecil.Cil</Namespace>
  <Namespace>Mono.Collections.Generic</Namespace>
</Query>

void Main()
{
    ///
    /// BASIC CONFIGURATION OPTIONS
    ///

    // The path to your Gnomoria installation:
    String path = @"D:\Steam\SteamApps\Common\Gnomoria\";
    
    // The path where the modified assembly will be stored.
    // When not set, defaults to the installation path defined above.
    String outputPath = "";
    
    ///
    /// ADVANCED CONFIGURATION OPTIONS
    ///
    
    // The name of the default Gnomoria executable:
    String assemblyNameGnomoria = "Gnomoria.exe";
    
    // The full name of the type that contains the method that will be modified.
    String moduleTypeInjectTarget = "Game.GnomanEmpire";
    
    // The name of the method where code will be injected:
    String methodNameInjectTarget = "get_Instance";
    
    // The name of the assembly that contains the Hook method that will be injected into the executable:
    String assemblyNameInject = "GnomoriaInjection.dll";
    
    // The full name of the type where the Hook method is defined:
    String moduleTypeInjectDestination = "GnomoriaInjection.Inject";
    
    // The name of the method that a call will be injected for.
    // This method is required to be static, and must not require any parameters.
    String methodNameToInject = "Hook";
    
    // The name of the modified Gnomoria executable:
    String assemblyNameGnomoriaOutput = "GnomoriaInjected.exe";

    ///
    /// INJECTION CODE
    ///

    // Ensure that a trailing slash is present on the path.  When omitted, append it.
    if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
    {
        path = path + Path.DirectorySeparatorChar;
    }

    String pathGnomoria = Path.Combine(path, assemblyNameGnomoria);
    String pathInject = Path.Combine(path, assemblyNameInject);

    if (File.Exists(pathGnomoria)) { Console.WriteLine("Gnomoria assembly found: {0}", pathGnomoria); }
    else { throw new FileNotFoundException("The Gnomoria assembly does not exist!", pathGnomoria); }

    if (File.Exists(pathInject)) { Console.WriteLine("Injection assembly found: {0}", pathInject); }
    else { throw new FileNotFoundException("The Injection assembly does not exist!", pathInject); }

    var resolver = new DefaultAssemblyResolver();
    var searchDir = Path.GetDirectoryName(path);
    resolver.AddSearchDirectory(searchDir);

    var p = new ReaderParameters
    {
        AssemblyResolver = resolver,
    };

    resolver.ResolveFailure += (sender, args) =>
    {
        sender.Dump("sender");
        args.Dump("args");
        return null;
    };
    
    
    AssemblyDefinition assemblyGnomoria = AssemblyDefinition.ReadAssembly(pathGnomoria, p);
    AssemblyDefinition assemblyInject = AssemblyDefinition.ReadAssembly(pathInject, p);

    var moduleGnomoria = assemblyGnomoria.MainModule;
    var moduleInject = assemblyInject.MainModule;

    var typeInject = moduleInject.Types.Single(obj => obj.FullName == moduleTypeInjectDestination);
    var methodHook = typeInject.Methods.Single(obj => obj.Name == methodNameToInject);
    assemblyGnomoria.MainModule.Import(methodHook);
    
    var typeGnomanEmpire = moduleGnomoria.Types.Single(obj => obj.FullName == moduleTypeInjectTarget);
    var methodGetInstance = typeGnomanEmpire.Methods.Single(obj => obj.Name == methodNameInjectTarget);
    var il = methodGetInstance.Body.GetILProcessor();
    var nextInstruction = il.Create(Mono.Cecil.Cil.OpCodes.Call, moduleGnomoria.Import(methodHook));
    var ret = methodGetInstance.Body.Instructions.Last();
    il.InsertBefore(ret, nextInstruction);

    if (String.IsNullOrWhiteSpace(outputPath))
    {
        outputPath = path;
    }
    outputPath = Path.Combine(outputPath, assemblyNameGnomoriaOutput);
    
    // Write the modified assembly.
    assemblyGnomoria.Write(outputPath);
    
    // The location of the assembly may not be known to the user running the script.
    // To more clearly indicate this, show them the location of the modified assembly.
    Console.WriteLine();
    Console.WriteLine("Assembly Written to {0}", outputPath);
}