using System.IO;

namespace BotLogger
{
    public static class Configuration
    {
        public static readonly string BotToken = File.ReadAllText("configuration/logger_bot_token.txt");
    }
}