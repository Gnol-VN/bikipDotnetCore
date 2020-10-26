using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CityProject.Services
{
    public class CloudMailService : IMailService
    {
        private readonly ILogger<LocalMailService> _logger;

        public CloudMailService(ILogger<LocalMailService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Send()
        {
            _logger.LogInformation("Send mail by CLOUD service");

        }
    }
}
