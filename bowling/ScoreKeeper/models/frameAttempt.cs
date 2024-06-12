namespace models
{
    public class FrameAttempt
    {
        public int frame;
        public int attempt;
        public int remainingPins;

        public FrameAttempt(int frame_number, int attempt_number, int remaining_pins)
        {
            frame = frame_number;
            attempt = attempt_number;
            remainingPins = remaining_pins;
        }
    }
}