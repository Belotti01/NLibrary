﻿using NL.Extensions;
using System.Collections.Generic;

namespace NL.Text {
    /// <summary>
    ///     Methods to modify the characters contained in a <see langword="string"/>.
    /// </summary>
    public static class Transform {
        private const string BaseAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string BaseAlphabetAndNumbers = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string BaseAlphabetNumbersAndSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890./-=[];',<>?:|{}!@#$%^&*()_+";

        private static readonly Dictionary<TextType, TextTypeConverter> Converters = new() {
            { TextType.Fancy, new(BaseAlphabetAndNumbers, "𝒶𝒷𝒸𝒹𝑒𝒻𝑔𝒽𝒾𝒿𝓀𝓁𝓂𝓃𝑜𝓅𝓆𝓇𝓈𝓉𝓊𝓋𝓌𝓍𝓎𝓏𝒜𝐵𝒞𝒟𝐸𝐹𝒢𝐻𝐼𝒥𝒦𝐿𝑀𝒩𝒪𝒫𝒬𝑅𝒮𝒯𝒰𝒱𝒲𝒳𝒴𝒵𝟣𝟤𝟥𝟦𝟧𝟨𝟩𝟪𝟫𝟢") },
            { TextType.FancyBold, new(BaseAlphabetAndNumbers, "𝓪𝓫𝓬𝓭𝓮𝓯𝓰𝓱𝓲𝓳𝓴𝓵𝓶𝓷𝓸𝓹𝓺𝓻𝓼𝓽𝓾𝓿𝔀𝔁𝔂𝔃𝓐𝓑𝓒𝓓𝓔𝓕𝓖𝓗𝓘𝓙𝓚𝓛𝓜𝓝𝓞𝓟𝓠𝓡𝓢𝓣𝓤𝓥𝓦𝓧𝓨𝓩𝟣𝟤𝟥𝟦𝟧𝟨𝟩𝟪𝟫𝟢") },
            { TextType.Gothic, new(BaseAlphabet, "𝔞𝔟𝔠𝔡𝔢𝔣𝔤𝔥𝔦𝔧𝔨𝔩𝔪𝔫𝔬𝔭𝔮𝔯𝔰𝔱𝔲𝔳𝔴𝔵𝔶𝔷𝔄𝔅ℭ𝔇𝔈𝔉𝔊ℌℑ𝔍𝔎𝔏𝔐𝔑𝔒𝔓𝔔ℜ𝔖𝔗𝔘𝔙𝔚𝔛𝔜ℨ") },
            { TextType.GothicBold, new(BaseAlphabet, "𝖆𝖇𝖈𝖉𝖊𝖋𝖌𝖍𝖎𝖏𝖐𝖑𝖒𝖓𝖔𝖕𝖖𝖗𝖘𝖙𝖚𝖛𝖜𝖝𝖞𝖟𝕬𝕭𝕮𝕯𝕰𝕱𝕲𝕳𝕴𝕵𝕶𝕷𝕸𝕹𝕺𝕻𝕼𝕽𝕾𝕿𝖀𝖁𝖂𝖃𝖄𝖅") },
            { TextType.Outline, new(BaseAlphabetNumbersAndSymbols, "𝕒𝕓𝕔𝕕𝕖𝕗𝕘𝕙𝕚𝕛𝕜𝕝𝕞𝕟𝕠𝕡𝕢𝕣𝕤𝕥𝕦𝕧𝕨𝕩𝕪𝕫𝔸𝔹ℂ𝔻𝔼𝔽𝔾ℍ𝕀𝕁𝕂𝕃𝕄ℕ𝕆ℙℚℝ𝕊𝕋𝕌𝕍𝕎𝕏𝕐ℤ𝟙𝟚𝟛𝟜𝟝𝟞𝟟𝟠𝟡𝟘./-=[]⨟❜,<>❔:|{}❕@#$%^&*()_+") },
            { TextType.FullWidth, new(BaseAlphabetNumbersAndSymbols, "ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ１２３４５６７８９０．／－＝[]；＇，<>？：|{}！＠＃＄％^＆＊（）_＋") },
            { TextType.Circled, new(BaseAlphabetAndNumbers, "ⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏ①②③④⑤⑥⑦⑧⑨⓪") },
            { TextType.UpsideDown, new(BaseAlphabetNumbersAndSymbols.Inverted(), "+‾()*⅋^%$#@¡{}|:¿<>ʻ╻;[]=-/.068𝘓95ߤ↋↊⇂Z⅄XϺɅՈꓕSꓤꝹԀONꟽ⅂ꓘᒋIH⅁ᖵƎᗡϽꓭ∀zʎxʍʌnʇsɹbdouɯʅʞɾᴉɥƃⅎǝpɔqɐ") }
        };

        /// <summary>
        ///     Replace available characters inside the <paramref name="baseString"/> to
        ///     their <paramref name="textType"/>'s counterpart, or keep them as-is if not
        ///     available.
        /// </summary>
        /// <param name="baseString">
        ///     The <see langword="string"/> to convert.
        /// </param>
        /// <param name="textType">
        ///     The <see cref="TextType"/> to convert to.
        /// </param>
        /// <returns>
        ///     A <see langword="string"/> where all characters are converted to the specified
        ///     <see cref="TextType"/>, or kept the same if not convertible.
        /// </returns>
        public static string Convert(string baseString, TextType textType) {
            return Converters[textType].Convert(baseString);
        }
    }

    public enum TextType {
        Fancy,
        FancyBold,
        Gothic,
        GothicBold,
        Outline,
        Circled,
        FullWidth,
        UpsideDown
    }
}
