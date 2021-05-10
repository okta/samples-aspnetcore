using Microsoft.AspNetCore.Http;
using Okta.Idx.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace okta_social_login_example.Okta
{
    public interface ISocialLoginIdxClient : IIdxClient
    {
        Task<SocialLoginSettings> StartSocialLoginAsync(HttpContext httpContext, string state = null, CancellationToken cancellationToken = default);

        Task<SocialLoginSettings> StartSocialLoginAsync(ISession session, string state = null, CancellationToken cancellationToken = default);

        Task<OktaTokens> RedeemInteractionCodeAsync(IdxContext idxContext, string interactionCode, Action<Exception> exceptionHandler = null, CancellationToken cancellationToken = default);
    }
}
