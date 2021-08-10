using NL.Extensions;
using System;
using System.IO;

namespace NL.Utils {
    
    public static class NLFile {
        
        /// <summary>
        ///     Format the <see langword="string"/> to a filepath valid
        ///     for all OS.
        /// </summary>
        /// <param name="path">
        ///     The filepath to format.
        /// </param>
        public static string AsPath(this string path) {
            return path
                .Trim()
                .Replace('/', '\\');
        }

        /// <summary>
        ///     Extract the filename from the given filepath.
        /// </summary>
        /// <param name="path">
        ///     The path to extract the filename from.
        /// </param>
        /// <param name="keepExtension">
        ///     Whether the returned filename keeps its extension (if present).
        /// </param>
        /// <returns>
        ///     The found filename if present; an empty <see langword="string"/>
        ///     otherwise.
        /// </returns>
        public static string GetFilename(string path, bool keepExtension = true) {
            if(path.IsEmpty()) {
                return string.Empty;
            }

            int startIndex = path.LastIndexOf('\\') + 1;
            int endIndex = keepExtension || path.LastIndexOf('.') < startIndex
                ? path.Length
                : path.LastIndexOf('.');

            if(startIndex == -1) {
                return path;
            }else if(startIndex >= path.Length) {
                return string.Empty;
            }

            string filename = path[startIndex..endIndex];
            return filename;
        }

        /// <summary>
        ///     Extract the file extension from a given filepath.
        /// </summary>
        /// <param name="path">
        ///     The filepath to extract the extension from.
        /// </param>
        /// <returns>
        ///     The file's extension if present; an empty <see langword="string"/>
        ///     otherwise.
        /// </returns>
        public static string GetExtension(string path) {
            int startIndex = path.LastIndexOf('.') + 1;
            return startIndex <= 0
                ? string.Empty
                : path[startIndex..];
        }

        /// <summary>
        ///     Remove the filename and extension from the given filepath.
        /// </summary>
        /// <param name="path">
        ///     The filepath to get the directory of.
        /// </param>
        /// <returns>
        ///     The directory of the given <paramref name="path"/>.
        /// </returns>
        public static string GetDirectory(string path) {
            int endIndex = path.LastIndexOf('\\') + 1;
            string directory;
            if(endIndex == 0) {
                directory = string.Empty;
            } else {
                directory = endIndex == path.Length - 1
                    ? path
                    : path[..endIndex];
            }
            return directory;
        }

    }

}
