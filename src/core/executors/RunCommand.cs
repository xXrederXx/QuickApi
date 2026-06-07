using QuickApi.Core.Common;

namespace QuickApi.Core.Executors;

public static class RunCommand
{
    public static void Execute(string path)
    {
        if (IsPathValid(path).TryGetError(out string pathError))
        {
            System.Console.WriteLine(pathError);
            return;
        }

        Result<string, string> projResult = TryGetProjFile(path);
        if (projResult.TryGetError(out string projFileError))
        {
            System.Console.WriteLine(projFileError);
            return;
        }
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

        if (!Directory.Exists(path))
        {
            return Result<bool, string>.Fail("Directory not found");
        }

        return Result<bool, string>.Success(true);
    }

    private static Result<string, string> TryGetProjFile(string path)
    {
        string? projFile = Directory
            .EnumerateFiles(path)
            .FirstOrDefault(fileName => Path.GetExtension(fileName) == ".qaproj");

        if (projFile is null)
        {
            return Result<string, string>.Fail("Project file could not be found.");
        }
        return Result<string, string>.Success(projFile);
    }
}
