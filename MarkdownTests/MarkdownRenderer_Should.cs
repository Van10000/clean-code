using System;
using System.Text;
using Markdown;
using Markdown.MarkdownEnumerable;
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
        [TestCase("a  \nb", ExpectedResult = "a<br>b", TestName = "Simple new line tag")]
        [TestCase("a \nb", ExpectedResult ="a \nb", TestName = "Need at least two white space symbols before new line")]
        [TestCase("a \u0009 \u2000\nb", ExpectedResult = "a<br>b", TestName = "Different white space symbols")]
        [TestCase("  \n", ExpectedResult = "", TestName = "White space symbols in the beginning are for paragraph")]
        [TestCase("# abc", ExpectedResult = "<h1>abc</h1>", TestName = "Simple header")]
        [TestCase("abc # def", ExpectedResult = "abc # def", TestName = "Header should be in the beginning of the line")]
        [TestCase("  \r  #### \u0009ab\u2000  c    ", ExpectedResult = "<h4>ab\u2000  c</h4>", TestName = "White space handling")]
        [TestCase("####### abc", ExpectedResult = "####### abc", TestName = "Too many # symbols")]
        [TestCase("##", ExpectedResult = "<h2></h2>", TestName = "Empty header is possible")]
        [TestCase("### some text\nnext line", ExpectedResult = "<h3>some text</h3><br>next line", 
            TestName = "<br> appears even without 2 spaces in line with header")]
        [TestCase("## abc ##", ExpectedResult = "<h2>abc</h2>", TestName = "Optionally close header tag")]
        [TestCase("## abc ######\n## abc", ExpectedResult = "<h2>abc</h2><br><h2>abc</h2>", TestName = "Not necessary to close with the same number of sharps")]
        [TestCase("## abc ## ## ### #", ExpectedResult = "<h2>abc ## ## ###</h2>", TestName = "Only count the last closing")]
        [TestCase("##abc", ExpectedResult = "##abc", TestName = "Space after '#' is required")]
        public string RenderToHtml_WithoutParagraphs(string markdown)
        {
            var htmlResult = MarkdownRenderer.RenderToHtml(markdown);

            return RemoveParagraphs(htmlResult);
        }

        [TestCase("", ExpectedResult = "", TestName = "Empty string does not contain paragraph")]
        [TestCase("abc", ExpectedResult = "<p>abc</p>", TestName = "Simple single paragraph")]
        [TestCase("abc\n\ndef", ExpectedResult = "<p>abc</p><p>def</p>", TestName = "Two simple paragraphs")]
        [TestCase("abc\n\r  \n\u2000def", ExpectedResult = "<p>abc</p><p>def</p>", TestName = "Extra space symbols between paragraphs are ignored")]
        [TestCase("abc\n\n\n\n\n\n\n\ndef", ExpectedResult = "<p>abc</p><p>def</p>", TestName = "Extra new line symbols are ignored")]
        [TestCase("abc\ndef", ExpectedResult = "<p>abc\ndef</p>", TestName = "One new line symbol is not enough")]
        [TestCase("abc\n\n\n", ExpectedResult = "<p>abc</p>", TestName = "Paragraph doesn't start at the end")]
        [TestCase("\n abc", ExpectedResult = "<p>abc</p>", TestName = "White space symbols are ignored also at the beginning")]
        [TestCase("abc\n\ndef\n\ng", ExpectedResult = "<p>abc</p><p>def</p><p>g</p>", TestName = "Three paragraphs")]
        [TestCase("### def", ExpectedResult = "<p><h3>def</h3></p>", TestName = "Header inside paragraph")]
        [TestCase("abc\n### def", ExpectedResult = "<p>abc</p><p><h3>def</h3></p>", TestName = "Header starts new paragraph")]
        public string RenderToHtml(string markdown)
        {
            return MarkdownRenderer.RenderToHtml(markdown);
        }

        [TestCase("[abc](/doc.txt)", "C:/documents/", ExpectedResult = "<a href=\"C:/documents/doc.txt\">abc</a>")]
        public string RenderHyperlinks_WithRelativePaths_WithoutParagraphs(string markdown, string baseUrl)
        {
            var renderer = new MarkdownRenderer(new StringMarkdownEnumerable(markdown), baseUrl);

            return RemoveParagraphs(renderer.RenderToHtml());
        }

        [TestCase("_a_ __b__ [c](d.ru)", "a.css", 
            ExpectedResult = "<p class=\"a.css\"><em class=\"a.css\">a</em> <strong class=\"a.css\">b</strong> <a href=\"d.ru\" class=\"a.css\">c</a></p>")]
        [TestCase("a  \nb", "abc.css", ExpectedResult = "<p class=\"abc.css\">a<br>b</p>", TestName = "<br> does not have css class")]
        public string RenderToHtmlWithCssClass(string markdown, string className)
        {
            var renderer = new MarkdownRenderer(new StringMarkdownEnumerable(markdown), cssClass: className);

            return renderer.RenderToHtml();
        }

        [Test, Timeout(2000)]
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

        [Test, Timeout(3000)]
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

        private static string RemoveParagraphs(string htmlPage)
        {
            return htmlPage.Replace("<p>", "").Replace("</p>", "");
        }
    }
}
