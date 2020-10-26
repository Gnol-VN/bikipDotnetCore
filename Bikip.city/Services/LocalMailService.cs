using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog;

namespace CityProject.Services
{
    public class LocalMailService : IMailService
    {
        private readonly ILogger<LocalMailService> _logger;

        public LocalMailService(ILogger<LocalMailService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Send()
        {
            _logger.LogInformation("Send mail by LOCAL service");

        }
    }
}
