#nullable enable
namespace best_song.Data;

/// <summary>
///     Tournament struct
///     Sets up a tournament-like structure by creating a binary tree from the bottom up, starting with the leaves
///     and working its way up to the root. Works for an even or uneven amount of inputs.
///     ID String: best_song.Shared.Tournament
/// </summary>
/// <typeparam name="T">Any object</typeparam>
public class Tournament<T>
{
    private int _currentNodeIndex;

    private List<Node> _topLevelNodes;

    /// <summary>
    ///     Initializes a new tournament
    /// </summary>
    /// <param name="entries">Entry list, size > 1</param>
    public Tournament(List<T> entries)
    {
        if (entries.Count < 2)
            throw new ArgumentException($"Entry amount is to small, current amount: {entries.Count}");
        _topLevelNodes = OrganizeNewTopLevel(entries);
    }

    public Tournament()
    {
        _topLevelNodes = new List<Node>(50);
    }

    /// <summary>
    ///     Initializes a new tournament
    /// </summary>
    /// <param name="entries">Entry array, size > 1</param>
    public Tournament(T[] entries) : this(entries.ToList())
    {
    } // Alternate ctor for arrays

    /// <summary>
    ///     Current "level" (tree height) of the tournament and the amount of nodes in it.
    /// </summary>
    public int TopLevelSize => _topLevelNodes.Count;

    public T? Winner { get; private set; }

    public int MatchUpsLeft => GetMatchUpsLeft(TopLevelSize);

    /// <summary>
    ///     Current tournament match up that needs to be resolved, one of the two values has to win.
    /// </summary>
    public T[] CurrentMatchUp
    {
        get
        {
            var currentNode = _topLevelNodes[_currentNodeIndex];
            if (currentNode.ChildCount == 1 || currentNode.ChildCount == 0)
            {
                if (_currentNodeIndex == TopLevelSize - 1)
                {
                    _topLevelNodes = OrganizeNewTopLevel();
                    return CurrentMatchUp;
                }

                return _topLevelNodes[++_currentNodeIndex].ToArray();
            }

            return currentNode.ToArray();
        }
    }

    /// <summary>
    ///     Adds the given list as entries into the tournament if the tournament is empty
    /// </summary>
    /// <param name="entries"></param>
    /// <returns>True if successful, false otherwise</returns>
    public void AddEntries(List<T> entries)
    {
        if (entries.Count == 0) throw new ArgumentException("Entry list empty");
        _topLevelNodes = OrganizeNewTopLevel(entries);
    }

    /// <summary>
    ///     Moves the current tournament match up to the next unfinished on on the current level.
    ///     Will generate a new level if all match ups in the current one are finished.
    /// </summary>
    /// <param name="value">Value that won the previous match up.</param>
    /// <returns>True if the tournament is finished, false otherwise.</returns>
    /// <exception cref="ArgumentException">If the given value did not exist in the current match up.</exception>
    public bool Win(T value)
    {
        var currentNode = _topLevelNodes[_currentNodeIndex];
        switch (currentNode.ChildCount)
        {
            case 0:
                if (_currentNodeIndex == TopLevelSize - 1)
                {
                    _topLevelNodes = OrganizeNewTopLevel();
                    return false;
                }

                _currentNodeIndex++;
                return false;
            case 1:
                currentNode.Value = currentNode.ToArray()[0];
                if (_currentNodeIndex == TopLevelSize - 1)
                {
                    _topLevelNodes = OrganizeNewTopLevel();
                    return false;
                }

                _currentNodeIndex++;
                return false;
        }

        if (currentNode.Left == null || currentNode.Right == null || currentNode.Left.Value == null ||
            currentNode.Right.Value == null)
            throw new NullReferenceException("Node child or child value was null");

        if (!currentNode.Left.Value.Equals(value) && !currentNode.Right.Value.Equals(value))
            throw new ArgumentException("Value could not be found in current match up");

        if (_currentNodeIndex == _topLevelNodes.Count - 1) // Final match up of this level
        {
            if (TopLevelSize == 1) // Final match up of tournament
            {
                _topLevelNodes[0].Value = value;
                Winner = value;
                return true;
            }

            currentNode.Value = value;
            _topLevelNodes = OrganizeNewTopLevel();
            return false;
        }

        currentNode.Value = value;
        _currentNodeIndex++;
        return false;
    }

