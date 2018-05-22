using System.Net.Mail;
using System.Threading.Tasks;
using LM.Api.Helpers;
using Microsoft.AspNet.Identity;

namespace LM.Api.Providers
{

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await SendGridHelper.SendAsync(message, new MailAddress("levdeo@hotmail.com"));
        }
    }
}