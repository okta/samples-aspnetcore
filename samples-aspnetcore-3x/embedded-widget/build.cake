var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solutionFile = GetFiles("./*.sln").First();
var solution = new Lazy<SolutionParserResult>(() => ParseSolution(solutionFile));
var distDir = Directory("./dist");
var buildDir = Directory("./build");

Task("Clean")
	.IsDependentOn("Clean-Outputs")
	.Does(() => 
	{
		MSBuild(solutionFile, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Clean")
			.SetVerbosity(Verbosity.Minimal));
	});

Task("Clean-Outputs")
	.Does(() => 
	{
		CleanDirectory(buildDir);
		CleanDirectory(distDir);
	});

Task("Build")
	.IsDependentOn("Clean-Outputs")
    .Does(() =>
	{
		NuGetRestore(solutionFile);

		MSBuild(solutionFile, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Rebuild")
			.SetVerbosity(Verbosity.Minimal));
    });


Task("Websites")
	.IsDependentOn("Build")
	.Does(() =>
	{
		var project = solution.Value
			.Projects
			.First();

			Information("Publishing {0}", project.Name);
			
			var publishDir = distDir + Directory(project.Name);

			MSBuild(project.Path, settings => settings
				.SetConfiguration(configuration)
				.WithProperty("DeployOnBuild", "true")
				.WithProperty("WebPublishMethod", "FileSystem")
				.WithProperty("DeployTarget", "WebPublish")
				.WithProperty("publishUrl", MakeAbsolute(publishDir).FullPath)
				.SetVerbosity(Verbosity.Minimal));
	});
	
Task("Default")
	.IsDependentOn("Websites");

RunTarget(target);
