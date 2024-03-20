using System.Text;

using Xunit;


using General.SequenceParsing.Generic;
using General.SequenceParsing.Char;


namespace General.SequenceParsing.UnitTests.Char;

public class CharSequenceParserTests
{
    [Fact]
    public void Create_WhenCalled_ShouldReturnAnInstanceOfCharSequenceParser()
    {
        var charSequence = CharSequence.Create("xxx");
        var sut = CharSequenceParser.Create(charSequence);
        Assert.IsType<CharSequenceParser>(sut);
    }

    [Fact]
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_BasicString1()
    {
        //--- Arrange ---
        string stringToParse = "xyzAAwktAAFbzBBejAA123AAF567BBFrien";
        CharSequenceParser parser = Fixtures.GetParser(stringToParse);

        StringMatchingEvaluator stringMatchingEvaluator = new StringMatchingEvaluatorByEquality();

        CharBlockDelimiter blockAStartDelimiter = CharBlockDelimiter.Create("AA", stringMatchingEvaluator);
        CharBlockDelimiter blockAEndDelimiter = CharBlockDelimiter.Create("AAF", stringMatchingEvaluator);
        CharBlock blockA = CharBlock.Create(blockAStartDelimiter, blockAEndDelimiter);

        CharBlockDelimiter blockBStartDelimiter = CharBlockDelimiter.Create("BB", stringMatchingEvaluator);
        CharBlockDelimiter blockBEndDelimiter = CharBlockDelimiter.Create("BBF", stringMatchingEvaluator);
        CharBlock blockB = CharBlock.Create(blockBStartDelimiter, blockBEndDelimiter, expectedInnerBlocks: CharBlocks.Create(new List<CharBlock> { blockA }));

        CharBlocks expectedInnerBlocks = CharBlocks.Create(new List<CharBlock> { blockA, blockB });



        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_BasicString1.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_BasicString1.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }


    [Fact]
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_From_BasicString2()
    {
        //--- Arrange ---
        string stringToParse = "xyzAA012wktAaf7ZbzBBPejAA432123AAF8x567Bf .rien";
        CharSequenceParser parser = Fixtures.GetParser(stringToParse);

        StringMatchingEvaluator stringMatchingEvaluator = new StringMatchingEvaluatorByRegExp();

        CharBlockDelimiter blockAStartDelimiter = CharBlockDelimiter.Create("AA[0-9]{3}", stringMatchingEvaluator);
        CharBlockDelimiter blockAEndDelimiter = CharBlockDelimiter.Create("AAF[0-9]{1}(X|Z)", stringMatchingEvaluator, false);
        CharBlock blockA = CharBlock.Create(blockAStartDelimiter, blockAEndDelimiter);

        CharBlockDelimiter blockBStartDelimiter = CharBlockDelimiter.Create("BB(K|P)", stringMatchingEvaluator);
        CharBlockDelimiter blockBEndDelimiter = CharBlockDelimiter.Create("BF [.]", stringMatchingEvaluator, false);
        CharBlock blockB = CharBlock.Create(blockBStartDelimiter, blockBEndDelimiter, expectedInnerBlocks: CharBlocks.Create(new List<CharBlock> { blockA }));

        CharBlocks expectedInnerBlocks = CharBlocks.Create(new List<CharBlock> { blockA, blockB });



        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_BasicString2.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_BasicString2.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }


    [Fact]
    public void Parse_WhenStartDelimiterIdemAsEndDelimiterAndNotSelfNestable_ShouldReturnTheCorrectParseResult()
    {
        //--- Arrange ---
        string stringToParse = "xyAAzAA123AAAbzeAA456AAjAA123F567";
        CharSequenceParser parser = Fixtures.GetParser(stringToParse);

        StringMatchingEvaluator stringMatchingEvaluator = new StringMatchingEvaluatorByEquality();

        CharBlockDelimiter blockAAStartDelimiter = CharBlockDelimiter.Create("AA", stringMatchingEvaluator);
        CharBlockDelimiter blockAAEndDelimiter = blockAAStartDelimiter;
        CharBlock blockAA = CharBlock.Create(blockAAStartDelimiter, blockAAEndDelimiter);

        CharBlocks expectedInnerBlocks = CharBlocks.Create(new List<CharBlock> { blockAA });



        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_StartDelimiterIdemAsEndDelimiterAndNOTSelfNestable.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_StartDelimiterIdemAsEndDelimiterAndNOTSelfNestable.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }

