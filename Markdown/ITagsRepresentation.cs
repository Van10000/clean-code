using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.MarkdownEnumerable;

namespace Markdown
{
    interface ITagsRepresentation
    {
        string GetOpeningTag(Tag tag);

        string GetClosingTag(Tag tag);
    }
}
