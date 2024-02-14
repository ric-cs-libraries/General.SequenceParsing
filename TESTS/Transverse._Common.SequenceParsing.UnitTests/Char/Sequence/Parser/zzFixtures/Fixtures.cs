using System.Text.RegularExpressions;

using Transverse._Common.SequenceParsing.Char;
using Transverse._Common.SequenceParsing.Generic;

namespace Transverse._Common.SequenceParsing.UnitTests.Char;

internal class Fixtures
{
    internal static class Pathes
    {
        private const string Assets_SubPath = "._Assets";
        internal const string ParseResults_AsTxtFiles_SubPath = $"{Assets_SubPath}/_ParseResults_AsTxtFiles";
        internal const string FilesToParse_SubPath = $"{Assets_SubPath}/._FilesToParse";
    }

    internal static class CSharpCode
    {
        private static string espacesSautsLigneRegExp = "(" + Environment.NewLine + "|[ ])*";
        private static string accoladeOuvranteRegExp = espacesSautsLigneRegExp + "[{]" + espacesSautsLigneRegExp;
        private static string ifRegExp = "if[ ]*[(](.*)[)]" + accoladeOuvranteRegExp;
        private static string classRegExp = "class[ ]*[A-Z]+" + accoladeOuvranteRegExp;

        private static StringMatchingEvaluator stringMatchingEvaluatorByEquality = new StringMatchingEvaluatorByEquality();
        private static StringMatchingEvaluator stringMatchingEvaluatorByRegExp = new StringMatchingEvaluatorByRegExp();

        private static CharBlockDelimiter commentMultiLigStartDelimiter = CharBlockDelimiter.Create("/*", stringMatchingEvaluatorByEquality);
        private static CharBlockDelimiter commentMultiLigEndDelimiter = CharBlockDelimiter.Create("*/", stringMatchingEvaluatorByEquality);
        private static CharBlock commentMultiLig_ = CharBlock.Create(commentMultiLigStartDelimiter, commentMultiLigEndDelimiter);

        private static CharBlockDelimiter commentMonoLigStartDelimiter = CharBlockDelimiter.Create("//", stringMatchingEvaluatorByEquality);
        private static CharBlockDelimiter commentMonoLigEndDelimiter = CharBlockDelimiter.Create(Environment.NewLine, stringMatchingEvaluatorByEquality);
        private static CharBlock commentMonoLig_ = CharBlock.Create("Comm.MonoLig.", commentMonoLigStartDelimiter, commentMonoLigEndDelimiter);

        private static CharBlockDelimiter ifStartDelimiter = CharBlockDelimiter.Create(ifRegExp, stringMatchingEvaluatorByRegExp, false);
        private static CharBlockDelimiter ifEndDelimiter = CharBlockDelimiter.Create("}", stringMatchingEvaluatorByEquality);
        private static CharBlock if_ = CharBlock.Create("IF{...}", ifStartDelimiter, ifEndDelimiter,
                                                CharBlocks.Create(new List<CharBlock> { commentMultiLig_, commentMonoLig_ }), canBeNestedWithinItSelf: true);

        private static CharBlockDelimiter classStartDelimiter = CharBlockDelimiter.Create(classRegExp, stringMatchingEvaluatorByRegExp, false);
        private static CharBlockDelimiter classEndDelimiter = CharBlockDelimiter.Create("}", stringMatchingEvaluatorByEquality);
        private static CharBlock class_ = CharBlock.Create("CLASS {...}", classStartDelimiter, classEndDelimiter, CharBlocks.Create(new List<CharBlock> { commentMultiLig_, commentMonoLig_, if_ }));

        internal static CharBlocks expectedInnerBlocks = CharBlocks.Create(new List<CharBlock> {
            commentMultiLig_,
            commentMonoLig_,
            class_,
            if_
        });

    }

    internal static class XML
    {
        private static StringMatchingEvaluator stringMatchingEvaluatorByRegExp = new StringMatchingEvaluatorByRegExp();
        private static StringMatchingEvaluator stringMatchingEvaluatorByEquality = new StringMatchingEvaluatorByEquality();

        private static string tagPrefix = @"<[ ]*[A-Z0-9\-]+";
        private static string monotagOuvrantRegExp = $"{tagPrefix} ";
        private static string tagOuvrantRegExp = $"{tagPrefix}[ ]*>";  //Seul le tag ouvrant suffira, car celui de fin sera immédiatement calculé une fois celui de début trouvé.


