using System;
using System.Linq;
using System.Threading.Tasks;
using BotDataSet;
using Telegram.Bot.Types;

namespace TGBotGame
{
    public static class MessageCombiner
    {
        private static string whomId;
        private static DateTime? unPunDate = null;

        public static async Task Combine(Message message)
        {
            //if it was message, not log
            if(!message.Text.Contains("#id")&&!message.Text.Contains("•")) return;
            
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
                        await Assist.ResetWarns(long.Parse(whomId));
                        Assist.Mute(long.Parse(whomId), "Получено 3 варна", GetUnpunishmentDate(message.Text));
                    }

                    break;
                case "warn":
                    await Assist.AddWarn(long.Parse(whomId), GetReason(message.Text));
                    if (message.Text.Contains("До:") && Assist.GetUserWarnCount(long.Parse(whomId)) >= 3)
                    {
                        await Assist.ResetWarns(long.Parse(whomId));
                        Assist.Mute(long.Parse(whomId), "Получено 3 варна", GetUnpunishmentDate(message.Text));
                    }

                    break;
                case "звук_включён":
                    Assist.UnMute(long.Parse(whomId));
                    break;
                case "warn_reset":
                    await Assist.RemoveWarn(long.Parse(whomId));
                    break;
                case "flood":
                    var res = GetPunishment(message.Text);
                    if (res.ToLower().Contains("заглушить"))
                    {
                        Assist.Mute(long.Parse(whomId), GetReason(message.Text), GetUnpunishmentDate(message.Text));
                    }
                    else if (res.ToLower().Contains("блок"))
                    {
                        Assist.Ban(long.Parse(whomId), GetReason(message.Text), GetUnpunishmentDate(message.Text));
                    }
                    else
                    {
                        await Assist.AddWarn(long.Parse(whomId), GetReason(message.Text));
                    }
                    break;
                case "spam":
                    var res1 = GetPunishment(message.Text);
                    if (res1.ToLower().Contains("заглушить"))
                    {
                        Assist.Mute(long.Parse(whomId), GetReason(message.Text), GetUnpunishmentDate(message.Text));
                    }
                    else if (res1.ToLower().Contains("блок"))
                    {
                        Assist.Ban(long.Parse(whomId), GetReason(message.Text), GetUnpunishmentDate(message.Text));
                    }
                    else
                    {
                        await Assist.AddWarn(long.Parse(whomId), GetReason(message.Text));
                    }
                    break;
                case "новый_пользователь":
                    whomId = GetUserId(message.Text, 1);
                    await Assist.AddUser(long.Parse(whomId));
                    break;
            }
        }

        private static string GetWarnCount(string message)
        {
            return message.Split('\n').First(x => x.Contains("• Новые предупреждения: "))
                .Replace("• Новые предупреждения: ", "").Split("/").First();
        }

        private static string GetReason(string message)
        {
            try
            {
                return message.Split('\n').First(x => x.Contains("• По какой причине: "))
                    .Replace("• По какой причине: ", "");
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
                if (message.Contains("Кому:"))
                {
                    return message.Split('\n').FirstOrDefault(x => x.Contains("Кому:")).Split(":").Last().Split(" ")
                        .Last()
                        .Replace("[", "").Replace("]", "");
                }
                else
                {
                    return message.Split('\n').FirstOrDefault(x => x.Contains("Кто:")).Split(":").Last().Split(" ")
                        .Last()
                        .Replace("[", "").Replace("]", "");
                }
            }
            catch (Exception e)
            {
                return "its new user XD";
            }
        }

        private static DateTime? GetUnpunishmentDate(string message)
        {
            try
            {
                message = message.Split('\n').FirstOrDefault(x => x.Contains("• До: ")).Replace("• До: ", "");
                var date = message.Split(' ')[0];
                var time = message.Split(' ')[1];
                return new DateTime(int.Parse(date.Split('/')[2]),
                    int.Parse(date.Split('/')[1]), int.Parse(date.Split('/')[0]),
                    int.Parse(time.Split(':')[0])+3, int.Parse(time.Split(':')[1]), 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static string? GetPunishment(string message)
        {
            try
            {
                return message.Split("\n").First(x => x.Contains("• Наказание: ")).Replace("• Наказание: ", "").Split(' ').First();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
        }
    }
}