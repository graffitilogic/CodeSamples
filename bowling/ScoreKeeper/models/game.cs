namespace models
{
    public class Game
    {
        public string title;

        public List<Player> players;

        public FrameAttempt currentProgress;

        //TODO:  support by the hour billing:
        public DateTime startTime;
        public DateTime endTime;

        public Game(string game_title)
        {
            title = game_title;
            players = new List<Player>();
        }

        public Game(string game_title, List<Player> game_players)
        {
            title = game_title;
            players = game_players;
        }
    }
}