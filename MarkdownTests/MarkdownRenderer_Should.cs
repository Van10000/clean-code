using System;
using System.Text;
using Markdown;
using Markdown.MarkdownEnumerable;
using Markdown.TagsRepresentation;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class MarkdownRenderer_Should
    {
        [TestCase("ab _c_ de", ExpectedResult = "ab <em>c</em> de", TestName = "Simple italic tag")]
        [TestCase(@"\_a\_", ExpectedResult = "_a_", TestName = "Escape italic")]
        [TestCase("ab __c__ de", ExpectedResult = "ab <strong>c</strong> de", TestName = "Simple strong tag")]
        [TestCase("__a _b_ c__", ExpectedResult = "<strong>a <em>b</em> c</strong>", TestName = "Italic inside strong")]
        [TestCase("_a __b__ c_", ExpectedResult = "<em>a __b__ c</em>", TestName = "Strong not inside italic")]
        [TestCase("_1_", ExpectedResult = "_1_", TestName = "digit near to highlight")]
        [TestCase("___abc___", ExpectedResult = "___abc___", TestName = "underscore near to highlight")]
        [TestCase("_ b_", ExpectedResult = "_ b_", TestName = "Space symbol after start of highlight is not allowed")]
        [TestCase("a_b_", ExpectedResult = "a_b_", TestName = "Non-space symbol before start of highlight is not allowed")]
        [TestCase("_a _", ExpectedResult = "<em>a _</em>", TestName = "Space symbol before end of highlight is not allowed")]
        [TestCase("_a b_c", ExpectedResult = "<em>a b_c</em>", TestName = "Non-space symbol after end of highlight is not allowed")]
        [TestCase("__a _b", ExpectedResult = "<strong>a <em>b</em></strong>", TestName = "Not closed tags close in the end")]
        [TestCase("__a _b c__", ExpectedResult = "<strong>a <em>b c</em></strong>", TestName = "High level tag closes before low level")]
        [TestCase("[abc](ya.ru)", ExpectedResult = "<a href=\"ya.ru\">abc</a>", TestName = "Simple hyperlink")]
        [TestCase("[a _b_ c](ya.ru)", ExpectedResult = "<a href=\"ya.ru\">a <em>b</em> c</a>", TestName = "Tag inside hyperlink text")]
        [TestCase("[a __b _c](ya.ru)", ExpectedResult = "<a href=\"ya.ru\">a <strong>b <em>c</em></strong></a>", TestName = "Unclosed low level tags close when met high level closing tag")]
        [TestCase("[a](b c)", ExpectedResult = "[a](b c)", TestName = "Incorrect hyperlink stays simple text")]
        [TestCase("[abc]( _b_ c)", ExpectedResult = "[abc]( <em>b</em> c)", TestName = "Tag inside incorrect link in hyperlink")]
        [TestCase("[ab _c]( d_ e)", ExpectedResult = "[ab <em>c]( d</em> e)", TestName = "Tag inside incorrect text and link in hyperlink")]
        [TestCase("[abc](ya.\nru)", ExpectedResult = "[abc](ya.\nru)", TestName = "Line translation inside link")]
        [TestCase("[a]b(c)", ExpectedResult = "[a]b(c)", TestName = "Symbol between hyperlink tags")]
        public string RenderToHtml(string markdown)
        {
            var htmlResult = MarkdownRenderer.RenderToHtml(markdown);

            return htmlResult;
        }

        [TestCase("[abc](/doc.txt)", "C:/documents/", ExpectedResult = "<a href=\"C:/documents/doc.txt\">abc</a>")]
        public string RenderHyperlinksWithRelativePaths(string markdown, string baseUrl)
        {
            var renderer = new MarkdownRenderer(new StringMarkdownEnumerable(markdown), baseUrl);

            return renderer.RenderToHtml();
        }

        [TestCase("_a_ __b__ [c](d.ru)", "a.css", 
            ExpectedResult = "<em class=\"a.css\">a</em> <strong class=\"a.css\">b</strong> <a href=\"d.ru\" class=\"a.css\">c</a>")]
        public string RenderToHtmlWithCssClass(string markdown, string className)
        {
            var renderer = new MarkdownRenderer(new StringMarkdownEnumerable(markdown), cssClass: className);

            return renderer.RenderToHtml();
        }

        [Test, Timeout(1000)]
        public void RenderFast_RandomString()
        {
            var charsCount = (int)1e5;
            var random = new Random(0);
            var markdownBuilder = new StringBuilder(charsCount + 1);

            for (int i = 0; i < charsCount; ++i)
            {
                if (random.Next() % 10 == 0)
                    markdownBuilder.Append('_');
                else if (random.Next() % 5 == 1)
                    markdownBuilder.Append(' ');
                else
                    markdownBuilder.Append('a');
            }

            MarkdownRenderer.RenderToHtml(markdownBuilder.ToString());
        }

        [Test, Timeout(2000)]
        public void RenderFast_TenBigHighlights()
        {
            var charsCount = (int)1e5;
            var partsCount = 10;
            var markdownBuilder = new StringBuilder(charsCount + 1);

            for (int i = 0; i < charsCount; ++i)
                if (charsCount % (charsCount / partsCount) == 0 && i != 0)
                    markdownBuilder.Append("_ _");
                else
                    markdownBuilder.Append('a');

            MarkdownRenderer.RenderToHtml(markdownBuilder.ToString());
        }
    }
}
