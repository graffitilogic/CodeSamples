using System.Drawing;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

namespace models
{
    public class Player
    {
        public string name;
        public int extraPointDistance; //2 for strike, 1 for spare
        public bool useBumpers;
        public List<Frame> frames;

        public int currentFrame;

        public List<FrameAttempt> strikeFrames;
        public List<FrameAttempt> spareFrames;

        public Guid uniqueID;

        public Player(string player_name, bool use_bumpers)

        {
            name = player_name;
            use_bumpers = useBumpers;
            uniqueID = Guid.NewGuid();
            frames = new List<models.Frame>();

            //seed the player-frames
            for (int i = 0; i < 10; i++)
            {
                frames.Add(new models.Frame(i + 1));
            }
        }

        public void addStrike(int gameFrame, int attempt)
        {
            if (strikeFrames == null) strikeFrames = new List<FrameAttempt>();
            strikeFrames.Add(new FrameAttempt(gameFrame, attempt, 0));
        }

        public void addSpare(int gameFrame, int attempt)
        {
            if (spareFrames == null) spareFrames = new List<FrameAttempt>();
            spareFrames.Add(new FrameAttempt(gameFrame, attempt, 0));
        }


        public FrameAttempt setFrameValue(int gameFrame, bool isStrike, bool isSpare, int pinCount)
        {
            FrameAttempt res = new FrameAttempt(gameFrame, 0, 0);

            models.Frame f = frames[gameFrame - 1];
            models.Frame? pf = (gameFrame == 1) ? null : pf = frames[gameFrame - 2];
            int previousScore = (pf == null) ? 0 : pf.runningScore;

            //set the score, return a little someting about the attempt for user feedback
            if (f.first == null)
            {
                f.first = new models.Attempt(isStrike, isSpare, false, (isStrike) ? 10 : pinCount);
                f.setScore(previousScore);
                res.attempt = 1;
                res.remainingPins = 10 - f.first.pins;
                //autocomplete remaining attempt
                if (isStrike)
                {
                    f.second = new models.Attempt(false, false, false, 0); //autocomplete second throw
                    res.remainingPins = 0;
                    res.attempt = 2;
                }
                strikesAndSpares(gameFrame, 1, f.first.pins);
                return res;
            }

            if (f.second == null)
            {

                //override pin count for strikes and spares
                if (isStrike || isSpare) pinCount = f.getPinCount(isStrike, isSpare, false);

                f.second = new models.Attempt(isStrike, isSpare, false, pinCount);
                f.setScore(previousScore);
                res.attempt = 2;
                strikesAndSpares(gameFrame, 2, f.second.pins);
                return res;
            }

            if (gameFrame == 10)
            {
                if (f.third == null)
                {
                    if (f.allowBonusThrow())
                    {
                        //override pin count for strikes and spares
                        if (isStrike || isSpare) pinCount = f.getPinCount(isStrike, isSpare, true);
                        f.third = new models.Attempt(isStrike, isSpare, false, pinCount);
                    }
                    else
                    {
                        f.third = new models.Attempt(false, false, false, 0);
                    }

                    f.setScore(previousScore);
                    res.attempt = 3;
                    strikesAndSpares(gameFrame, 3, f.third.pins);
                    return res;
                }
            }
            return res;
        }

        public void strikesAndSpares(int gameFrame, int attempt, int pinCount)
        {
            //add points modifiers 
            bool hasChanges = false;
            if (strikeFrames != null)
            {
                //find strikes within 2 shots to modify, do not self-select
                List<FrameAttempt> nearbyStrikes = strikeFrames.Where(sf => sf.frame >= gameFrame - 2 && sf.frame <= gameFrame && !(sf.frame == gameFrame && sf.attempt == attempt)).ToList<FrameAttempt>();

                //two frames for strikes
                foreach (FrameAttempt fa in nearbyStrikes)
                {
                    models.Frame f = frames.Where(f => f.number == fa.frame).First(); // load specific strike frame and modify it.
                    f.addModifier(pinCount);
                    hasChanges = true;
                }
            }
            if (spareFrames != null)
            {
                //find strikes within 1 shot to modify, do not self-select
                List<FrameAttempt> nearbySpares = spareFrames.Where(sf => sf.frame >= gameFrame - 1 && sf.frame <= gameFrame && !(sf.frame == gameFrame && sf.attempt == attempt)).ToList<FrameAttempt>();

                foreach (FrameAttempt fa in nearbySpares)
                {
                    models.Frame f = frames.Where(f => f.number == fa.frame).First();
                    f.addModifier(pinCount);
                    hasChanges = true;
                }
            }

            if (hasChanges) walkCalc();
        }

        public void walkCalc()
        {
            int previousScore = 0;
            foreach (models.Frame f in frames)
            {
                if ((f.number < 10 && f.first != null && f.second != null) || (f.number == 10 && f.first != null && f.second != null && (f.allowBonusThrow() && f.third != null))) previousScore = f.setScore(previousScore);
            }
        }
    }
}