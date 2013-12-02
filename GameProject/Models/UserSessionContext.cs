using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Models
{
    public class UserSessionContext
    {
        private HttpContextBase context;
        private const string UserSessionKey = "UserId";

        public UserSessionContext(HttpContextBase context)
        {
            this.context = context;
        }

        public int GetUserId()
        {
            if (context.Session[UserSessionKey] == null)
            {
                return -1;
            }

            return (int)context.Session[UserSessionKey];
        }

        public void SetUserId(int userId)
        {
            context.Session[UserSessionKey] = userId;
        }

        public void RemoveUserId()
        {
            context.Session[UserSessionKey] = null;
        }
    }
}
