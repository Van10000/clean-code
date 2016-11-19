using System;
using System.Collections.Generic;

namespace Markdown
{
    internal static class EnumerableExtentions
    {
        public static T FirstOrDefault<T>(this IEnumerable<T> items, Predicate<T> prediate, T defaultValue)
        {
            foreach (var tag in items)
                if (prediate(tag))
                    return tag;
            return defaultValue;
        }
    }
}
