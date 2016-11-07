using System;

namespace Markdown.CharParsers
{
    internal class EscapeParser : ICharParser
    {
        /// <summary>
        /// Returns next symbol or html-tag
        /// </summary>
        public virtual string Parse(string markdown, int startPosition, out int positionAfterLastParsed)
        {
            throw new NotImplementedException();
        }
    }
}
