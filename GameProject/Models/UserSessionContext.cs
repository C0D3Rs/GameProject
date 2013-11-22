using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Models
{
    public class UserSessionContext
    {
        private HttpSessionStateBase session;

        private const string SessionUserIdKey = "SessionUserIdKey";
        private int userId = 0;

        public void SetHttpSessionStateBase(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public int GetUserId()
        {
            if (session != null && session[SessionUserIdKey] != null)
            {
                userId = (int)session[SessionUserIdKey];
            }

            return userId;
        }

        public void SetUserId(int userId)
        {
            this.userId = userId;
            session[SessionUserIdKey] = userId;
        }

        public void RemoveUserId()
        {
            userId = 0;
            session[SessionUserIdKey] = 0;
        }
    }
}
