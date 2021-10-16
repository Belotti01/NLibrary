using System;
using System.Collections.Generic;
using System.Linq;

namespace NL.Extensions {

    public static class Objects {

        /// <summary>
        ///     Finds whether the value is in the range between <paramref name="min"/> and
        ///     <paramref name="max"/> (exclusive).
        /// </summary>
        /// <param name="min">
        ///     The exclusive minimum value to compare to.
        /// </param>
        /// <param name="max">
        ///     The exclusive maximum value to compare to.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the value is between and not equal to
        ///     <paramref name="min"/> and <paramref name="max"/>.
        /// </returns>
        public static bool IsInRange<T>(this T value, T min, T max) where T : IComparable<T>
            => value.CompareTo(min) > 0 && value.CompareTo(max) < 0;

        /// <summary>
        ///     Finds whether the value is in the range between <paramref name="min"/> and
        ///     <paramref name="max"/> (inclusive).
        /// </summary>
        /// <param name="min">
        ///     The inclusive minimum value to compare to.
        /// </param>
        /// <param name="max">
        ///     The inclusive maximum value to compare to.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the value is in the range between or equal to
        ///     <paramref name="min"/> and <paramref name="max"/>.
        /// </returns>
        public static bool IsInRangeInclusive<T>(this T value, T min, T max) where T : IComparable<T>
            => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;

        /// <summary>
        ///     Create a copy of this <see cref="IEnumerable{T}"/> where all elements are
        ///     cast to <see cref="string"/>.
        /// </summary>
        /// <returns>
        ///     An array of <see cref="string"/>s, where all elements are the result of the 
        ///     call to <see cref="object.ToString"/> on the type <typeparamref name="T"/>.
        /// </returns>
        public static string[] ToStringArray<T>(this IEnumerable<T> values) {
            return values
                .Select(v => v.ToString())
                .ToArray();
        }

        /// <summary>
        ///     Compare this item to the default value of the type
        ///     <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> only if the value of this item
        ///     equals:
        ///     <list type="bullet">
        ///         <item>  
        ///                 <see langword="null"/> - if <typeparamref name="T"/> is 
        ///                 a class or a <see cref="Nullable"/> type;
        ///         </item>
        ///         <item>  
        ///                 <see langword="default"/> - if <typeparamref name="T"/> is a structure.
        ///         </item>
        ///     </list>
        /// </returns>
        public static bool IsNullOrDefault<T>(this T item)
            => EqualityComparer<T>.Default.Equals(item, default);

        /// <inheritdoc cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource)"/>
        public static bool In<T>(this T value, params T[] source)
            => source.Contains(value);
    }

}
