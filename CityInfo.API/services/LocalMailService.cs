using System.Diagnostics;

namespace CityInfo.API.services
{
    public class LocalMailService : IMailService
    {
        private readonly string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private readonly string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"LOCAL MAIL SENT FROM {_mailFrom} to {_mailTo}");
        }
    }
}
