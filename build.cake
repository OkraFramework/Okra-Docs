//////////////////////////////////////////////////////////////////////
// PARAMETERS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var rootDirectory = Argument("rootDirectory", ".");

DirectoryPath baseDirectory = new DirectoryPath(rootDirectory);
DirectoryPath toolsDirectory = baseDirectory.Combine("tools");
DirectoryPath docfxDirectory = toolsDirectory.Combine("docfx");
FilePath docfxZipFile = toolsDirectory.CombineWithFilePath("docfx.zip");
FilePath docfxExeFile = docfxDirectory.CombineWithFilePath("docfx.exe");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("InstallDocFX")
    .WithCriteria(!DirectoryExists (docfxDirectory))
    .Does(() =>
    {
        DownloadFile("https://github.com/dotnet/docfx/releases/download/v2.1/docfx.zip", docfxZipFile);
        Unzip(docfxZipFile, docfxDirectory);
    });

Task("Build")
    .IsDependentOn("InstallDocFX")
    .Does(() =>
    {
        var exitCode = StartProcess(docfxExeFile, "build");
    });

Task("Serve")
    .IsDependentOn("InstallDocFX")
    .Does(() =>
    {
        var exitCode = StartProcess(docfxExeFile, "build --serve");
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);