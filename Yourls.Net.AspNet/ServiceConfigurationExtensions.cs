using System;
using System.Net.Http;
using Yourls.Net;
using Yourls.Net.AspNet;
using Yourls.Net.Authentication;
using Yourls.Net.JsonNet;

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
            
            if (!string.IsNullOrWhiteSpace(settings.Signature))
            {
                settings.AuthenticationHandler = new SignatureAuthentication(settings.Signature);
            }
            else
            {
                var isUsername = string.IsNullOrWhiteSpace(settings.Username);
                var isPassword = string.IsNullOrWhiteSpace(settings.Password);

                if (isUsername || isPassword)
                {
                    if (!(isUsername && isPassword))
                    {
                        throw new InvalidOperationException(
                            "YourlsConfiguration.Username and YourlsConfiguration.Password both should be presented."
                        );
                    }

                    settings.AuthenticationHandler = new UsernamePasswordAuthentication(settings.Username, settings.Password);
                }
                else
                {
                    settings.AuthenticationHandler = new NoAuthentication();
                }
            }

            serviceCollection.AddSingleton(settings);

            serviceCollection.AddScoped<YourlsClient>(sp =>
            {
                var obj = sp.GetService<YourlsConfiguration>();

                if (obj is null)
                {
                    throw new InvalidOperationException("Could not resolve an instance of YourlsConfiguration.");
                }

                IAuthenticationHandler authenticationHandler;

                if (!(obj.AuthenticationHandler is null))
                {
                    authenticationHandler = obj.AuthenticationHandler;
                }
                else if (!string.IsNullOrWhiteSpace(obj.Signature))
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

                var deserializer = obj.JsonDeserializer ?? new YourlsJsonDeserializer();

                return new YourlsClient(
                    new Uri(obj.ApiUrl),
                    authenticationHandler,
                    httpClient,
                    deserializer
                )
                {
                    HttpMethod = obj.HttpMethod,
                };
            });
        }
    }
}