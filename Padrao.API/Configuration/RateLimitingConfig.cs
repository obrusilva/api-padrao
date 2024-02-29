using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Padrao.Domain.Virtual;
using System;
using System.Threading.RateLimiting;

namespace Padrao.Api.Configuration
{
    public static class RateLimitingConfig
    {
        public static IServiceCollection AddRateLimitingConfig(this IServiceCollection services)
        {

            services.AddRateLimiter(opt =>
            {
                opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests; //força o retorno 429 de too many requests
                opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: $"{httpContext.User.Identity?.Name}_{httpContext.Request.Path}",
                        factory: partion =>
                        {
                            return new FixedWindowRateLimiterOptions
                            {
                                AutoReplenishment = true,
                                PermitLimit = 120,
                                QueueLimit = 0,
                                Window = TimeSpan.FromSeconds(60)
                            };
                        })
                );

                opt.AddPolicy("ip", httpContext =>
                   RateLimitPartition.GetFixedWindowLimiter(
                       partitionKey: $"{httpContext.Request.Path}_{httpContext.Connection.RemoteIpAddress}",
                       factory: partition =>
                       {
                           return new FixedWindowRateLimiterOptions
                           {
                               AutoReplenishment = true,
                               PermitLimit = 1,
                               QueueLimit = 0,
                               Window = TimeSpan.FromSeconds(60)
                           };
                       }));
                opt.AddPolicy("concurrency", httpContext =>
                    RateLimitPartition.GetConcurrencyLimiter(
                        partitionKey: httpContext.Request.Path,
                        factory: partition =>
                        {
                            return new ConcurrencyLimiterOptions
                            {
                                PermitLimit = 1,
                                QueueLimit = 0,
                            };
                        }));


                opt.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAtfer))
                        await context.HttpContext.Response.WriteAsJsonAsync(new ResultJson($"O limite de requisições foi atingifo tente novamente daqui {retryAtfer.TotalSeconds} segundos ", null));
                    else if (context.Lease.TryGetMetadata(MetadataName.ReasonPhrase, out var reasonPhrase))
                        await (context.HttpContext.Response.WriteAsync("O limite de requisições simultâneas foi atingido,tente novamente mais tarde"));
                    else
                        await (context.HttpContext.Response.WriteAsync("O limite de requisições foi atingido, tente novamente mais tarde"));
                };
            });

            return services;
        }
    }
}
