using AlArz.AdminManagement;
using Domain.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace AlArz
{
    public class CustomMiddleware
    {
        private readonly IRepository<BlockedUserToken, Guid> _blockRepository;
        private readonly RequestDelegate _next;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILogger<CustomMiddleware> _logger;
        public CustomMiddleware(IRepository<BlockedUserToken, Guid> blockRepository, RequestDelegate next, IIdentityUserRepository userRepository, ILogger<CustomMiddleware> logger)
        {
            _blockRepository = blockRepository;
            _userRepository = userRepository;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userPrincipal = context.User as ClaimsPrincipal;

            string userId = string.Empty;

            var clientBrowser = context.Request.Headers["User-Agent"].ToString();


            context.RequestAborted.ThrowIfCancellationRequested();//to handle if the user cancel the request

            if (userPrincipal != null && userPrincipal.Claims.Any())
            {
                userId = userPrincipal.FindFirstValue("Sub");
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userRepository.GetAsync(Guid.Parse(userId));
                    if (user != null && !user.IsActive)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                    var tokenId = ((ClaimsPrincipal)userPrincipal).FindFirstValue("token_id");
                    if (string.IsNullOrEmpty(tokenId) == false && await _blockRepository.CountAsync(c => c.ToeknId == Guid.Parse(tokenId)) > 0)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                }
            }

            //  var acceptLanguage = context.Request.Headers.AcceptLanguage.ToString();

            //   userPrincipal.Identities.First().AddClaim(new Claim(Admin.Language, acceptLanguage));

            var language = ((ClaimsPrincipal)userPrincipal).FindFirstValue(Admin.Language);

            context.RequestAborted.ThrowIfCancellationRequested();//to handle if the user cancel the request

            //this Block To log response to return the user 
            //-----------------------------------
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                // Log the response data
                responseBody.Seek(0, SeekOrigin.Begin);

                var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();

                _logger.LogInformation("Response: {StatusCode} {Body}", context.Response.StatusCode, responseBodyText);

                responseBody.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);
            }
            //-----------------------------------

        }

    }



    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorization(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }



}

