//////////////////////////////////////////////////////////////////////
// PARAMETERS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var rootDirectory = Argument("rootDirectory", ".");
var docfxUri = "https://github.com/dotnet/docfx/releases/download/v2.1.0-cli-alpha/docfx.cli.zip";

DirectoryPath baseDirectory = new DirectoryPath(rootDirectory);
DirectoryPath toolsDirectory = baseDirectory.Combine("tools");
DirectoryPath docfxDirectory = toolsDirectory.Combine("docfx");
FilePath docfxZipFile = toolsDirectory.CombineWithFilePath("docfx.cli.zip");
FilePath docfxExeFile = docfxDirectory.CombineWithFilePath("docfx.exe");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("InstallDocFX")
    .WithCriteria(!DirectoryExists (docfxDirectory))
    .Does(() =>
    {
        DownloadFile(docfxUri, docfxZipFile);
        Unzip(docfxZipFile, docfxDirectory);
    });

Task("Build")
    .IsDependentOn("InstallDocFX")
    .IsDependentOn("BuildMetadata")
    .Does(() =>
    {
        var exitCode = StartProcess(docfxExeFile, "build");
    });

Task("Serve")
    .IsDependentOn("InstallDocFX")
    .IsDependentOn("BuildMetadata")
    .Does(() =>
    {
        var exitCode = StartProcess(docfxExeFile, "build --serve");
    });

Task("BuildMetadata")
    .IsDependentOn("InstallDocFX")
    .Does(() =>
    {
        var exitCode = StartProcess(docfxExeFile, "metadata");
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