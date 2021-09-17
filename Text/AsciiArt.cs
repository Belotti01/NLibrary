using NL.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NL.Text {
    public static class AsciiArt {
        private static readonly char[] Characters;
        private static readonly string AsciiFontsFolder;

        static AsciiArt() {
            Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            string libraryFolder = Path.GetDirectoryName(typeof(AsciiArt).Assembly.Location);
            AsciiFontsFolder = Path.Combine(libraryFolder, "Text", "AsciiArtFonts");
        }

        /// <summary>
        ///     Convert plain text (character range [A-Za-z0-9]) to
        ///     ASCII art of the specified <paramref name="font"/>,
        ///     formatted following the chosen <paramref name="hMode"/>
        ///     and <paramref name="vMode"/>.
        /// </summary>
        /// <param name="text">
        ///     The text to convert to ASCII art.
        /// </param>
        /// <param name="font">
        ///     The ASCII art font to convert the <paramref name="text"/> to.
        /// </param>
        /// <param name="hMode">
        ///     The horizontal spacing mode.
        /// </param>
        /// <param name="vMode">
        ///     The vertical spacing mode.
        /// </param>
        /// <returns>
        ///     A <see langword="string"/> containing the ASCII art version
        ///     of the entered <paramref name="text"/> in the selected
        ///     <paramref name="font"/>, spaced following the configured
        ///     <paramref name="hMode"/> and <paramref name="vMode"/>.
        /// </returns>
        public static string FromText(string text, AsciiFont font, AsciiHorizontalMode hMode = AsciiHorizontalMode.Spaced, AsciiVerticalMode vMode = AsciiVerticalMode.Spaced) {
            text = text.Replace("\t", "    ");
            string[] fontFileLines = ReadFontFile(font);

            Dictionary<char, string[]> asciiCharacters = new();
            int charHeight = fontFileLines.FindIndexWhere(x => x.IsWhiteSpace());
            
            string[] asciiChar;
            int charNumber = -1;
            int currentCharWidth, maxCharWidth, startRow, i;

            // Save ASCII font in structure
            foreach(char c in Characters) {
                charNumber++;
                // Skip unused characters
                if(!text.Contains(c))
                    continue;

                asciiChar = new string[charHeight];
                startRow = (charHeight + 1) * charNumber;
                maxCharWidth = 0;
                if(hMode is AsciiHorizontalMode.Trimmed or AsciiHorizontalMode.Spaced) {
                    // Calculate the width of the next char
                    for(i = startRow; i < startRow + charHeight; i++) {
                        currentCharWidth = fontFileLines[i].TrimEnd().Length;
                        if(currentCharWidth > maxCharWidth) {
                            maxCharWidth = currentCharWidth;
                        }
                    }
                }

                switch(hMode) {
                    case AsciiHorizontalMode.Block:
                        // Leave the ASCII char as-is, so all characters are of the same width
                        for(i = startRow; i < startRow + charHeight; i++) {
                            asciiChar[i - startRow] = fontFileLines[i];
                        }
                        break;
                    case AsciiHorizontalMode.Trimmed:
                        // Trim all whitespace between characters
                        for(i = startRow; i < startRow + charHeight; i++) {
                            asciiChar[i - startRow] = fontFileLines[i][..(Math.Min(maxCharWidth, fontFileLines[i].Length))];
                        }
                        break;
                    case AsciiHorizontalMode.Spaced:
                        // Leave exactly 1 whitespace between each character
                        for(i = startRow; i < startRow + charHeight; i++) {
                            asciiChar[i - startRow] = $"{fontFileLines[i][..(Math.Min(maxCharWidth, fontFileLines[i].Length))]} ";
                        }
                        break;
                }
                asciiCharacters.Add(c, asciiChar);
            }

            if(text.Contains(' ')) {
                //Add space character
                asciiChar = new string[charHeight];
                int spaceWidth = asciiCharacters.First().Value[0].Length;
                if(hMode is not AsciiHorizontalMode.Block) {
                    spaceWidth /= 2;
                }
                string spaceLine = new StringBuilder().Append(' ', spaceWidth).ToString();
                for(i = 0; i < charHeight - 1; i++) {
                    asciiChar[i] = spaceLine;
                }
                asciiCharacters.Add(' ', asciiChar);
            }

            // Create ASCII text
            string[] textLines = text.Split('\n');
            int verticalSpace = vMode switch {
                AsciiVerticalMode.Stacked => 0,
                AsciiVerticalMode.Spaced => 1,
                AsciiVerticalMode.Wide => 2,
                _ => 0
            };
            StringBuilder ascii = new();
            foreach(string line in textLines) {
                for(int row = 0; row < charHeight; row++) {
                    foreach(char c in line) {
                        // Skip unavailable characters
                        if(Characters.Contains(c) || c == ' ') {
                            ascii.Append(asciiCharacters[c][row]);
                        }
                    }
                    if(row != charHeight - 1) {
                        ascii.Append('\n');
                    }
                }
                ascii.Append('\n', verticalSpace);
            }

            return ascii.ToString();
        }

        private static string[] ReadFontFile(AsciiFont font) {
            string fontFile = Path.Combine(AsciiFontsFolder, $"{font}.txt");
            string[] fontFileLines = File.ReadAllLines(fontFile)
                .SkipWhile(x => x.IsWhiteSpace())
                .ToArray();
            return fontFileLines;
        }
    }

    public enum AsciiFont {
        Bloody,
        Moscow,
        Poison,
        RowanCap
    }

    public enum AsciiHorizontalMode {
        /// <summary>
        ///     Characters are as close as possible.
        /// </summary>
        Trimmed,
        /// <summary>
        ///     Characters are all of the same size.
        /// </summary>
        Block,
        /// <summary>
        ///     Characters are divided by exactly 1 empty column.
        /// </summary>
        Spaced
    }

    public enum AsciiVerticalMode {
        /// <summary>
        ///     Characters have no space on top nor on the bottom.
        /// </summary>
        Stacked,
        /// <summary>
        ///     Characters are spaced by 1 empty row between each line.
        /// </summary>
        Spaced,
        /// <summary>
        ///     Characters are spaced by 2 empty rows between each line.
        /// </summary>
        Wide
    }
}
