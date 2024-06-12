namespace models
{
    public class Attempt
    {
        public bool isStrike;

        public bool isSpare;

        public bool isGutter;  //trigger depressing animation

        public int pins; //pins hit.

        public Attempt(bool is_strike, bool is_spare, bool is_gutter, int pins_down){
                isStrike = is_strike;
                isSpare=is_spare;
                isGutter=is_gutter;
                pins=pins_down;
        }

        public String displayValue(){
            if (isStrike) return "X";
            if (isSpare) return "/";
            if (pins==0) return "-";
            return pins.ToString();
        }

    }
}