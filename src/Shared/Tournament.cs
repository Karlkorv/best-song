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
        public bool HasNext
        {
            get => nextPair != null;
        }
        public Pair(T? upper, T? lower)
        {
            this.upper = upper;
            this.lower = lower;
        }

        public Pair(T upper)
        {
            this.upper = upper;
        }
    }

    public readonly int Size;
    private List<Pair> pairs;

    public Tournament(List<T?> entrys)
    {
        // Input checking
        if (entrys.Count == 0 || entrys.Contains(default(T)))
        {
            throw new ArgumentException();
        }
#pragma warning disable // Entrys will not contain null since that is checked in the lines above
        pairs = organizeIntoPairs(entrys);
#pragma warning restore
    }

    public Tournament(T[] entrys)
    {
        if (entrys.Count() == 0)
        {
            throw new ArgumentException("Tournament entry is empty");
        }
        pairs = organizeIntoPairs(entrys.ToList());
    }

    public List<Pair> organizeIntoPairs(List<T> entrys)
    {
        List<Pair> returnList = new();
        var rng = new Random();
        for (int i = 0, j = entrys.Count - 1; i <= j; i++, j--)
        {
            if (i == j)
            {
                returnList.Add(new(entrys[i]));
                break;
            }
            returnList.Add(new Pair(entrys[i], entrys[j]));
        }
        return returnList;
    }
}