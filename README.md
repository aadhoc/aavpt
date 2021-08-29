# AAVPT (VideoPokerTester)
Having some fun... looking at odds during actual play, rather than theoretical.

Before a trip to Las Vegas I whipped up this Video Poker Simulator based on the cool pre-calulcation/indexing technique provided by [platatat](https://github.com/platatat) / [SnapCall](https://github.com/platatat/SnapCall). I refactored and expanded the bits I needed. There's so much more that I could have added or made configurable, but I'm back from Las Vegas, so I'm done with this for a while.

I'm sharing this ad hoc project for you (but also for myself later). Contribute and/or ask questions if you want. This was a quick personal project and I've even added some documentation. :-)

Shout-out: Follow [VitalVegas](https://twitter.com/VitalVegas) on Twitter. I was able to meet this famous blogger/tweeter in Las Vegas. He has no idea I created this project, or mentioned him.

## Background
I wanted to get a feel for the probabilities of various Video Poker hands while utilizing a basic strategy. I spent about a week of spare time on this and it ended up being "good enough" for my needs & interest.

Some things to watch for...
- When running **PlayForStatistics** for 28,000,000 hands, I got almost the exact results from running 1,000,000,000 hands.
- When running **PlayWithAmount** based on a $1,000,000 bankrole, it played about 73,000,000 hands before bankrupt.
- After initial pre-evaluation and saving into cache, each additional run starts VERY quickly.
- It takes my machine 15 seconds to run a second time (when all pre-evaluation is loaded rather than computed), and running 1,000,000 hands through in **PlayForStatistics** mode.

## Install
Grab the repo and compile/run this console app in Visual Studio 2019.

## Usage
There is code to run in two different modes **PlayForStatistics** and **PlayWithAmount**.

Note that when this runs for the first time (in any mode), it pre-calculates several dictionaries for all possible 5 card combinations, and saves them so they don't have to be re-computed every run. These are computed for all possible 5 card combinations:
- rankings (for dealt hands, courtesy of [platatat](https://github.com/platatat) / [SnapCall](https://github.com/platatat/SnapCall))
- possible hands (from given hand)
- hold choices (from given hand)
- payouts (for dealt hands)

For the most part, the output of this console app is intended to be saved as a CSV file, and loaded into Excel.

### PlayForStatistics
In `Program.cs`, in `Main()` set the run as follows:
```
    //PlayWithAmount(evaluationManager);
    PlayForStatistics(evaluationManager);
```
Set the number of iterations as follows:
```
    static void PlayForStatistics(EvaluationManager evaluationManager)
    {
        var iterations = 1000000;
```

Output is CSV text with lots of overlapping statistics. It looks like this:

Header
```
Start,Possible,Final,Percentage,Count,Odds
```

The "`>Final`" lines are for stats on the final hands the player is left with.
- The Final hand is provided (ex: HighPair)
- The Percentage of the time that final hand occurs
- The Count of times that final hand occurs
- The Odds of that final hand occurring
```
>Final,,LowPair,27.943%,279430,3:1
>Final,,HighCard,26.197%,261967,3:1
>Final,,HighPair,21.939%,219393,4:1
```

The "`>Initial`" lines are for stats of the initial hands the player is given.
- The Initial hand is provided (ex: HighPair)
- The Percentage of the time that initial hand occurs
- The Count of times that initial hand occurs
- The Odds of that initial hand occurring
```
>Initial,,HighCard,50.094%,500942,1:1
>Initial,,LowPair,29.290%,292902,3:1
>Initial,,HighPair,12.972%,129718,7:1
```

The "`>Same`" lines are for stats of the initial and final hands having the same ranking.
- The Initial and final hand ranking provided (ex: HighPair)
- The Percentage of the time that hand "doesn't get better"
- The Count of times that hand "doesn't get better"
- The Odds of that hand "doesn't get better"
```
>Same,,HighCard,25.643%,256432,3:1
>Same,,LowPair,20.341%,203412,4:1
>Same,,HighPair,9.246%,92463,10:1
```

Some lines show stats on having an initial hand, and merely records when it gets better or worse:
- The Initial hand
- Whether the hand got better or worse
- The Percentage of the time the hand got better or worse
- The Count of times the hand got better or worse
- The Odds the hand got better or worse
```
HighPair,,>Worse,0.005%,47,21276:1
LowPair,,>Better,8.405%,84048,11:1
LowPair,,>Worse,0.544%,5442,183:1
```

The majority of lines show:
- The Initial hand
- The Possible hand being attempted during the draw
- The Final hand
- The Percentage of the time for this initial/possible/final combo
- The Count of times for this initial/possible/final combo
- The Odds for this initial/possible/final combo"
```
HighPair,StraightFlush,ThreeOfAKind,0.005%,46,21739:1
HighPair,Royal,HighCard,0.005%,45,22222:1
HighPair,Flush,FullHouse,0.003%,33,30303:1
HighPair,Royal,Flush,0.001%,12,83333:1
HighPair,Straight,FullHouse,0.001%,9,111111:1
HighPair,Flush,FourOfAKind,0.001%,8,125000:1
HighPair,Straight,FourOfAKind,0.001%,5,200000:1
HighPair,StraightFlush,FullHouse,0.000%,4,250000:1
HighPair,Royal,LowPair,0.000%,2,500000:1
HighPair,Royal,Straight,0.000%,2,500000:1
HighPair,Royal,RoyalFlush,0.000%,1,1000000:1
LowPair,Royal,TwoPair,4.402%,44022,22:1
LowPair,Royal,ThreeOfAKind,3.216%,32156,31:1
LowPair,Flush,HighCard,0.438%,4375,228:1
LowPair,Royal,FullHouse,0.283%,2825,353:1
```

Note there are some weird situations where a great hand is initially dealt, but the final hand is lower. For example, a flush might be dealt, but due to possible Royal, the chance MUST be attempted, so a flush could end up being a mere HighPair or even HighCard. Ugh.

If you want to see/change the basic strategy, look in `HoldHelper.cs`
```
    public static Hand DetermineHoldHand(Hand hand)
    {
        Hand holdHand = null;
        HandStrength handStrength = HandReader.GetHandStrength(hand);
        var handRanking = handStrength.HandRanking;
        var possibles = handStrength.Possibles;

        var possibleRoyal = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.Royal);
        var possibleFlush = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.Flush);
        var possibleStraight = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.Straight);
        var possibleStraightFlush = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.StraightFlush);
        var possibleHighs = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.Highs);

        if (handRanking == HandRanking.RoyalFlush)
        {
            holdHand = hand;
        }
        else if (handRanking == HandRanking.StraightFlush)
        {
            holdHand = hand;
        }
        else if (handRanking == HandRanking.FourOfAKind)
        {
            holdHand = new Hand(hand.Cards.Where(card => (int)card.Rank == handStrength.Kickers[0]));
        }
        else if (possibleRoyal != null && possibleRoyal.Cards.Count == 4)
        {
            holdHand = new Hand(possibleRoyal.Cards);
        }
        [..]
```

### PlayWithAmount
This is the simulation of starting with a fixed bankrole and trying to run some number of iterations... basically to see when the bankrole runs out.

Note that this runs with a `*4` to convert dollars into quarters, and 5 quarters per play.
```
    var iterations = 1000000000;
    var balance = 1000000 * 4;  // Convert dollars into quarters
    var minShow = 100;          // Minimum payout amount to display
    var minAlert = 200;         // Minimum payout amount to emphasize
    const int amountPerPlay = 5;// Each play is with 5 quarters
```

In `Program.cs`, in `Main()` set the run as follows:
```
    PlayWithAmount(evaluationManager);
    //PlayForStatistics(evaluationManager);
```

Here's some example output...

Notice:
- The first column is the hand number
- The second column is the balance
- The third column shows the amount won and will be added to the balance
- The initial and final hands are shown
- The final hand ranking is shown
- "`minShow`" is kicking in, showing nothing lower than FourOfAKind
- "`minAlert`" is emphasizing the StraightFlush and RoyalFlush
```
#181891 $994268.75 + $31.25: 8♥ 8♦ 8♣ T♥ J♦ -> 8♠ 8♥ 8♦ 8♣ A♥ (FourOfAKind)
#182048 $994258.75 + $62.50: 6♥ 7♥ 8♥ 9♥ T♥ -> 6♥ 7♥ 8♥ 9♥ T♥ (StraightFlush) !!!!!!!!!!!!!!!!!!!!!!
#182124 $994298.75 + $31.25: 7♣ J♦ Q♦ K♦ K♣ -> 3♣ K♠ K♥ K♦ K♣ (FourOfAKind)
#183130 $994210.00 + $31.25: 2♠ 2♥ 2♦ 5♦ K♥ -> 2♠ 2♥ 2♦ 2♣ A♦ (FourOfAKind)
#183615 $994208.75 + $31.25: 5♠ T♠ K♠ K♥ K♦ -> 3♦ K♠ K♥ K♦ K♣ (FourOfAKind)
#184132 $994121.25 + $31.25: 3♦ 4♠ 4♦ 4♣ 6♥ -> 4♠ 4♥ 4♦ 4♣ 7♣ (FourOfAKind)
#184334 $994076.25 + $31.25: 3♥ 4♥ 8♠ K♥ K♦ -> 4♦ K♠ K♥ K♦ K♣ (FourOfAKind)
#184525 $994062.50 + $31.25: 4♣ 7♠ 7♥ 7♣ 9♣ -> 7♠ 7♥ 7♦ 7♣ J♣ (FourOfAKind)
#184730 $994035.00 + $31.25: 6♠ 6♥ 6♦ 6♣ K♥ -> 6♠ 6♥ 6♦ 6♣ T♠ (FourOfAKind)
#184746 $994057.50 + $31.25: 3♦ 4♦ 7♠ 8♦ K♥ -> 5♥ K♠ K♥ K♦ K♣ (FourOfAKind)
#185097 $994042.50 + $31.25: 2♦ 6♣ 9♥ 9♦ 9♣ -> 9♠ 9♥ 9♦ 9♣ T♥ (FourOfAKind)
#186160 $994048.75 + $31.25: 3♣ T♦ J♦ J♣ K♠ -> J♠ J♥ J♦ J♣ A♣ (FourOfAKind)
#186465 $994053.75 + $31.25: 5♦ 6♥ 6♣ T♦ Q♥ -> 6♠ 6♥ 6♦ 6♣ T♠ (FourOfAKind)
#187066 $994021.25 + $31.25: 4♥ 4♣ 5♠ 8♦ A♠ -> 4♠ 4♥ 4♦ 4♣ Q♣ (FourOfAKind)
#187659 $993992.50 + $31.25: 5♦ 6♦ T♠ T♥ K♠ -> 9♠ T♠ T♥ T♦ T♣ (FourOfAKind)
#187926 $994083.75 + $31.25: 2♣ 4♠ 4♥ 4♦ 9♣ -> 4♠ 4♥ 4♦ 4♣ 9♥ (FourOfAKind)
#188917 $993976.25 + $1000.00: 2♣ J♥ Q♥ K♥ A♥ -> T♥ J♥ Q♥ K♥ A♥ (RoyalFlush) !!!!!!!!!!!!!!!!!!!!!!
```

If you want to see/change the payouts, look in `PayoutEvaluator`.
Shown here, the code is configured to what should be the "Jacks Or Better" video poker pay table.
```
    private int HandStrengthPayoutMultiplier(HandStrength handStrength)
    {
        switch (handStrength.HandRanking)
        {
            case HandRanking.RoyalFlush:
                return _maxPay ? 800 : 250;
            case HandRanking.StraightFlush:
                return 50;
            case HandRanking.FourOfAKind:
                return 25;
            case HandRanking.FullHouse:
                return 9;
            case HandRanking.Flush:
                return 6;
            case HandRanking.Straight:
                return 4;
            case HandRanking.ThreeOfAKind:
                return 3;
            case HandRanking.TwoPair:
                return 2;
            case HandRanking.HighPair:
                return 1;
            default:
                return 0;
        }
    }
```

## Contributing

Feel free to dive in, and open an issue or submit PRs. There's lots of room for improvement in the user interface, configurability, and performance.

You might even find bugs. If so, please submit a PR with a new unit test that shows the defect.

I'll try to watch for questions and PRs over time.

Enjoy!

## License
[Creative Commons Attribution - CC BY](https://creativecommons.org/licenses/by/4.0/)

## Warning
- Use at your own risk.
- Gamble at your own risk.
- Las Vegas is lots of fun. Oh, I mean "at your own risk".