using System.IO;
using System.Net;

namespace TGBotGame
{
    public static class Configuration
    {
        public static readonly string BotToken = File.ReadAllText("configuration/bot_token.txt");
        public static readonly string RolesDescLink = File.ReadAllText("configuration/roles_descriptions_chat_link.txt");
        public static readonly string RulesLink = File.ReadAllText("configuration/rules_chat_link.txt");
        public static readonly string QiwiToken = "572d68e7f687c369faa28bf9b5636251";
        public static readonly string QiwiMobile = "79185617179";
    }
}