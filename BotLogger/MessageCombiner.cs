using System;
using System.Linq;
using BotDataSet;

namespace BotLogger
{
    public static class MessageCombiner
    {
        //private static string whomName;
        private static string whomId;

        public static void Combine(string message)
        {
            whomId = GetUserId(message);
            var spl1 = message.Split('\n')[0];
            var tlw = spl1.ToLower();
            var spl2 = tlw.Split('#');
            var mes = spl2[1];
            switch (mes)
                {
                    case "ban":
                        Assist.Ban(long.Parse(whomId), GetReason(message) );
                        break;
                    case "unban":
                        Assist.UnBan(long.Parse(whomId));
                        break;
                    case "заглушить":
                        Assist.Mute(long.Parse(whomId),GetReason(message));
                        break;
                    case "предупреждать_редактировать ✍🏻":
                        Assist.AddWarn(long.Parse(whomId), GetReason(message));
                        break;
                    case "warn_reset":
                        Assist.RemoveWarn(long.Parse(whomId));
                        break;
                    case "новый_пользователь":
                        whomId = GetUserId(message, 2);
                        Assist.AddUser(long.Parse(whomId));
                        break;
                }
        }

        private static string GetReason(string message)
        {
            try
            {
                return message.Split('\n')[5].Split(':')[2];
            }
            catch (Exception e)
            {
                return "exception";
            }
        }
        
        private static string GetUserId(string message, int columnNumber=3)
        {
            try
            {
                return message.Split('\n')[columnNumber].Split(':')[1].Split(' ').
                    Last().Replace('[','\0').Replace(']', '\0');
            }
            catch (Exception e)
            {
                return "its new user XD";
            }
            
        }
    }
}