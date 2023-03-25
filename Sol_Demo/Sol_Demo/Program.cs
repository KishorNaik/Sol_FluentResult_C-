// See https://aka.ms/new-console-template for more information
using FluentResults;

try
{
    var myService = new MyService();

    // # Example1
    var result = myService.Example1("kishor");

    if (result.IsFailed)
    {
        Console.WriteLine(result.Errors);
    }
    else
    {
        Console.WriteLine(result.Value);
    }

    // # Example2
    result = myService.Example2("kishor");

    if (result.IsSuccess)
    {
        Console.WriteLine(result.Value);
    }
    else
    {
        foreach (var item in result.Errors)
        {
            Console.WriteLine(item);
        }
    }

    // # Example3

    result = myService.Example3(null);

    if (result.IsSuccess)
    {
        Console.WriteLine(result.Value);
    }
    else
    {
        foreach (var item in result.Errors)
        {
            if (item.Metadata.ContainsKey("StatusCode"))
            {
                Console.WriteLine($"Error => {item.Message}");
                Console.WriteLine($"Status Code => {(int)item.Metadata["StatusCode"]}");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

public class MyService
{
    public Result<string> Example1(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Fail<string>("Name cannot be empty");
        }

        return Result.Ok<string>($"Hello ${name}");
    }

    public Result<string> Example2(string name)
    {
        Result nameResult = ValidateName(name);

        if (nameResult.IsSuccess)
        {
            return Result.Ok<string>($"Hello ${name}");
        }
        else
        {
            return Result.Fail<string>(nameResult.Errors);
        }

        // Inner Method
        Result ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Fail("Name cannot be empty.");
            }
            if (name.Length > 10)
            {
                return Result.Fail("Name cannot be longer than 10 characters.");
            }
            return Result.Ok();
        }
    }

    public Result<string> Example3(string name)
    {
        var errorList = new List<Error>();

        if (string.IsNullOrEmpty(name))
        {
            var error = new Error("Name cannot be empty.")
                            .WithMetadata("StatusCode", 400);
            errorList.Add(error);
        }

        if (name?.Length > 10)
        {
            var error = new Error("Name cannot be longer than 10 characters.")
                            .WithMetadata("StatusCode", 400);
            errorList.Add(error);
        }

        if (errorList.Count > 0)
        {
            return Result.Fail<string>(errorList);
        }

        return Result.Ok<string>($"Hello ${name}");
    }
}