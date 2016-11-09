using System;

namespace Markdown.MarkdownEnumerable
{
    public class StringMarkdownEnumerable : IMarkdownEnumerable
    {
        public StringMarkdownEnumerable(string markdown)
        {
            
        }

        public Tag GetNextTag()
        {
            throw new NotImplementedException();
        }

        public char GetNextChar()
        {
            throw new NotImplementedException();
        }

        public bool IsFinished()
        {
            throw new NotImplementedException();
        }
    }
}
