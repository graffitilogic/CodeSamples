namespace models
{
    public class Game
    {
        public string title;

        public List<Player> players;

        //TODO:  support by the hour billing:
        public DateTime startTime;
        public DateTime endTime;
    }
}