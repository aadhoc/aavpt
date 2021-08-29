using System;

namespace AAVPT.Library
{
    using AAVPT.Library.Enums;

    public class Card : IEquatable<Card>
    {
        public static ConsoleColor[] SuitColors = { ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green };
        public static char[] Ranks = "23456789TJQKA".ToCharArray();
        public static char[] SuitSymbols = { '♠', '♥', '♦', '♣' };
        public static char[] SuitTexts = { 'S', 'H', 'D', 'C' };

        public Rank Rank { get; set; }
        public Suit Suit { get; set; }

        private static int[] rankPrimes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41 };
        private static int[] suitPrimes = new int[] { 43, 47, 53, 59 };

        public int PrimeRank { get { return rankPrimes[(int)Rank]; } }
        public int PrimeSuit { get { return suitPrimes[(int)Suit]; } }

        public ulong GetBitmap()
        {
            return 1ul << ((int)Rank * 4 + (int)Suit);
        }

        public bool Equals(Card other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (!(other is Card otherCard))
            {
                return false;
            }
            return Rank == otherCard.Rank && Suit == otherCard.Suit;
        }

        //public int GetHashCode(Card c)
        //{
        //    return c.PrimeRank * c.PrimeSuit;
        //}
        public override int GetHashCode()
        {
            return PrimeRank * PrimeSuit;
        }

        public Card(string s)
        {
            var chars = s.ToUpper().ToCharArray();
            if (chars.Length != 2) throw new ArgumentException("Card string must be length 2");
            switch (chars[0])
            {
                case '2': Rank = Rank.Two; break;
                case '3': Rank = Rank.Three; break;
                case '4': Rank = Rank.Four; break;
                case '5': Rank = Rank.Five; break;
                case '6': Rank = Rank.Six; break;
                case '7': Rank = Rank.Seven; break;
                case '8': Rank = Rank.Eight; break;
                case '9': Rank = Rank.Nine; break;
                case 'T': Rank = Rank.Ten; break;
                case 'J': Rank = Rank.Jack; break;
                case 'Q': Rank = Rank.Queen; break;
                case 'K': Rank = Rank.King; break;
                case 'A': Rank = Rank.Ace; break;
                default: throw new ArgumentException("Card string rank not valid");
            }
            switch (chars[1])
            {
                case 'S': Suit = Suit.Spades; break;
                case 'H': Suit = Suit.Hearts; break;
                case 'D': Suit = Suit.Diamonds; break;
                case 'C': Suit = Suit.Clubs; break;
                default: throw new ArgumentException("Card string suit not valid");
            }
        }

        public override string ToString()
        {
            return ToString(SuitSymbols);
        }

        public string ToString(char[] suits)
        {
            return Ranks[(int)Rank].ToString() + suits[(int)Suit].ToString();
        }
    }
}