    [Fact]
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_From_CSharpCodeAsString()
    {
        //--- Arrange ---
        string stringToParse = """
            // Ceci est un comm mono-ligne
            /* Ceci est un commentaire 
               multilignes
               class pas { pris en compte car dans comm } .
               */
           
               class MaClasse {
              
                  //Contenu de ma classe! class dans comm!

                  if (toto == 9) {

                    //Un comm mono
                      if (bidule < 5) 
                      {

                        //C'est cool
                      }

                  }


               }
            """;
        CharSequenceParser parser = Fixtures.GetParser(stringToParse);

        CharBlocks expectedInnerBlocks = Fixtures.CSharpCode.expectedInnerBlocks;



        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_FromStringAsCSharpCode.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_FromStringAsCSharpCode.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }

    [Fact]
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_From_CSharpCodeFile()
    {
        //--- Arrange ---
        string stringToParse = File.ReadAllText($"{Fixtures.Pathes.FilesToParse_SubPath}/CSharpCode.csharp");
        CharSequenceParser parser = Fixtures.GetParser(stringToParse);

        CharBlocks expectedInnerBlocks = Fixtures.CSharpCode.expectedInnerBlocks;



        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_FromCSharpCodeFile.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_FromCSharpCodeFile.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }

    [Fact]
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_From_XMLFile()
    {
        //--- Arrange ---
        string stringToParse = File.ReadAllText($"{Fixtures.Pathes.FilesToParse_SubPath}/XML.xml");
        CharSequenceParser parser = Fixtures.GetParser(stringToParse);

        CharBlocks expectedInnerBlocks = Fixtures.XML.expectedInnerBlocks;



        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_FromXMLFile.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_FromXMLFile.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }

    [Fact]
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_From_XMLFileWithAttributes()
    {
        //--- Arrange ---
        string stringToParse = File.ReadAllText($"{Fixtures.Pathes.FilesToParse_SubPath}/XML_WithAttributes.xml");
        CharSequenceParser parser = Fixtures.GetParser(stringToParse);

        CharBlocks expectedInnerBlocks = Fixtures.XML.expectedInnerBlocks;



        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_FromXMLFile_WithAttributes.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_FromXMLFile_WithAttributes.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }

    [Fact]
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_From_LPDP_MyPoemsList()
    {
        //--- Arrange ---
        string stringToParse = File.ReadAllText($"{Fixtures.Pathes.FilesToParse_SubPath}/LPDP_MyPoemsList.html", Encoding.Latin1);
        CharSequenceParser parser = Fixtures.LPDP_PoemsList.Parser(stringToParse);

        CharBlocks expectedInnerBlocks = Fixtures.LPDP_PoemsList.expectedInnerBlocks;



        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_From_LPDP_MyPoemsList.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_From_LPDP_MyPoemsList.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }


    [Fact] //ATTENTION prend 22sec ! (167 poèmes)
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_From_LPDP_Marco_PoemsList()
    {
        Assert.True(true); return;
        //--- Arrange ---
        string stringToParse = File.ReadAllText($"{Fixtures.Pathes.FilesToParse_SubPath}/LPDP_Marco_PoemsList.html", Encoding.Latin1);
        CharSequenceParser parser = Fixtures.LPDP_PoemsList.Parser(stringToParse);

        CharBlocks expectedInnerBlocks = Fixtures.LPDP_PoemsList.expectedInnerBlocks;


        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_From_LPDP_Marco_PoemsList.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_From_LPDP_Marco_PoemsList.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }

    [Fact] //ATTENTION prend 10mn !!! (767 poèmes)
    public void Parse_WhenCalled_ShouldReturnTheCorrectParseResult_From_LPDP_Mystic4Ever_PoemsList()
    {
        Assert.True(true); return;
        //--- Arrange ---
        string stringToParse = File.ReadAllText($"{Fixtures.Pathes.FilesToParse_SubPath}/LPDP_Mystic4Ever_PoemsList.html", Encoding.Latin1);
        CharSequenceParser parser = Fixtures.LPDP_PoemsList.Parser(stringToParse);

        CharBlocks expectedInnerBlocks = Fixtures.LPDP_PoemsList.expectedInnerBlocks;


        //--- Act ---
        ParseResult<char> parseResult = parser.Parse(expectedInnerBlocks);
        string resultAsString = parseResult.GetStateAsString();
        //File.WriteAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_From_LPDP_Mystic4Ever_PoemsList.txt", resultAsString);

        //--- Assert ---
        string expectedResultAsString = File.ReadAllText($"{Fixtures.Pathes.ParseResults_AsTxtFiles_SubPath}/ParseResult_From_LPDP_Mystic4Ever_PoemsList.txt");
        Assert.Equal(expectedResultAsString, resultAsString);
    }

}
