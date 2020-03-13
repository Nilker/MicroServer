using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abc.Interface;
using Abc.Model;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Abc.IdentityServer4
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserService loginUserService;

        public ResourceOwnerPasswordValidator(IUserService _loginUserService)
        {
            this.loginUserService = _loginUserService;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            User loginUser = null;
            bool isAuthenticated = loginUserService.Authenticate(context.UserName, context.Password, out loginUser);
            if (!isAuthenticated)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid client credential");
            }
            else
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: new Claim[] {
                        new Claim("Name", context.UserName),
                        new Claim("Id", loginUser.Id.ToString()),
                        new Claim("RealName", loginUser.Name),
                        new Claim("Email", loginUser.Email)
                    }
                );
            }

            return Task.CompletedTask;
        }
    }
}
