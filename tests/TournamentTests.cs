using System;
using System.Collections.Generic;
using best_song.Data;
using Xunit;

public class TournamentTests
{
    private readonly Tournament<int> evenTrn;
    private readonly Tournament<int> unevenTrn;

    public TournamentTests()
    {
        int[] unevenEntry = { 1, 2, 3, 4, 5 };
        int[] evenEntry = { 1, 2, 3, 4 };
        var emptyEntry = Array.Empty<int>();
        unevenTrn = new Tournament<int>(unevenEntry);
        evenTrn = new Tournament<int>(evenEntry);
    }

    [Fact]
    public void evenEntryIsFinishedAfter3Rounds()
    {
        evenTrn.Win(evenTrn.CurrentMatchUp[0]);
        evenTrn.Win(evenTrn.CurrentMatchUp[0]);
        Assert.True(evenTrn.Win(evenTrn.CurrentMatchUp[0]));
    }

    [Fact]
    public void unevenEntryIsFinishedAfter4Rounds()
    {
        // Arrange
        var actual = false;
        // Act
        for (var i = 0; i < 4; i++) actual = unevenTrn.Win(unevenTrn.CurrentMatchUp[0]);
        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void TournamentWorksForLargerEntries()
    {
        var bigList = new List<int>();
        for (var i = 0; i < 2000; i++) bigList.Add(i);

        Tournament<int> bigTrn = new(bigList);
        var wonLast = false;
        while (!wonLast) wonLast = bigTrn.Win(bigTrn.CurrentMatchUp[1]);
        Assert.True(wonLast);
    }
}