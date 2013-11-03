using GameProject.Enums;
using GameProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Helpers
{
    public static class FlashMessageHelper
    {
        public static void SetMessage(this Controller controller, FlashMessageType messageType, string message)
        {
            controller.TempData[messageType.ToString()] = message;
        }

        public static string GetMessage(TempDataDictionary tempData, FlashMessageType messageType)
        {
            return tempData[messageType.ToString()].ToString();
        }

        public static bool HasMessage(TempDataDictionary tempData, FlashMessageType messageType)
        {
            var result = tempData[messageType.ToString()];
            return result != null ? result.ToString().Length > 0 : false;
        }
    }
}
