using System.Security.Cryptography.X509Certificates;

namespace models
{
    public class Frame
    {
        public int number;

        public Attempt first;
        public Attempt second;
        public Attempt third;

        public List<int> modifiers;

        public int frameScore;

        public int runningScore;
        
        public Frame (int frame_number){
            number = frame_number;
        }

        public void addModifier(int value){
            if (modifiers==null) modifiers = new List<int>();
            modifiers.Add(value);
        }

        public int setScore(int old_score){
            int pinTotal = 0;
            if (first!=null) pinTotal+= first.pins;
            if (second!=null) pinTotal+= second.pins;
            if (third!=null) pinTotal += third.pins;

            frameScore = pinTotal;

            //process frame modifiers (spare, strike scoring)
            if (modifiers!=null){
                foreach (int modifier in modifiers){
                    frameScore +=modifier;
                }
            }

            if (frameScore >30) frameScore=30; //rule: max score per frame
            runningScore = old_score+frameScore; 
            return runningScore;
        }

        public bool allowBonusThrow(){
            bool result =false;

            //strike or spares wins the bonus frame
            if (first!=null && first.isStrike) result=true;
            if (second!=null && (second.isSpare || second.isStrike)) result=true;
            return result;
        }

        public int getPinCount(bool isStrike, bool isSpare, bool isBonus ){
                int pinCount = 0;
                //override pin count for strikes and spares
                if (isStrike)
                {
                    pinCount = 10;
                }
                else if (isSpare)
                {
                    pinCount = 10 - (!isBonus ? first.pins: second.pins);
                }
                return pinCount;
        }
    }
}
