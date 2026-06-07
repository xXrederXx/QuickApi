using QuickApi.Core.Common;
using QuickApi.Core.Config;
using Tomlyn.Syntax;

namespace QuickApi.Core.Executors;

public static class NewCommand
{
    public static void Execute(string path)
    {
        if (IsPathValid(path).TryGetError(out string error))
        {
            System.Console.WriteLine(error);
            return;
        }

        CreateFolders(path);
        CreateFiles(path);
    }

    private static Result<bool, string> IsPathValid(string path)
    {
        if (!Path.IsPathRooted(path))
        {
            return Result<bool, string>.Fail("Path must be rooted");
        }

        if (File.Exists(path))
        {
            return Result<bool, string>.Fail("Path must point to a directory not a file");
        }
        if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
        {
            return Result<bool, string>.Fail("Directory must be empty");
        }

        return Result<bool, string>.Success(true);
    }

    private static void CreateFolders(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        Directory.CreateDirectory(Path.Combine(path, PathConstants.EndpointsFolderName));
        Directory.CreateDirectory(Path.Combine(path, PathConstants.EntitiesFolderName));
    }

    private static void CreateFiles(string path)
    {
        string projName = new DirectoryInfo(path).Name;
        string fileName = $"{projName}.qaproj";
        File.WriteAllText(Path.Combine(path, fileName), GenerateQaProjTOMLContent(projName));
    }

    private static string GenerateQaProjTOMLContent(string projName)
    {
        DocumentSyntax doc = new DocumentSyntax
        {
            Tables =
            {
                new TableSyntax("meta")
                {
                    Items = { { "name", projName }, { "version", "1.0.0" } },
                },
            },
        };
        return doc.ToString();
    }
}
