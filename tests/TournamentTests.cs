using Xunit;
using Bunit;
using best_song;
using System.Collections.Generic;
using System;
public class TournamentTests
{
    TournamentTests()
    {
        int[] unevenEntry = { 1, 2, 3, 4, 5 };
        int[] evenEntry = { 1, 2, 3, 4 };
        int[] emptyEntry = Array.Empty<int>();
    }

    [Fact]
    public void evenEntryIsFinishedAfter3Rounds()
    {

    }
}