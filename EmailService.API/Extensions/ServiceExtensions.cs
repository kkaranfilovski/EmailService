using EmailService.Services.Interfaces;
using EmailService.Services.Services;
using Mailjet.Client;
using Serilog;

namespace EmailService.API.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // Services
            services.AddSingleton<IEmailService, MailJetEmailService>();

            // Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("Logs/logs-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Mailjet
            var mailJetApiKey = Environment.GetEnvironmentVariable("MailJetApiKey");
            var mailJetApiSecret = Environment.GetEnvironmentVariable("MailJetApiSecret");

            services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
            {
                client.SetDefaultSettings();
                client.UseBasicAuthentication(mailJetApiKey, mailJetApiSecret);
            });

            return services;
        }
    }
}
