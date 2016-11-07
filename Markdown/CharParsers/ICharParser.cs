namespace Markdown.CharParsers
{
    internal interface ICharParser
    {
        string Parse(string str, int startPosition, out int positionAfterLast);
    }
}
