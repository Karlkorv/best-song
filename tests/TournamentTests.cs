using Xunit;
using Bunit;
using best_song;
using System.Collections.Generic;
public class TournamentTests
{
    private Tournament<int> trn;
    private List<int> entrys;
    public TournamentTests()
    {
        entrys = new(50);
        for (int i = 0; i < 50; i++)
        {
            entrys[i] = i;
        }
        trn = new(entrys);
    }
    [Fact]
    public void OrganizeIntoPairsIncludesAllElements()
    {
        // When
        var pairs = trn.organizeIntoPairs(entrys);
        // Then
        foreach (var pair in pairs)
        {
            Assert.Contains(pair.upper, entrys);
        }
    }
}