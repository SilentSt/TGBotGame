namespace TGBotGame
{
    public static class Constants
    {
        public const string GROUP_USAGE = "Команды бота:\n" +
                                          "/next – позвать на следующую игру\n" +
                                          "/gift – передать кредиты (количество)\n" +
                                          "/friends – добавить в друзья\n" +
                                          "/friendsplay – позвать друзей\n" +
                                          "/delfriends – удалить из друзей\n" +
                                          "/help - помощь";

        public const string USER_USAGE = "Я не понимаю текстовых сообщений, пожалуйста ,воспользуйтесь клавиатурой";
        public const string MENU_TEXT = "Чтобы взаимодействовать со мной, воспользуйтесь клавиатурой";
        public const string FRIENDS_TEXT = "Вы хотите посмотреть список друзей или удалить кого-то из списка?";
        public const string FRIEND_LIST_TEXT = "Список друзей:";
        public const string REMOVE_FRIEND_TEXT = "Кого вы хотите удалить?";
        public const string RULES_TEXT = "Вы перенаправлены на канал правил";
        public const string ROLES_DESRIPION_TEXT = "Вы перенаправлены на канал правил";
        public const string VOKE_NEXT_GAME_TEXT = "Готово!:) мы оповестим тебя как начнётся следущая регистрация";
        public const string VOKE_NEXT_GAME_ALREADY_TEXT = "Ты уже запросил приглашение на следующую игру";
        public const string REASON_PUNISHMENT_TEXT = "Вы хотите узнать причину или снять наказание?";
        public const string REASON_TEXT = "О причне какого типа наказания вы хотите унзать?";
        public const string PUNISHMENT_TEXT = "Мой баланс кредитов: ";
        public const string FILL_CREDITS_TEXT = "Какое колличество кредитов вы хотите пополнить?";
        public const string NO_WARNS_TEXT = "У тебя нет варнов, ты молодец";
        public const string NO_MUTES_TEXT = "У тебя нет мута, играй и общайся спокойно!";
        public const string NO_BANS_TEXT = "Тебя не выгоняли, заходи в игру.";
        public const string FRIEND_REMOVED_TEXT = "Готово, вы больше не друзья!";
        public const string FRIEND_REMOVED_ALREADY =
            "Произошла какая-то ошибка, данного пользователя не было у вас в друзьях";
        public const string VOKE_PLAYER_PLAY = "Привет, вот-вот начнется новая игра, заходи!";
        
        public const string UNSUCCESS_GIFT = "Что-то пошло не так, попробуйте отправить подарок позже";
        public const string FRIEND_REMOVED_EXCEPTION =
            "Что-то пошло не так, попробуйте удалить этого пользователя из друзей позже";
        public const string SUCCESS_REMOVED_PUNISHMENT_TEXT = "Готово! :)";
        public const string NOT_ENOUGH_CREDITS_TEXT = "Недостаточно кредитов!";

        public const string SUCCESS_FILLING_BALANCE_TEXT =
            "После оплаты автоматическое зачисление на счёт в течение 15 минут";

        public const string UNSUCCESS_FILING_BALANCE_TEXT = "Что-то пошло не так, попробуйте пополнить баланс позже";
        public const string UNSUCCESS_REMOVING_PUNISHMENT = "Что-то пошло не так, попробуйте снять наказание позже";
        public const string VOKE_FRIENDS_PLAY_TEXT = "Привет, тебя зовет играть ";
        public const string SUCCESS_ADD_FRIEND_TEXT = "Теперь этот пользователь у вас в друзьях";
        public const string USER_NOT_FOUND = "Этого пользователя нет в моем списке пользователей";
        public const string GIFT_SENT_TEXT = "Подарок был успешно отправлен";
        public const int BAN_REMOVE_PRICE = 16;
        public const int WARN_REMOVE_PRICE = 5;
        public const int MUTE_REMOVE_PRICE = 7;
    }
}