    public override string ToString()
    {
        return _topLevelNodes.ToString() ?? throw new InvalidOperationException();
    }

    // Helper methods:
    private void Shuffle(List<T> list)
    {
        var rng = new Random();
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private void Shuffle(List<Node> list)
    {
        var rng = new Random();
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private List<Node> OrganizeNewTopLevel(List<T> entries)
    {
        if (_currentNodeIndex != 0 || entries.Count == 0) throw new ArgumentException("List is empty");

        Shuffle(entries);
        List<Node> newTopLevel = new();

        if (entries.Count % 2 != 0)
        {
            for (var i = 0; i < entries.Count - 1; i++) newTopLevel.Add(new Node(entries[i++], entries[i]));

            newTopLevel.Add(new Node(entries[^1]));
        }
        else
        {
            for (var i = 0; i < entries.Count; i++) newTopLevel.Add(new Node(entries[i++], entries[i]));
        }

        _currentNodeIndex = 0;
        return newTopLevel;
    }

    private List<Node> OrganizeNewTopLevel()
    {
        if (_currentNodeIndex != _topLevelNodes.Count - 1)
            throw new InvalidOperationException(
                "Tried to generate new level of nodes with current level unfinished " +
                "current index: {currentNodeIndex}");

        Shuffle(_topLevelNodes);
        List<Node> newTopLevel = new();

        if (_topLevelNodes.Count % 2 != 0)
        {
            for (var i = 0; i < _topLevelNodes.Count - 1; i++)
                newTopLevel.Add(new Node(_topLevelNodes[i++], _topLevelNodes[i]));

            newTopLevel.Add(new Node(_topLevelNodes[^1], null));
        }
        else
        {
            for (var i = 0; i < _topLevelNodes.Count; i++)
                newTopLevel.Add(new Node(_topLevelNodes[i++], _topLevelNodes[i]));
        }

        _currentNodeIndex = 0;
        return newTopLevel;
    }

    private int GetMatchUpsLeft(int curTopSize)
    {
        // Denna är klurig, antalet matchups på grundnivån beror av antalet barn (entries)

        // Base case: Final matchup remains
        if (curTopSize == 1) return 1;

        // Count children
        if (curTopSize == TopLevelSize)
        {
            if (curTopSize % 2 != 0) return 1 + GetMatchUpsLeft(curTopSize - 1);

            return TopLevelSize - _currentNodeIndex + GetMatchUpsLeft(curTopSize / 2);
        }

        return curTopSize + GetMatchUpsLeft(curTopSize / 2);
    }

    internal class Node
    {
        internal readonly Node? Left;
        internal readonly Node? Right;
        internal T? Value;

        // Constructors, one bottom-level and one top-level
        internal Node(T leftValue, T rightValue)
        {
            Left = new Node(leftValue);
            Right = new Node(rightValue);
            Value = default;
        }

        internal Node(T value)
        {
            Left = null;
            Right = null;
            Value = value;
        }

        internal Node(Node? left, Node? right)
        {
            Left = left;
            Right = right;
            Value = default;
        }

        internal int ChildCount
        {
            get
            {
                if ((Left == null) ^ (Right == null)) return 1;
                if (Left != null && Right != null) return 2;
                return 0;
            }
        }

        internal T[] ToArray()
        {
            if (Right != null && Left != null && Right.Value != null && Left.Value != null)
                return new[] { Left.Value, Right.Value };

            if (Right == null && Left == null && Value != null) return new[] { Value };

            if (Right != null && Right.Value != null) return new[] { Right.Value };

            if (Left != null && Left.Value != null) return new[] { Left.Value };

            return Array.Empty<T>();
        }
    }
}