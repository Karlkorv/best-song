using Xunit;
using Bunit;
using best_song;
using System.Collections.Generic;
using System;
public class TournamentTests
{
    private Tournament<int> trn;
    private List<int> entrys;
    public TournamentTests()
    {
        entrys = new(50);
        for (int i = 1; i < 51; i++)
        {
            entrys.Add(i);
        }
        trn = new(entrys);
    }
    [Fact]
    public void OrganizeIntoPairsIncludesAllElements()
    {
        // When
        var pairs = trn.organizeIntoPairs(new(entrys));
        List<int> actual = new();
        // Then
        foreach (var pair in pairs)
        {
            if (pair.upper != 0)
            {
                actual.Add(pair.upper);
            }
            if (pair.lower != 0)
            {
                actual.Add(pair.lower);
            }
        }
        actual.Sort();
        entrys.Sort();
        Assert.Equal(entrys, actual);
    }

    [Fact]
    public void OrganizeIntoPairsWorksOnUnevenLists()
    {
        // Given
        List<int> expected = new();
        List<int> actual = new();
        for (int i = 1; i < 50; i++)
        {
            expected.Add(i);
        }
        // When
        var pairs = trn.organizeIntoPairs(expected);
        foreach (var pair in pairs)
        {
            if (pair.upper != 0)
            {
                actual.Add(pair.upper);
            }
            if (pair.lower != 0)
            {
                actual.Add(pair.lower);
            }
        }
        // Then
        expected.Sort();
        actual.Sort();
        Assert.Equal(expected, actual);
    }
}