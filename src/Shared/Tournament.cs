using System;
using best_song;
using System.Diagnostics;

public class Tournament<T>
{
    // Länkad lista fast med två objekt per nod.
    public class Pair
    {
        public T? upper { get; set; }
        public T? lower { get; set; }
        public Pair? nextPair { get; set; }
        bool isFinal { get; set; }
        public Pair(T upper, T lower, bool isFinal)
        {
            this.upper = upper;
            this.lower = lower;
            this.isFinal = isFinal;
        }
        public Pair(bool isFinal)
        {
            this.isFinal = isFinal;
        }
    }

    public readonly int Size;
    private List<Pair> pairs;

    public Tournament(List<T> entrys)
    {
        // Input checking
        if (entrys.Count == 0)
        {
            throw new ArgumentException("Tournament entry is empty");
        }
        pairs = organizeIntoPairs(entrys);
    }

    public List<Pair> organizeIntoPairs(List<T> entrys)
    {
        List<Pair> returnList = new((entrys.Count + entrys.Count % 2) / 2);
        bool[] selected = new bool[returnList.Capacity];
        var rng = new Random();
        for (int i = 0; i < entrys.Count; i++)
        {
            int firstIndex = 0;
            int secondIndex = 0;
            while (firstIndex == secondIndex)
            {
                firstIndex = rng.Next(entrys.Count);
                secondIndex = rng.Next(entrys.Count);
            }
            Pair pair = new Pair(false);
            pair.upper = entrys[firstIndex];
            pair.lower = entrys[secondIndex];
            entrys.RemoveAt(firstIndex);
            entrys.RemoveAt(secondIndex);
            returnList.Add(pair);
        }
        return returnList;
    }
}