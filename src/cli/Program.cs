using System.CommandLine;
using QuickApi.Core.Executors;

namespace QuickApi.Cli;

class Program
{
    static void Main(string[] args)
    {
        RootCommand rootCommand = new RootCommand("The CLI Interface for Quick API");

        AddNewCommand(rootCommand);
        AddRunCommand(rootCommand);

        rootCommand.Parse(args).Invoke();
    }

    static void AddNewCommand(RootCommand rootCommand)
    {
        Command command = new Command("new", "Create a new Quick API Project");

        var path = new Argument<string>("path")
        {
            Description = "Where to create the Project",
            DefaultValueFactory = _ => ".",
        };

        command.SetAction(pr =>
        {
            NewCommand.Execute(Path.GetFullPath(pr.GetValue<string>("path")!));
        });

        command.Arguments.Add(path);

        rootCommand.Add(command);
    }

    static void AddRunCommand(RootCommand rootCommand)
    {
        Command command = new Command("run", "Excecute a Quick API Project");

        var path = new Argument<string>("path")
        {
            Description = "Project path",
            DefaultValueFactory = _ => ".",
        };

        command.SetAction(pr =>
        {
            RunCommand.Execute(Path.GetFullPath(pr.GetValue<string>("path")!));
        });

        command.Arguments.Add(path);

        rootCommand.Add(command);
    }
}
