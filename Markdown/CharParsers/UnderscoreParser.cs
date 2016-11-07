using System;

namespace Markdown.CharParsers
{
    internal class UnderscoreParser : EscapeParser
    {
        public override string Parse(string markdown, int startPosition, out int positionAfterLastParsed)
        {
            throw new NotImplementedException();
        }

        private static bool IsUnderscore(string markdown, int position)
        {
            throw new NotImplementedException();
        }
    }
}
