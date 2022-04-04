using NL.Extensions;
using System.IO;
using System.Text;

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
            StringBuilder validPath = new();
            path = path
                .Trim()
                .Replace('/', '\\');
            bool isUNC = path.StartsWith(@"\\");
            bool hasRoot = Path.IsPathRooted(path);

            // Add '.' before relative paths, except for UNC-formatted strings.
            if(path.StartsWith(@"\")) {
                if(!isUNC) {
                    validPath.Append('.');
                }
            }else if(!(hasRoot || path.StartsWith(@"."))) {
                validPath.Append(@".\");
            }

            validPath.Append(path);

            return validPath.ToString();
        }

        /// <summary>
        ///     Create a new file in the specified <paramref name="filepath"/>
        ///     and immediately close the resulting <see cref="FileStream"/>.
        /// </summary>
        /// <param name="filepath">
        ///     The path including the file to create.
        /// </param>
        /// <param name="replaceExisting">
        ///     Whether to replace the file if it already exists.
        /// </param>
        public static void Create(string filepath, bool replaceExisting) {
            if(replaceExisting || !File.Exists(filepath)) {
                File.Create(filepath).Close();
            }
        }

        /// <summary>
        ///     Create a new file in the specified <paramref name="filepath"/>.
        /// </summary>
        /// <param name="filepath">
        ///     The path including the file to create.
        /// </param>
        /// <param name="mode">
        ///     How the <see cref="FileStream"/> should be opened.
        /// </param>
        /// <param name="overwrite">
        ///     Whether to overwrite the file if it already exists.
        /// </param>
        /// <returns>
        ///     A <see cref="FileStream"/> to the specified 
        ///     <paramref name="filepath"/>, with the <see cref="FileAccess"/>
        ///     rights necessary to the selected <paramref name="mode"/>.
        /// </returns>
        public static FileStream CreateAndOpenStream(string filepath, FileMode mode, bool overwrite = true) {
            if(overwrite || !File.Exists(filepath)) {
                File.Create(filepath).Close();
            }
            return OpenStream(filepath, mode);
        }

        /// <inheritdoc cref="CreateAndOpenStream(string, FileMode, bool)"/>
        /// <summary>
        ///     Open a <see cref="FileStream"/> to the specified <paramref name="filepath"/>.
        /// </summary>
        public static FileStream OpenStream(string filepath, FileMode mode) {
            return new FileStream(filepath, mode, GetRequiredAccess(mode));
        }

        /// <summary>
        ///     Retrieve the minimum required <see cref="FileAccess"/>
        ///     by the selected <paramref name="forMode"/>.
        /// </summary>
        /// <param name="forMode">
        ///     The target <see cref="FileMode"/>.
        /// </param>
        /// <returns>
        ///     The minimum required <see cref="FileAccess"/> to open a
        ///     <see cref="FileStream"/> in the specified 
        ///     <paramref name="forMode"/>.
        /// </returns>
        public static FileAccess GetRequiredAccess(FileMode forMode) {
            return forMode switch {
                FileMode.Append or
                FileMode.Truncate or
                FileMode.CreateNew or
                FileMode.Create => FileAccess.Write,
                FileMode.Open => FileAccess.ReadWrite,
                FileMode.OpenOrCreate => FileAccess.ReadWrite,
                _ => default
            };
        }

    }

}
