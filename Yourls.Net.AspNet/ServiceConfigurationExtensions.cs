using System;
using System.Net.Http;
using Yourls.Net;
using Yourls.Net.AspNet;
using Yourls.Net.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddYourlsClient(
            this IServiceCollection serviceCollection,
            Action<YourlsConfiguration> config
        )
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
            if (config is null) throw new ArgumentNullException(nameof(config));

            var settings = new YourlsConfiguration();

            config(settings);

            serviceCollection.AddSingleton(settings);

            serviceCollection.AddScoped<YourlsClient>(sp =>
            {
                var obj = sp.GetService<YourlsConfiguration>();

                if (obj is null)
                {
                    throw new InvalidOperationException("Could not resolve an instance of YourlsConfiguration.");
                }

                IAuthenticationHandler authenticationHandler;

                if (!string.IsNullOrWhiteSpace(obj.Signature))
                {
                    authenticationHandler = new SignatureAuthentication(obj.Signature);
                }
                else
                {
                    var isUsername = string.IsNullOrWhiteSpace(obj.Username);
                    var isPassword = string.IsNullOrWhiteSpace(obj.Password);

                    if (isUsername || isPassword)
                    {
                        if (!(isUsername && isPassword))
                        {
                            throw new InvalidOperationException(
                                "YourlsConfiguration.Username and YourlsConfiguration.Password both should be presented."
                            );
                        }
                        
                        authenticationHandler = new UsernamePasswordAuthentication(obj.Username, obj.Password);
                    }
                    else
                    {
                        authenticationHandler = new NoAuthentication();
                    }
                }

                var httpClient = new HttpClient();

                if (!(obj.Timeout is null))
                {
                    httpClient.Timeout = obj.Timeout.Value;
                }

                return new YourlsClientAspNet(
                    new Uri(settings.ApiUrl),
                    authenticationHandler,
                    httpClient
                );
            });
        }
    }
}