using AAVPT.Library;
using System;
using System.Diagnostics;
using AAVPT.Library.HandEvaluators;
using System.Collections.Generic;
using System.Linq;

namespace AAVPT
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            EvaluationManager evaluationManager = new EvaluationManager();

            Deck.Debug = true;
            EvaluationManager.Debug = true;

            var payoutEvaluator = new PayoutEvaluator(5, true, debug: true);
            var holdEvaluator = new HoldEvaluator(debug: true);
            var rankingEvaluator = new RankingEvaluator(debug: true);
            var possibleEvaluator = new PossibleEvaluator(debug: true);
            evaluationManager.AddHandEvaluator("payout", payoutEvaluator);
            evaluationManager.AddHandEvaluator("hold", holdEvaluator);
            evaluationManager.AddHandEvaluator("ranking", rankingEvaluator);
            evaluationManager.AddHandEvaluator("possible", possibleEvaluator);
            evaluationManager.InitializeHandEvaluators();
            evaluationManager.PersistHandEvaluators();

            //PlayWithAmount(evaluationManager);
            PlayForStatistics(evaluationManager);

            Console.WriteLine($"Duration {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine("Press enter when ready...");
            Console.ReadLine();
       }

        static void PlayForStatistics(EvaluationManager evaluationManager)
        {
            var iterations = 1000000;

            var rankingEvaluator = evaluationManager.GetHandEvaluator("ranking");
            var payoutEvaluator = evaluationManager.GetHandEvaluator("payout");
            var possibleEvaluator = evaluationManager.GetHandEvaluator("possible");

            var countsBySituation = new Dictionary<string, int>();
            ulong startBitmap = 0;
            Hand startHand = null;
            Play(
                iterations,
                evaluationManager,
                (round) => {
                },
                (round, initialBitmap, initialHand) => {
                    startBitmap = initialBitmap;
                    startHand = initialHand;
                },
                (round, holdBitmap, holdHand) => {
                },
                (round, finalBitmap, finalHand) => {
                    if (round % 1000 == 0) Console.Write($"{nameof(PlayForStatistics)}: {round} / {iterations}\r");

                    var startHandRanking = (HandRanking)rankingEvaluator.GetMappedValue(startBitmap);
                    var finalHandRanking = (HandRanking)rankingEvaluator.GetMappedValue(finalBitmap);
                    var handPossible = (HandPossible)possibleEvaluator.GetMappedValue(startBitmap);

                    if (startHandRanking != finalHandRanking)
                    {
                        IncrementSituation($"{startHandRanking}:{handPossible}:{finalHandRanking}", countsBySituation);

                        if (finalHandRanking > startHandRanking)
                        {
                            IncrementSituation($"{startHandRanking}::>Better", countsBySituation);
                        }
                        else if (finalHandRanking < startHandRanking)
                        {
                            IncrementSituation($"{startHandRanking}::>Worse", countsBySituation);
                        }
                    }
                    else
                    {
                        IncrementSituation($">Same::{startHandRanking}", countsBySituation);
                    }

                    IncrementSituation($">Initial::{startHandRanking}", countsBySituation);
                    IncrementSituation($">Final::{finalHandRanking}", countsBySituation);

                    return true;
                }
            );

            Console.WriteLine();
            Console.WriteLine("Start,Possible,Final,Percentage,Count,Odds");
            char[] splitChars = ":".ToCharArray();
            foreach (var pair in countsBySituation.OrderBy(pair => pair.Key.Substring(0, pair.Key.IndexOf(":"))).ThenByDescending(pair => pair.Value))
            {
                var situation = pair.Key;
                var segments = situation.Split(splitChars, StringSplitOptions.None);
                var curStartHand = segments[0];
                var curHandPossible = segments[1];
                var curFinalHand = segments[2];
                var count = pair.Value;
                var percentage = $"{((double)count * 100 / iterations):0.000}";
                Console.WriteLine($"{curStartHand},{curHandPossible},{curFinalHand},{percentage}%,{count},{iterations/count:0.}:1");
            }
        }

        static void IncrementSituation(string situation, Dictionary<string, int> countsBySituation)
        {
            if (countsBySituation.ContainsKey(situation))
            {
                countsBySituation[situation]++;
            }
            else
            {
                countsBySituation[situation] = 1;
            }
        }

        static void PlayWithAmount(EvaluationManager evaluationManager)
        {
            var iterations = 1000000000;
            var balance = 1000000 * 4;  // Convert dollars into quarters
            var minShow = 100;          // Minimum payout amount to display
            var minAlert = 200;         // Minimum payout amount to emphasize
            const int amountPerPlay = 5;// Each play is with 5 quarters

            var payoutEvaluator = evaluationManager.GetHandEvaluator("payout");
            int originalBalance = 0;
            Hand startHand = null;
            Play(
                iterations,
                evaluationManager,
                (round) => {
                    originalBalance = balance;
                    balance -= amountPerPlay;
                },
                (round, initialBitmap, initialHand) => {
                    startHand = initialHand;
                },
                null,
                (round, finalBitmap, finalHand) => {
                    var finalPayout = payoutEvaluator.GetMappedValue(finalBitmap);

                    balance += (int)finalPayout;

                    if ((int)finalPayout >= minShow)
                    {
                        Console.WriteLine($"#{round} ${(double)originalBalance / 4:00.00} + ${(double)finalPayout / 4:00.00}: {startHand} -> {finalHand} ({HandReader.GetHandStrength(finalHand).HandRanking}) {((int)finalPayout >= minAlert ? "!!!!!!!!!!!!!!!!!!!!!!" : "")}");
                    }

                    return balance > 0;
                }
            );
        }

        static void Play(
            int iterations,
            EvaluationManager evaluationManager,
            Action<int> startRoundFunc,
            Action<int, ulong, Hand> initialHandFunc,
            Action<int, ulong, Hand> holdHandFunc,
            Func<int, ulong, Hand, bool> finalHandFunc)
        {
            var holdEvaluator = evaluationManager.GetHandEvaluator("hold");

            var deck = new Deck();
            for (var round = 1; round <= iterations; round++)
            {
                startRoundFunc?.Invoke(round);

                deck.Shuffle();
                var initialBitmap = deck.Draw(5);
                var initialHand = new Hand(initialBitmap);
                initialHandFunc?.Invoke(round, initialBitmap, initialHand);

                var holdBitmap = holdEvaluator.GetMappedValue(initialBitmap);
                var holdHand = new Hand(holdBitmap);
                holdHandFunc?.Invoke(round, holdBitmap, holdHand);

                var drawBitmap = deck.Draw(5 - holdHand.Cards.Count);
                var finalBitmap = holdBitmap | drawBitmap;
                var finalHand = new Hand(finalBitmap);
                var cont = finalHandFunc == null || finalHandFunc(round, finalBitmap, finalHand);
                if (!cont)
                {
                    break;
                }
            }
        }
    }
}
