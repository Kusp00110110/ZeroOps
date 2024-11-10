using System;
using System.Collections.Generic;
using System.Globalization;

namespace MusicIndustries.ProductLoader.Extensions
{
    namespace Extensions
    {
        public static class EnumerableExtensions
        {
            public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
            {
                if (chunkSize <= 0) throw new ArgumentException("Chunk size must be greater than zero.", nameof(chunkSize));

                using var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    yield return YieldChunkElements(enumerator, chunkSize - 1);
                }
            }

            private static IEnumerable<T> YieldChunkElements<T>(IEnumerator<T> enumerator, int chunkSize)
            {
                yield return enumerator.Current;

                for (var i = 0; i < chunkSize && enumerator.MoveNext(); i++)
                {
                    yield return enumerator.Current;
                }
            }
        }

        public static class StringEtensions
        {
            public static string ToTitleCase(this string input)
            {
                // Use the CultureInfo's TextInfo to convert to title case
                if (string.IsNullOrWhiteSpace(input)) return input;
                var textInfo = CultureInfo.CurrentCulture.TextInfo;
                return textInfo.ToTitleCase(input.ToLower());
            }
        }
    }

}
