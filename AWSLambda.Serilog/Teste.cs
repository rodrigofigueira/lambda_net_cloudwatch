using Microsoft.Extensions.Logging;

namespace AWSLambda.Serilog
{
    public class Teste : ITeste
    {
        private readonly ILogger<Teste> _logger;

        public Teste(ILogger<Teste> logger)
        {
            _logger = logger;
        }

        public void Vai(string value)
        {
            _logger.LogInformation($"Vai... foi {value}", value);
        }

    }
}
