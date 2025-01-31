using Merrsoft.MerrMail.Application.Interfaces;
using Merrsoft.MerrMail.Domain.Models;
using Merrsoft.MerrMail.Infrastructure.Helpers;

namespace Merrsoft.MerrMail.Infrastructure.Services;

public class ApplicationGmailService : IApplicationEmailService
{
    public List<Email> GetUnreadEmails(EnvironmentVariables env)
    {
        var service = GmailApiHelper.GetGmailService(env.OAuthClientCredentialsPath, env.AccessTokenPath);

        const string userId = "me";

        var response = service.Users.Messages.List(userId).Execute();

        if (response.Messages != null)
        {
            foreach (var message in response.Messages)
            {
                var fullMessage = service.Users.Messages.Get(userId, message.Id).Execute();

                // Extract and print message details
                Console.WriteLine($"Subject: {fullMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value}");
                Console.WriteLine($"From: {fullMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value}");
                Console.WriteLine($"Date: {fullMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value}");
                Console.WriteLine($"Snippet: {fullMessage.Snippet}");
                Console.WriteLine("------------------------------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("No messages found.");
        }

        return [];
    }

    public Task Reply(string to)
    {
        throw new NotImplementedException();
    }
}