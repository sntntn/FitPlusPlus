using FluentEmail.Core;

namespace NotificationService.API.Email;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail ?? throw new ArgumentNullException(nameof(fluentEmail));
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        var response = await _fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body)
            .SendAsync();
        
        Console.WriteLine(response.Successful);
        
        return response.Successful;
    }
}