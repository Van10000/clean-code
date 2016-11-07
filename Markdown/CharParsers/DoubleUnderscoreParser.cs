using System;

namespace Markdown.CharParsers
{
    internal class DoubleUnderscoreParser : UnderscoreParser
    {
        public override string Parse(string markdown, int startPosition, out int positionAfterLastParsed)
        {
            throw new NotImplementedException();
        }

        private static bool IsDoubleUnderscore(string markdown, int position)
        {
            throw new NotImplementedException();
        }
    }
}
