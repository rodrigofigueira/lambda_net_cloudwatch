using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Serilog.Formatting.Json;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace AWSLambda.Serilog;

public class Function
{
    private readonly IConfiguration _configuration;
    private ServiceCollection services;

    public Function()
    {
        services = new ServiceCollection();

        _configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", true)
                                .Build();

        // initialize serilog's logger property with valid configuration
        Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(_configuration)
                        .WriteTo.Console(new JsonFormatter())
                        .CreateLogger();
    }


    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public string FunctionHandler(string input, ILambdaContext context)
    {
        StartupServices();
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        var teste = serviceProvider.GetService<ITeste>();
        teste.Vai(input);

        Log.Logger.Information($"Executing lambda function with input {input}", input);
        return input.ToUpper();
    }

    private void StartupServices()
    {
        services.AddLogging(opt => opt.AddSerilog());
        services.AddTransient<ITeste, Teste>();
    }
}
