#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace best_song.Shared
{
    public class Tournament<T>
    {
        internal class Node
        {
            internal readonly Node? Left;
            internal readonly Node? Right;
            internal T Value;

            // Constructors, one bottom-level and one top-level
            internal Node(T leftValue, T rightValue)
            {
                Left = new(leftValue);
                Right = new(rightValue);
                Value = default;
            }

            internal Node(T value)
            {
                Left = null;
                Right = null;
                this.Value = value;
            }

            internal Node(Node? left, Node? right)
            {
                this.Left = left;
                this.Right = right;
                Value = default;
            }

            internal T[] ToArray()
            {
                return new T[] { Left.Value, Right.Value };
            }
        }

        private List<Node> topLevelNodes;
        private int currentNodeIndex;
        public int TopLevelSize { get => topLevelNodes.Count; }
        public T[] currentMatchup
        {
            get => topLevelNodes[currentNodeIndex].ToArray();
        }

        public Tournament(List<T> entrys)
        {
            organizeNewTopLevel(entrys);
        }

        public Tournament(T[] entrys) : this(entrys.ToList()) { } // Alternate ctor for arrays

        public bool win(T value)
        {
            var currentNode = topLevelNodes[currentNodeIndex];
            if (!currentNode.Left.Value.Equals(value) && !currentNode.Right.Value.Equals(value))
            {
                throw new ArgumentException("Value could not be found in current matchup");
            }

            if (currentNodeIndex + 1 == topLevelNodes.Count - 1) // Final matchup of this level
            {
                if(TopLevelSize == 1) // Final matchup of tournament
                {
                    topLevelNodes[0].Value = value;
                    return true;
                }

                currentNode.Value = value;
                organizeNewTopLevel();
                return false;
            }
            currentNode.Value = value;
            currentNodeIndex++;
            return false;
        }

        // Helper methods:
        private void shuffle(List<T> list)
        {
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void shuffle(List<Node> list)
        {
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void organizeNewTopLevel(List<T> entrys)
        {
            if (currentNodeIndex != 0 || entrys.Count == 0)
            {
                throw new ArgumentException();
            }

            shuffle(entrys.ToList());
            List<Node> newTopLevel = new();

            if (entrys.Count() % 2 != 0)
            {
                for (int i = 0; i < entrys.Count - 1; i++)
                {
                    newTopLevel.Add(new(entrys[i++], entrys[i]));
                }
                newTopLevel.Add(new(entrys[^0]));
            }
            else
            {
                for (int i = 0; i < entrys.Count; i++)
                {
                    newTopLevel.Add(new(entrys[i++], entrys[i]));
                }
            }

            topLevelNodes = newTopLevel;
            currentNodeIndex = 0;
        }

        private void organizeNewTopLevel()
        {
            if (currentNodeIndex != topLevelNodes.Count - 1)
            {
                throw new SystemException($"Tried to generate new level of nodes with current level unfinished " +
                    "current index: {currentNodeIndex}");
            }

            shuffle(topLevelNodes);
            List<Node> newTopLevel = new();

            if (topLevelNodes.Count() % 2 != 0)
            {
                for (int i = 0; i < topLevelNodes.Count - 1; i++)
                {
                    newTopLevel.Add(new(topLevelNodes[i++], topLevelNodes[i]));
                }
                newTopLevel.Add(new(topLevelNodes[^0], null));
            }
            else
            {
                for (int i = 0; i < topLevelNodes.Count; i++)
                {
                    newTopLevel.Add(new(topLevelNodes[i++], topLevelNodes[i]));
                }
            }

            topLevelNodes = newTopLevel;
            currentNodeIndex = 0;
        }
    }
}