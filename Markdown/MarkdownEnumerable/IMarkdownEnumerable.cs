namespace Markdown.MarkdownEnumerable
{
    public interface IMarkdownEnumerable
    {
        Tag GetNextTag();
        
        char GetNextChar();

        bool IsFinished();
    }

    public enum Tag
    {
        None, Strong, Italic
    }
}
