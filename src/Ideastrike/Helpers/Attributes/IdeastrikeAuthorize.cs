﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ideastrike.Models.Repositories;

namespace Ideastrike.Helpers.Attributes
{
    public class IdeastrikeAuthorizeAttribute : AuthorizeAttribute
    {
        public IUserRepository UserRepository { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            var userName = httpContext.User.Identity.Name;

            var user = UserRepository.GetAll().FirstOrDefault(u => u.UserName == userName);
            if (user == null)
                return false;

            var expectedRoles = Roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!expectedRoles.Any())
                return true;

            var actualRoles = user.Claims.ToList();
            
            return actualRoles.Any(expectedRoles.Contains);
        }
    }
}