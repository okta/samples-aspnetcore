using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Okta.Idx.Sdk;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Okta
{
    public static class OktaExtensions
    {
        public const string IdxStateKey = "IdxStateHandle";

        public static IdxContext GetIdxContext(this ISession session, string stateHandle)
        {
            string idxContextJson = session.GetString(stateHandle);
            Dictionary<string, string> idxContext = JsonConvert.DeserializeObject<Dictionary<string, string>>(idxContextJson);

            return new IdxContext(idxContext["codeVerifier"], idxContext["codeChallenge"], idxContext["codeChallengeMethod"], idxContext["interactionHandle"], idxContext["state"]);
        }

        public static Task<SignInWidgetConfiguration> StartWidgetSignInAsync(this HttpContext httpContext, IIdxClient idxClient, CancellationToken cancellationToken = default)
        {
            return StartWidgetSignInAsync(httpContext.Session, idxClient);
        }

        public static async Task<SignInWidgetConfiguration> StartWidgetSignInAsync(this ISession session, IIdxClient idxClient, CancellationToken cancellationToken = default)
        {
            WidgetSignInResponse widgetSignInResponse = await idxClient.StartWidgetSignInAsync(cancellationToken);
            IIdxContext idxContext = widgetSignInResponse.IdxContext;
            string idxContextJson = JsonConvert.SerializeObject(new
            {
                codeVerifier = idxContext.CodeVerifier,
                codeChallenge = idxContext.CodeChallenge,
                codeChallengeMethod = idxContext.CodeChallengeMethod,
                interactionHandle = idxContext.InteractionHandle,
                state = idxContext.State,
            });
            session.SetString(IdxStateKey, idxContext.State);
            session.SetString(widgetSignInResponse.IdxContext.State, idxContextJson);
            return widgetSignInResponse.SignInWidgetConfiguration;
        }
    }
}
