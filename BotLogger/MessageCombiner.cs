using System;
using System.Linq;
using BotDataSet;

namespace BotLogger
{
    public static class MessageCombiner
    {
        //private static string whomName;
        private static string whomId;
        public static void DatabaseWriter(string message)
        {
            
        }

        private static void Combine(string message)
        {
            if (!message.Contains("log"))
            {
                return;
            }
            else
            {
                whomId = GetUserId(message);
                switch(message.Split(' ')[1].Split('\n')[1].ToLower().Split('#')[1])
                {
                    case "ban":
                        Assist.Ban(Assist.GetUser(long.Parse(whomId)), DateTime.Now, );
                        break;
                    case "unban":
                        //Assist. (Assist.GetUser(long.Parse(whomId)));
                        break;
                    case "заглушить":
                        Assist.Mute(Assist.GetUser(long.Parse(whomId)), DateTime.Now, );
                        break;
                    case "предупреждать_редактировать":
                        Assist.AddWarn(Assist.GetUser(long.Parse(whomId)), GetReason(message));
                        break;
                    case "warn_reset":
                        
                        break;
                    case "новый_пользователь":
                        whomId = GetUserId(message, 2);
                        //Assist.
                        break;
                }
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