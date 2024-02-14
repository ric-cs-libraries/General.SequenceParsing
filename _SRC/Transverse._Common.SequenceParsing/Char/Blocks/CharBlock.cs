using Transverse._Common.SequenceParsing.Generic;

namespace Transverse._Common.SequenceParsing.Char;

public record CharBlock : Block<char>
{
    private CharBlock(
        string type,
        CharBlockDelimiter startDelimiter, CharBlockDelimiter endDelimiter,
        CharBlocks? expectedInnerBlocks = null, bool canBeNestedWithinItSelf = false)
        : base(type, startDelimiter, endDelimiter, expectedInnerBlocks, canBeNestedWithinItSelf)
    {
    }

    //canBeNestedWithinItSelf => si true, alors on envisage que le présent Block soit trouvable à l'intérieur de lui-même. (Ex. : if() { if() { if() {...} } })
    public static CharBlock Create(
        string type,
        CharBlockDelimiter startDelimiter, CharBlockDelimiter endDelimiter,
        CharBlocks? expectedInnerBlocks = null, bool canBeNestedWithinItSelf = false)
    {
        CharBlock result = new(type, startDelimiter, endDelimiter, expectedInnerBlocks, canBeNestedWithinItSelf);
        return result;
    }

    //canBeNestedWithinItSelf => si true, alors on envisage que le présent Block soit trouvable à l'intérieur de lui-même. (Ex. : if() { if() { if() {...} } })
    public static CharBlock Create(
        CharBlockDelimiter startDelimiter, CharBlockDelimiter endDelimiter,
        CharBlocks? expectedInnerBlocks = null, bool canBeNestedWithinItSelf = false)
    {
        string type = GetDefaultType(startDelimiter, endDelimiter);
        CharBlock result = Create(type, startDelimiter, endDelimiter, expectedInnerBlocks, canBeNestedWithinItSelf);
        return result;
    }


    public CharBlock GetClone(CharBlocks? expectedInnerBlocks = null, bool? canBeNestedWithinItSelf = null)
    {
        //REM.: StartDelimiter et EndDelimiter sont du type parent : BlockDelimiter<char>, d'où l'obligation de downcast explicite (maîtrisé).
        CharBlock result = GetClone((StartDelimiter as CharBlockDelimiter)!, (EndDelimiter as CharBlockDelimiter)!, expectedInnerBlocks, canBeNestedWithinItSelf);
        return result;
    }

    public CharBlock GetClone(string startDelimiter, string endDelimiter, CharBlocks? expectedInnerBlocks = null, bool? canBeNestedWithinItSelf = null)
    {
        CharBlockDelimiter charBlockStartDelimiter = (StartDelimiter as CharBlockDelimiter)!; //StartDelimiter étant du type parent : BlockDelimiter<char>
        CharBlockDelimiter charBlockEndDelimiter = (EndDelimiter as CharBlockDelimiter)!;
        bool canBeNestedWithinItSelf_ = (canBeNestedWithinItSelf is null) ? SelfNestable : canBeNestedWithinItSelf.Value;

        //StringMatchingEvaluator et CaseSensitive, n'appartenant qu'au type : CharBlockDelimiter
        CharBlockDelimiter newStartDelimiter = CharBlockDelimiter.Create(
            startDelimiter,
            charBlockStartDelimiter.StringMatchingEvaluator,
            charBlockStartDelimiter.CaseSensitive
        );
        CharBlockDelimiter newEndDelimiter = CharBlockDelimiter.Create(
            endDelimiter,
            charBlockEndDelimiter.StringMatchingEvaluator,
            charBlockEndDelimiter.CaseSensitive
        );
        CharBlock newBlock = GetClone(newStartDelimiter, newEndDelimiter, expectedInnerBlocks, canBeNestedWithinItSelf);
        return newBlock;
    }

    public CharBlock GetClone(CharBlockDelimiter startDelimiter, CharBlockDelimiter endDelimiter, CharBlocks? expectedInnerBlocks = null, bool? canBeNestedWithinItSelf = null)
    {
        //REM.: on ne clone pas d'emblée ExpectedInnerBlocks, car en pratique sans grand intérêt, ET risque de référence circulaire !

        CharBlocks? expectedInnerBlocks_ = expectedInnerBlocks ?? (ExpectedInnerBlocks as CharBlocks);
        bool canBeNestedWithinItSelf_ = (canBeNestedWithinItSelf is null) ? SelfNestable : canBeNestedWithinItSelf.Value;
        CharBlock result = CharBlock.Create(startDelimiter, endDelimiter, expectedInnerBlocks_, canBeNestedWithinItSelf_);
        return result;
    }


    //Lors du matching du Delimiter de début, on est SUSCEPTIBLE DE vouloir modifier, notamment, le Delimiter de fin, en fonction de la valeur exacte ayant matché.
    //Auquel cas, comme un Block (this) est un Value Object (immutable après création), on va en créer une COPIE modifiée et la renvoyer.
    //Cette modif. n'aura lieu ici QUE si ce EndDelimiter a son membre FComputeDelimiter renseigné.
    public override CharBlock OnStartDelimiterMatching_MayNeedANewAdaptedBlock(IReadOnlyList<char> startDelimiterMatchingContent)
    {
        CharBlockDelimiter EndDelimiter_ = (EndDelimiter as CharBlockDelimiter)!; //Car EndDelimiter est une property héritée du parent : Block<char>.

        if (EndDelimiter_.IsComputedDelimiter)
        {
            string realStartDelimiter = string.Join("", startDelimiterMatchingContent);
            string newStartDelimiter = realStartDelimiter;

            string newEndDelimiter = EndDelimiter_.FComputeDelimiter!(realStartDelimiter);

            var expectedInnerBlocks = (new List<CharBlock> { this });
            if (ExpectedInnerBlocks is not null  &&  ExpectedInnerBlocks.List.Any())
            {
                expectedInnerBlocks.AddRange(ExpectedInnerBlocks.List.Select(b=> (b as CharBlock)!));
            }
            CharBlocks? expectedInnerBlocks_ = (SelfNestable) ? CharBlocks.Create(expectedInnerBlocks.Distinct().ToList()) : null; //Si this.SelfNestable alors on gardera this et ses éventuels ExpectedInnerBlocks
                                                                                                                                   //en expectedInnerBlocks du nouveau Block result ci-dessous,
                                                                                                                                   //car c'est this (avec son EndDelimiter.FComputeDelimiter)
                                                                                                                                   //qui a permis le présent mécanisme auto.
            CharBlock result = GetClone(newStartDelimiter, newEndDelimiter, expectedInnerBlocks_, canBeNestedWithinItSelf: false); //canBeNestedWithinItSelf: false car pour 
                                                                                                                                   //le CharBlock result,
                                                                                                                                   //avoir this (et ses éventuels ExpectedInnerBlocks)
                                                                                                                                   //en expectedInnerBlocks suffira.
            return result;
        }
        return this;
    }

}
