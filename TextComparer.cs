using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diff_CP
{
    internal class TextComparer : ITextComparer
    {
        /// <summary>
        /// Compares two text strings and returns a list of differences.
        /// </summary>
        /// <param name="text1">The first text string.</param>
        /// <param name="text2">The second text string.</param>
        /// <returns>A list of differences between the two texts.</returns>
        public List<TextOp> CompareTexts(List<string> source, List<string> destination)
        {
            // myers diff algorithm
            int[,] matrix = new int[destination.Count, source.Count];
            List<TextOp> result = new List<TextOp>();
            HashSet<int> sourceUnchangedIndexes = new HashSet<int>();
            HashSet<int> destinationUnchangedIndexes = new HashSet<int>();

            int minSrc = 0;
            for (int i = 0; i < destination.Count; i++)
            {
                for (int j = minSrc; j < source.Count; j++)
                {
                    if(destination[i] == source[j])
                    {
                        minSrc = j+1;
                        matrix[i, j] = 1;
                        sourceUnchangedIndexes.Add(j);
                        destinationUnchangedIndexes.Add(i);
                        break;
                    }
                }
            }

            int srcStart = 0;
            int destStart = 0;
            for (int i = 0; i < sourceUnchangedIndexes.Count; i++)
            {
                int srcEnd = sourceUnchangedIndexes.ElementAt(i);
                for(int j = srcStart; j < srcEnd; j++)
                {
                    result.Add(new TextOp { Type = TextOpType.Delete, Text = source[j] });
                }
                srcStart = srcEnd + 1;

                int destEnd = destinationUnchangedIndexes.ElementAt(i);
                for (int j = destStart; j < destEnd; j++)
                {
                    result.Add(new TextOp { Type = TextOpType.Insert, Text = destination[j] });
                }
                destStart = destEnd + 1;

                result.Add(new TextOp { Type = TextOpType.Unchanged, Text = source[srcEnd] });
            }

            for(int j = srcStart; j < source.Count; j++)
            {
                result.Add(new TextOp { Type = TextOpType.Delete, Text = source[j] });
            }

            for (int j = destStart; j < destination.Count; j++)
            {
                result.Add(new TextOp { Type = TextOpType.Insert, Text = destination[j] });
            }

            return result;
        }
    }
}
