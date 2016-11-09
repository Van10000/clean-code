using System;

namespace Markdown.MarkdownEnumerable
{
    public class StringMarkdownEnumerable : IMarkdownEnumerable
    {
        public StringMarkdownEnumerable(string markdown)
        {
            
        }

        public TextType GetNextTag()
        {
            throw new NotImplementedException();
        }

        public char GetNextChar()
        {
            throw new NotImplementedException();
        }

        public IMarkdownEnumerable GetCopy()
        {
            throw new NotImplementedException();
        }

        public bool IsFinished()
        {
            throw new NotImplementedException();
        }
    }
}
