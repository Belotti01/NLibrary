﻿using System.Collections.Generic;
using System.Linq;

namespace NL.Extensions {

    public static class Enumerables {

        /// <summary>
        ///     Remove all <see langword="default"/> and <see langword="null"/> values
        ///     from the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>
        ///     A copy of the <see cref="IEnumerable{T}"/>, without <see langword="default"/> 
        ///     and <see langword="null"/> values
        /// </returns>
        public static IEnumerable<T> RemoveDefaults<T>(this IEnumerable<T> collection) {
            return collection
                .Where(item => !item.IsNullOrDefault());
        }

        /// <summary>
        ///     Remove all instances of <see langword="null"/> and <see cref="string.Empty"/>
        ///     from the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>
        ///     A copy of the <see cref="IEnumerable{T}"/>, without <see langword="null"/> 
        ///     and <see cref="string.Empty"/> values
        /// </returns>
        public static IEnumerable<string> RemoveDefaults(this IEnumerable<string> collection) {
            return collection
                .Where(item => !item.IsNullOrDefault() && item != string.Empty);
        }

        /// <summary>
        ///     Remove all instances of <see langword="null"/>, <see cref="string.Empty"/>,
        ///     and whitespace-only <see langword="string"/>s from the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>
        ///     A copy of the <see cref="IEnumerable{T}"/>, without <see langword="null"/>, 
        ///     <see cref="string.Empty"/> and whitespace-only <see langword="string"/> values
        /// </returns>
        public static IEnumerable<string> RemoveDefaultsAndWhitespace(this IEnumerable<string> collection) {
            return collection
                .Where(item => !(item.IsNullOrDefault() || item.IsEmpty()));
        }

        /// <summary>
        ///     Execute the <see cref="string.Trim(char[])"/> method on all <see langword="string"/>s
        ///     in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="trimChars">
        ///     The <see langword="char"/>s to trim from the <see langword="string"/>s.
        /// </param>
        /// <returns>
        ///     A copy of the <see cref="IEnumerable{T}"/> in which the <see cref="string.Trim(char[])"/>
        ///     method is executed for every present <see langword="string"/>.
        /// </returns>
        public static IEnumerable<string> TrimAll(this IEnumerable<string> collection, params char[] trimChars) {
            return collection.Select(s => s is null ? s : s.Trim(trimChars));
        }

        /// <summary>
        ///     Invert the order of values inside the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>
        ///     A copy of the <see cref="IEnumerable{T}"/> where the order of the
        ///     values is backwards.
        /// </returns>
        public static IEnumerable<T> Inverted<T>(this IEnumerable<T> collection) {
            long collectionSize = collection.LongCount();
            T[] invertedCollection = collection.ToArray();

            T temp;
            for(long i = 0; i < collectionSize / 2; i++) {
                temp = invertedCollection[i];
                invertedCollection[i] = invertedCollection[collectionSize - i - 1];
                invertedCollection[collectionSize - i - 1] = temp;
            }

            return invertedCollection;
        }

    }

}
