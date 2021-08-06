using System.IO;
using System.Net;

namespace TGBotGame
{
    public static class Configuration
    {
        public static readonly string BotToken = File.ReadAllText("configuration/bot_token.txt");
        public static readonly string RolesDescLink = File.ReadAllText("configuration/roles_descriptions_chat_link.txt");
        public static readonly string RulesLink = File.ReadAllText("configuration/rules_chat_link.txt");
        public static readonly string QiwiToken = File.ReadAllText("configuration/qiwi_token.txt");
        public static readonly string QiwiMobile = File.ReadAllText("configuration/qiwi_phone.txt");
        public static long GroupId = long.Parse(File.ReadAllText("configuration/group_id.txt"));
        public static long AdminsGroupId = long.Parse(File.ReadAllText("configuration/admins_group_id.txt"));
    }
}