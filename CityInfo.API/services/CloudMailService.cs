using System.Diagnostics;

namespace CityInfo.API.services
{
    public class CloudMailService : IMailService
    {
        private readonly string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private readonly string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"CLOUD MAIL SENT FROM {_mailFrom} to {_mailTo}");
        }
    }
}
