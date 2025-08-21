using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diff_CP
{
    internal interface ITextComparer
    {
        /// <summary>
        /// Compares two text strings and returns a list of differences.
        /// </summary>
        /// <param name="text1">The first text string.</param>
        /// <param name="text2">The second text string.</param>
        /// <returns>A list of differences between the two texts.</returns>
        List<TextOp> CompareTexts(List<string> text1, List<string> text2);
    }

    internal enum TextOpType
    {
        Insert,
        Delete,
        Unchanged
    }

    internal class TextOp
    {
        internal TextOpType Type { get; set; }
        internal string Text { get; set; } = string.Empty;

        public override string ToString()
        {
            return Type switch
            {
                TextOpType.Insert => $"+{Text}",
                TextOpType.Delete => $"-{Text}",
                TextOpType.Unchanged => $" {Text}",
                _ => "Unknown operation"
            };
        }
    }
}