        private static string attributStartRegExp = @"[ ]*[a-z0-9]+[=][\""]";
        private static CharBlockDelimiter attributStartDelimiter = CharBlockDelimiter.Create(attributStartRegExp, stringMatchingEvaluatorByRegExp, false);
        private static CharBlockDelimiter attributEndDelimiter = CharBlockDelimiter.Create("\"", stringMatchingEvaluatorByEquality, false);
        private static CharBlock attributBlock = CharBlock.Create("tagAttribute=\"...\"", attributStartDelimiter, attributEndDelimiter);


        private static CharBlockDelimiter monotagStartDelimiter = CharBlockDelimiter.Create(monotagOuvrantRegExp, stringMatchingEvaluatorByRegExp, false);
        private static CharBlockDelimiter monotagEndDelimiter = CharBlockDelimiter.Create("/>", stringMatchingEvaluatorByEquality, false);
        private static CharBlock monotag_ = CharBlock.Create("<monotag ... />", monotagStartDelimiter, monotagEndDelimiter,
                                                             expectedInnerBlocks: CharBlocks.Create(new List<CharBlock> { attributBlock }));
        //-- Ici on va exploiter un puissant mécanisme : EndDelimiter calculé en fonction du StartDelimiter réel trouvé (expr. régulière) --
        // (cf. notamment : méthode OnStartDelimiterMatching_MayNeedANewAdaptedBlock de la classe CharBlock).
        public static DelimiterComputer fComputeEndDelimiter = (string realStartDelimiter) =>   //realStartDelimiter donc ici de la forme : <Nimporte>
        {
            string endDelimiter = Regex.Replace(realStartDelimiter, "<", "</"); //On déduit le Delimiter de fin à partir de celui de début !
            return endDelimiter;
        };
        private static CharBlockDelimiter tagStartDelimiter = CharBlockDelimiter.Create(tagOuvrantRegExp, stringMatchingEvaluatorByRegExp, false);
        private static CharBlockDelimiter tagEndDelimiter = CharBlockDelimiter.Create(fComputeEndDelimiter, stringMatchingEvaluatorByEquality, false);
        private static CharBlock tag_ = CharBlock.Create("<tag>...</tag>", tagStartDelimiter, tagEndDelimiter,
                                                         expectedInnerBlocks: CharBlocks.Create(new List<CharBlock> { monotag_ }),
                                                         canBeNestedWithinItSelf: true);
        
