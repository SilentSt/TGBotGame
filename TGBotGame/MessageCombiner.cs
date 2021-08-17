using System;
using System.Linq;
using System.Threading.Tasks;
using BotDataSet;
using Telegram.Bot.Types;

namespace TGBotGame
{
    public static class MessageCombiner
    {
        //private static string whomName;
        private static string whomId;
        private static DateTime? unPunDate = null;
        public static async Task Combine(Message message)
        {
            whomId = GetUserId(message.Text);
            var spl1 = message.Text.Split('\n')[0];
            var tlw = spl1.ToLower();
            var spl2 = tlw.Split('#');
            var spl3 = spl2[1].Split(' ');
            var mes = spl3[0];
            switch (mes)
            {
                case "ban":
                    if (message.Text.Contains("До:"))
                    {
                        unPunDate = GetUnpunishmentDate(message.Text);
                    }
                    Assist.Ban(long.Parse(whomId), GetReason(message.Text), unPunDate);
                    unPunDate = null;
                    break;
                case "unban":
                    Assist.UnBan(long.Parse(whomId));
                    break;
                case "заглушить":
                    if (message.Text.Contains("До:"))
                    {
                        unPunDate = GetUnpunishmentDate(message.Text);
                    }
                    Assist.Mute(long.Parse(whomId), GetReason(message.Text), unPunDate);
                    unPunDate = null;
                    break;
                case "предупреждать_редактировать":
                    if (int.Parse(GetWarnCount(message.Text)) > Assist.GetUserWarnCount(long.Parse(whomId)))
                    {
                        await Assist.AddWarn(long.Parse(whomId), GetReason(message.Text));
                    }
                    else if (int.Parse(GetWarnCount(message.Text)) < Assist.GetUserWarnCount(long.Parse(whomId)))
                    {
                        await Assist.RemoveWarn(long.Parse(whomId));
                    }

                    if (message.Text.Contains("До:") && Assist.GetUserWarnCount(long.Parse(whomId)) == 3)
                    {
                        Assist.ResetWans(long.Parse(whomId));                                                                               
                        Assist.Mute(long.Parse(whomId), "Получено 3 варна", GetUnpunishmentDate(message.Text));
                    }

                    break;
                case "warn":
                    await Assist.AddWarn(long.Parse(whomId), GetReason(message.Text));
                    break;
                case "звук_включён":
                    Assist.UnMute(long.Parse(whomId));
                    break;
                case "warn_reset":
                    await Assist.RemoveWarn(long.Parse(whomId));
                    break;
                case "новый_пользователь":
                    whomId = GetUserId(message.Text, 1);
                    await Assist.AddUser(long.Parse(whomId));
                    break;
            }
        }

        private static string GetWarnCount(string message)
        {
            var rows = message.Split('\n').Length;
            var sp1 = message.Split('\n')[rows-2];
            var sp2 = sp1.Split(':')[1];
            var sp3 = sp2.Replace(" ", "").Split('/')[0];
            return sp3;
        }

        private static string GetReason(string message)
        {
            try
            {
                return message.Split('\n')[4].Split(':')[2];
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static string GetUserId(string message, int columnNumber = 2)
        {
            try
            {
                return message.Split('\n')[columnNumber].Split(':')[1].Split(' ').Last().Replace("[", "")
                    .Replace("]", "");
            }
            catch (Exception e)
            {
                return "its new user XD";
            }
        }

        private static DateTime GetUnpunishmentDate(string message)
        {
            var rows = message.Split('\n').Length;
            message = message.Split('\n')[rows-3].Split(':')[1];
            var date = message.Split(' ')[1];
            var time = message.Split(' ')[2];
            return new DateTime(int.Parse(date.Split('/')[2]),
                int.Parse(date.Split('/')[1]), int.Parse(date.Split('/')[0]),
                int.Parse(time.Split(':')[0]), int.Parse(time.Split(':')[1]), 0);
        }
    }
}