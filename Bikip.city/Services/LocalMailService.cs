using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;

namespace CityProject.Services
{
    public class LocalMailService : IMailService
    {
        private readonly ILogger<LocalMailService> _logger;
        private readonly IConfiguration _configuration;

        public LocalMailService(ILogger<LocalMailService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        public void Send()
        {
            string from = _configuration["mailSetting:from"];
            string to = _configuration["mailSetting:to"];
            _logger.LogInformation($"Send mail by LOCAL service from {from} to {to}");

        }
    }
}