        internal static CharBlocks expectedInnerBlocks = CharBlocks.Create(new List<CharBlock> { tag_, monotag_ });
    }

    internal static class LPDP_PoemsList
    {
        private const string NODE_TYPE_TITRE_SECTION = "TITRE_SECTION";
        private const string NODE_TYPE_TITRE_ECRIT = "TITRE_ECRIT";
        private const string NODE_TYPE_INFOS_ECRIT = "INFOS_ECRIT";

        private const string LINK_TAG_HREF_PATTERN = "<a href=\"(.*cat=[0-9]+)\">";


        private static StringMatchingEvaluator stringMatchingEvaluatorByRegExp = new StringMatchingEvaluatorByRegExp();
        private static StringMatchingEvaluator stringMatchingEvaluatorByEquality = new StringMatchingEvaluatorByEquality();


        private static CharBlockDelimiter aTitreEtUrlEcritStartDelimiter = CharBlockDelimiter.Create($"{LINK_TAG_HREF_PATTERN}", stringMatchingEvaluatorByRegExp, false);
        private static CharBlockDelimiter aTitreEtUrlEcritEndDelimiter = CharBlockDelimiter.Create("</a>", stringMatchingEvaluatorByEquality, false);
        private static CharBlock aTitreEtUrlEcrit = CharBlock.Create($"{NODE_TYPE_TITRE_ECRIT} <a href='urlecrit'>titreEcrit</a>", aTitreEtUrlEcritStartDelimiter, aTitreEtUrlEcritEndDelimiter);

        private static CharBlockDelimiter spanAutresInfosEcritStartDelimiter = CharBlockDelimiter.Create("<span class=\"medTxt\">", stringMatchingEvaluatorByEquality, false);
        private static CharBlockDelimiter spanAutresInfosEcritEndDelimiter = CharBlockDelimiter.Create("</span>", stringMatchingEvaluatorByEquality, false);
        private static CharBlock spanAutresInfosEcrit = CharBlock.Create($"{NODE_TYPE_INFOS_ECRIT} <span class='...'>autresInfos</span>", spanAutresInfosEcritStartDelimiter, spanAutresInfosEcritEndDelimiter);

        private static CharBlockDelimiter simpleTdStartDelimiter = CharBlockDelimiter.Create("<td>", stringMatchingEvaluatorByEquality, false);
        private static CharBlockDelimiter simpleTdEndDelimiter = CharBlockDelimiter.Create("</td>", stringMatchingEvaluatorByEquality, false);
        private static CharBlock simpleTd = CharBlock.Create("<td>...</td>", simpleTdStartDelimiter, simpleTdEndDelimiter,
                                                              expectedInnerBlocks: CharBlocks.Create(new List<CharBlock> { aTitreEtUrlEcrit, spanAutresInfosEcrit })
                                                             );

        private static CharBlockDelimiter tdTitreSectionStartDelimiter = CharBlockDelimiter.Create("<td class=\"msg0 msgTopBorderMed msgBottomBorderMed ti5-k\">", stringMatchingEvaluatorByEquality, false);
        private static CharBlockDelimiter tdTitreSectionEndDelimiter = CharBlockDelimiter.Create("</td>", stringMatchingEvaluatorByEquality, false);
        private static CharBlock tdTitreSection = CharBlock.Create($"{NODE_TYPE_TITRE_SECTION} <td class Titre section...>...</td>", tdTitreSectionStartDelimiter, tdTitreSectionEndDelimiter);

        private static CharBlockDelimiter trStartDelimiter = CharBlockDelimiter.Create("<tr>", stringMatchingEvaluatorByEquality, false);
        private static CharBlockDelimiter trEndDelimiter = CharBlockDelimiter.Create("</tr>", stringMatchingEvaluatorByEquality, false);
        private static CharBlock trBlock = CharBlock.Create("<tr>...</tr>", trStartDelimiter, trEndDelimiter,
                                                             expectedInnerBlocks: CharBlocks.Create(new List<CharBlock> { tdTitreSection, simpleTd })
                                                            );

        internal static CharBlocks expectedInnerBlocks = CharBlocks.Create(new List<CharBlock> { trBlock });

        internal static CharSequenceParser Parser(string stringToParse) => GetParser(
            stringToParse,
            sequenceStartsAfterRegExp: new Regex("<table class=\"msg1 msgTableHeaderMed\""),
            sequenceEndsAtRegExp: new Regex("<!-- END - Sous-tableau du centre")
        );
    }

    internal static CharSequenceParser GetParser(string stringToParse, int sequenceStartIndex=0, int? sequenceEndIndex= null)
    {
        CharSequence charSequenceToParse = CharSequence.Create(stringToParse, sequenceStartIndex, sequenceEndIndex);
        CharSequenceParser parser = GetParser(charSequenceToParse);
        return parser;
    }
    internal static CharSequenceParser GetParser(string stringToParse, Regex sequenceStartsAfterRegExp, int? sequenceEndIndex = null)
    {
        CharSequence charSequenceToParse = CharSequence.Create(stringToParse, sequenceStartsAfterRegExp, sequenceEndIndex);
        CharSequenceParser parser = GetParser(charSequenceToParse);
        return parser;
    }
    internal static CharSequenceParser GetParser(string stringToParse, int sequenceStartIndex, Regex sequenceEndsAtRegExp)
    {
        CharSequence charSequenceToParse = CharSequence.Create(stringToParse, sequenceStartIndex, sequenceEndsAtRegExp);
        CharSequenceParser parser = GetParser(charSequenceToParse);
        return parser;
    }

    internal static CharSequenceParser GetParser(string stringToParse, Regex sequenceStartsAfterRegExp, Regex sequenceEndsAtRegExp)
    {
        CharSequence charSequenceToParse = CharSequence.Create(stringToParse, sequenceStartsAfterRegExp, sequenceEndsAtRegExp);
        CharSequenceParser parser = GetParser(charSequenceToParse);
        return parser;
    }

    private static CharSequenceParser GetParser(CharSequence charSequenceToParse)
    {
        CharSequenceParser parser = CharSequenceParser.Create(charSequenceToParse);
        return parser;
    }

}
