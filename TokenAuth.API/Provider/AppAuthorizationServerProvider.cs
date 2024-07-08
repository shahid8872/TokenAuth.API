﻿using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TokenAuth.API.Models;
using TokenAuth.API.Repository;

namespace TokenAuth.API.Provider
{
    public class AppAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.SetError("invalid_client", "Client credentials could not be retrived");
                context.Rejected();
                return;
            }

            ClientDetail client = (new ClientDetailsRepo()).ValidateClient(clientId, clientSecret);

            if (client != null)
            {
                context.OwinContext.Set<ClientDetail>("oauth:client", client);
                context.Validated(clientId);
            }
            else
            {
                context.SetError("invalid_client", "Client credentials are not valid");
                context.Rejected();
            }

            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserRepo repo = new UserRepo())
            {
                var user = repo.ValidateUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "Username or Password is incorrect!");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                foreach (var role in user.Roles.Split(','))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.Trim()));
                }
                context.Validated(identity);

            }
        }
    }
}