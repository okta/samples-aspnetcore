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

        public static IIdxContext GetIdxContext(this ISession session, string stateHandle)
        {
            if (string.IsNullOrEmpty(stateHandle))
            {
                return null;
            }

            string idxContextJson = session.GetString(stateHandle);
            if(string.IsNullOrEmpty(idxContextJson))
            {
                return null;
            }

            Dictionary<string, string> idxContext = JsonConvert.DeserializeObject<Dictionary<string, string>>(idxContextJson);
            return new IdxContext(idxContext["codeVerifier"], idxContext["codeChallenge"], idxContext["codeChallengeMethod"], idxContext["interactionHandle"], idxContext["state"]);
        }

        public static Task<SignInWidgetConfiguration> StartWidgetSignInAsync(this HttpContext httpContext, IIdxClient idxClient, string state = null, CancellationToken cancellationToken = default)
        {
            return StartWidgetSignInAsync(httpContext.Session, idxClient, state);
        }

        public static async Task<SignInWidgetConfiguration> StartWidgetSignInAsync(this ISession session, IIdxClient idxClient, string state = null, CancellationToken cancellationToken = default)
        {
            IIdxContext idxContext = session.GetIdxContext(state);
            SignInWidgetConfiguration signInWidgetConfiguration = new SignInWidgetConfiguration(idxClient.Configuration, idxContext);
            if (idxContext == null)
            {
                WidgetSignInResponse widgetSignInResponse = await idxClient.StartWidgetSignInAsync(cancellationToken);
                idxContext = widgetSignInResponse.IdxContext;
                signInWidgetConfiguration = widgetSignInResponse.SignInWidgetConfiguration;
            }            

            string idxContextJson = JsonConvert.SerializeObject(new
            {
                codeVerifier = idxContext.CodeVerifier,
                codeChallenge = idxContext.CodeChallenge,
                codeChallengeMethod = idxContext.CodeChallengeMethod,
                interactionHandle = idxContext.InteractionHandle,
                state = idxContext.State,
            });
            session.SetString(IdxStateKey, idxContext.State);
            session.SetString(idxContext.State, idxContextJson);
            return signInWidgetConfiguration;
        }
    }
}
