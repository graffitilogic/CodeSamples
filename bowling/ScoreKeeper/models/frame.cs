namespace models
{
    public class Frame
    {
        public int number;

        public Attempt first;
        public Attempt second;
        public Attempt third;
        public bool isBonus;

        public Frame (int frame_number){
            number = frame_number;
        }
    }
}
