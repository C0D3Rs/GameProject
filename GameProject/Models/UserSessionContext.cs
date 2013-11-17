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

        private const string SessionUserKey = "SessionUserKey";
        private User user;

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

        public User GetUser()
        {
            if (session != null && session[SessionUserKey] != null)
            {
                user = (User)session[SessionUserKey];

            }

            return user;
        }

        public void SetUser(User user)
        {
            this.user = user;
            session[SessionUserKey] = user;
        }

        public void RemoveUser()
        {
            user = null;
            session.Remove(SessionUserKey);
        }
    }
}
