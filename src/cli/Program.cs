using System.CommandLine;

class Program
{
    static void Main(string[] args)
    {
        RootCommand rootCommand = new RootCommand("The CLI Interface for Quick API");

        AddNewCommand(rootCommand);
        
        rootCommand.Parse(args).Invoke();
    }

    static void AddNewCommand(RootCommand rootCommand)
    {
        Command command = new Command("new", "Create a new Quick API Project");

        var path = new Argument<string>("path")
        {
            Description = "Where to create the Project",
            DefaultValueFactory = _ => "."
        };

        command.SetAction(pr =>
        {
           System.Console.WriteLine(pr.GetValue<string>("path")); 
           System.Console.WriteLine(Path.GetFullPath(pr.GetValue<string>("path")));
        });

        command.Arguments.Add(path);

        rootCommand.Add(command);
    }
}
