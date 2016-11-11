using System;

namespace Markdown.MarkdownEnumerable
{
    public class StringMarkdownEnumerable : IMarkdownEnumerable
    {
        public StringMarkdownEnumerable(string markdown)
        {
            
        }

        public Tag GetNextOpeningTag()
        {
            throw new NotImplementedException();
        }

        public Tag GetNextClosingTag()
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
