namespace models
{
    public class Player
    {
        public string name;
        public int currentStreak;
        public bool useBumpers;
        public List<Frame> frames;

        public Player (string player_name, bool use_bumpers){
            name=player_name;
            use_bumpers=useBumpers;

            //seed the player-frames
            for (int i=0; i< 10;i++){
                frames.Add(new models.Frame(i+1));
            }
        }
    }
}