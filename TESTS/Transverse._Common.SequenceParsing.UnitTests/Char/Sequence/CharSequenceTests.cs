using System.Text.RegularExpressions;
using Xunit;

using Transverse._Common.SequenceParsing.Generic;
using Transverse._Common.SequenceParsing.Char;
using Transverse._Common.General.Basics.Exceptions;


namespace Transverse._Common.SequenceParsing.UnitTests.Char;

public class CharSequenceTests
{
    [Fact]
    public void Create_WhenCalled_ShouldReturnAnInstanceOfCharSequence()
    {
        var charSequence = CharSequence.Create("0123456789");
        Assert.IsType<CharSequence>(charSequence);
    }

    [Fact]
    public void Create_WhenCalled_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var stringSequence = "0123456789";
        var charSequence = CharSequence.Create(stringSequence);

        var result = charSequence.ToString();

        var expected = stringSequence;
        Assert.Equal(expected, result);
    }

    #region Create with startIndex or startAfterRegExp   ONLY
    [Fact]
    public void Create_WhenStartIndexPassedAsInt_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var stringSequence = "0123456789";
        var startIndex = 5;
        var charSequence = CharSequence.Create(stringSequence, startIndex);

        var result = charSequence.ToString();

        var expected = stringSequence.AsSpan(startIndex);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_WhenStartIndexPassedAsRegExp_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var afterMatch = "XyabczKAbc";
        var stringSequence = $"0123456789ABc986{afterMatch}";
        var startIndexRegExp = new Regex("abC[0-9]+", RegexOptions.IgnoreCase);
        var charSequence = CharSequence.Create(stringSequence, startIndexRegExp);

        var result = charSequence.ToString();

        var expected = afterMatch;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0123456789ABcZXyabczKAbc", "abC[0-9]+")]
    [InlineData("0123456789", "zz")]
    public void Create_WhenStartIndexPassedAsRegExpAndHasNoMatch_ShouldInitializeInstanceOfCharSequenceWithTheFullString(string stringSequence, string startIndexRegExp)
    {
        var startIndexRegExp_ = new Regex(startIndexRegExp, RegexOptions.IgnoreCase);
        var charSequence = CharSequence.Create(stringSequence, startIndexRegExp_);

        var result = charSequence.ToString();

        var expected = stringSequence;
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_WhenStartIndexIsOverTheLastPossibleIndex_ShouldThrowAnUnexistingChunkException()
    {
        var stringSequence = "0123456789";
        var startIndex = stringSequence.Length + 1;

        var ex = Assert.Throws<UnexistingChunkException>(() => CharSequence.Create(stringSequence, startIndex));
    }

    [Fact]
    public void Create_WhenStartIndexIsNegative_ShouldThrowAnUnexistingChunkException()
    {
        var stringSequence = "0123456789";
        var startIndex = -1;

        var ex = Assert.Throws<UnexistingChunkException>(() => CharSequence.Create(stringSequence, startIndex));
    }
    #endregion Create with startIndex or startAfterRegExp   ONLY


    #region Create with endIndex or endAtRegExp   ONLY
    [Fact]
    public void Create_WhenEndIndexPassedAsInt_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var stringSequence = "0123456789";
        var startIndex = 0;
        var endIndex = 5;
        var charSequence = CharSequence.Create(stringSequence, startIndex, endIndex);

        var result = charSequence.ToString();

        var expected = stringSequence.AsSpan(startIndex, endIndex - startIndex + 1);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_WhenEndIndexPassedAsRegExp_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var beforeMatch = "0123456789";
        var stringSequence = $"{beforeMatch}ABc986AzWku6";
        var startIndex = 0;
        var endIndexRegExp = new Regex("abC[0-9]+", RegexOptions.IgnoreCase);
        var charSequence = CharSequence.Create(stringSequence, startIndex, endIndexRegExp);

        var result = charSequence.ToString();

        var expected = beforeMatch;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0123456789ABcZXyabczKAbc", "abC[0-9]+")]
    [InlineData("0123456789", "zz")]
    public void Create_WhenEndIndexPassedAsRegExpAndHasNoMatch_ShouldInitializeInstanceOfCharSequenceTillTheEndOfTheString(string stringSequence, string endIndexRegExp)
    {
        var startIndex = 0;
        var endIndexRegExp_ = new Regex(endIndexRegExp, RegexOptions.IgnoreCase);
        var charSequence = CharSequence.Create(stringSequence, startIndex, endIndexRegExp_);

        var result = charSequence.ToString();

        var expected = stringSequence.AsSpan(startIndex);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_WhenEndIndexIsOverTheLastPossibleIndex_ShouldThrowAnUnexistingChunkException()
    {
        var stringSequence = "0123456789";
        var startIndex = 0;
        var endIndex = stringSequence.Length + 1;

        var ex = Assert.Throws<UnexistingChunkException>(() => CharSequence.Create(stringSequence, startIndex, endIndex));
    }

    [Fact]
    public void Create_WhenEndIndexIsNegative_ShouldThrowAnUnexistingChunkException()
    {
        var stringSequence = "0123456789";
        var startIndex = 0;
        var endIndex = -1;

        var ex = Assert.Throws<UnexistingChunkException>(() => CharSequence.Create(stringSequence, startIndex, endIndex));
    }
    #endregion Create with endIndex or endAtRegExp   ONLY



    #region Create with (startIndex or startAfterRegExp)   AND   (endIndex or endAtRegExp)   not generating Exceptions
    [Fact]
    public void Create_WhenStartIndexAndEndIndexPassedAsInt_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var stringSequence = "0123456789";
        var startIndex = 2;
        var endIndex = 5;
        var charSequence = CharSequence.Create(stringSequence, startIndex, endIndex);

        var result = charSequence.ToString();

        var expected = stringSequence.AsSpan(startIndex, endIndex - startIndex + 1);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_WhenStartIndexPassedAsIntAndEndIndexPassedAsRegExp_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var beforeMatch = "0123456789";
        var stringSequence = $"{beforeMatch}ABc986AzWku6";
        var startIndex = 4;
        var endIndexRegExp = new Regex("abC[0-9]+", RegexOptions.IgnoreCase);
        var charSequence = CharSequence.Create(stringSequence, startIndex, endIndexRegExp);

        var result = charSequence.ToString();

        var expected = beforeMatch.AsSpan(startIndex);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_WhenStartIndexPassedAsRegExpAndEndIndexPassedAsRegExp_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var betweenMatch = "K123456789";
        var stringSequence = $"zkxW78Abc2{betweenMatch}ABe986AzWku6";
        var startIndexRegExp = new Regex("abC[0-9]+", RegexOptions.IgnoreCase); ;
        var endIndexRegExp = new Regex("abE[0-9]+", RegexOptions.IgnoreCase);
        var charSequence = CharSequence.Create(stringSequence, startIndexRegExp, endIndexRegExp);

        var result = charSequence.ToString();

        var expected = betweenMatch;
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_WhenStartIndexPassedAsRegExpAndEndIndexPassedAsInt_ShouldReturnACorrectlyInitializedInstanceOfCharSequence()
    {
        var afterMatch = "K123456789ABe986AzWku6";
        var stringSequence = $"zkxW78Abc2{afterMatch}";
        var startIndexRegExp = new Regex("abC[0-9]+", RegexOptions.IgnoreCase); ;
        var endIndex = 14;
        var charSequence = CharSequence.Create(stringSequence, startIndexRegExp, endIndex);

        var result = charSequence.ToString();

        var startIndexFound = 10;
        var expected = stringSequence.AsSpan(startIndexFound, endIndex - startIndexFound + 1);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_WhenEndIndexPassedAsIntEqualToStartIndexPassedAsInt_ShouldReturnACorrectlyInitializedInstanceOfCharSequenceWithLengthOf1()
    {
        var stringSequence = "0123456789";
        var startIndex = 5;
        var endIndex = startIndex;
        var charSequence = CharSequence.Create(stringSequence, startIndex, endIndex);

        var result = charSequence.ToString();

        var expected = stringSequence.AsSpan(startIndex, endIndex - startIndex + 1);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0123456789ABcZXyabczKAbc", "abC[0-9]+")]
    [InlineData("0123456789", "zz")]
    public void Create_WhenStartIndexPassedAsIntAndEndIndexPassedAsRegExpAndHasNoMatch_ShouldInitializeInstanceOfCharSequenceTillTheEndOfTheString(string stringSequence, string endIndexRegExp)
    {
        var startIndex = 4;
        var endIndexRegExp_ = new Regex(endIndexRegExp, RegexOptions.IgnoreCase);
        var charSequence = CharSequence.Create(stringSequence, startIndex, endIndexRegExp_);

        var result = charSequence.ToString();

        var expected = stringSequence.AsSpan(startIndex);
        Assert.Equal(expected, result);
    }
    #endregion Create with (startIndex or startAfterRegExp)   AND   (endIndex or endAtRegExp)   not generating Exceptions


    #region Create with (startIndex or startAfterRegExp)   AND   (endIndex or endAtRegExp)   generating Exceptions
    [Fact]
    public void Create_WhenEndIndexPassedAsIntAndIsUnderStartIndexPassedAsInt_ShouldThrowAnUnexistingChunkException()
    {
        var stringSequence = "0123456789";
        var startIndex = 5;
        var endIndex = 3;

        var ex = Assert.Throws<UnexistingChunkException>(() => CharSequence.Create(stringSequence, startIndex, endIndex));
    }

    [Fact]
    public void Create_WhenEndIndexPassedAsRegExpAndStartIndexPassedAsRegExpWithEndIndexBeforeStartIndex_ShouldThrowAnUnexistingChunkException()
    {
        var betweenMatch = "K123456789";
        var stringSequence = $"zkxW78Abc2{betweenMatch}ABe986AzWku6";
        var endIndexRegExp = new Regex("abC[0-9]+", RegexOptions.IgnoreCase);
        var startIndexRegExp = new Regex("abE[0-9]+", RegexOptions.IgnoreCase);

        var ex = Assert.Throws<UnexistingChunkException>(() => CharSequence.Create(stringSequence, startIndexRegExp, endIndexRegExp));
    }
    #endregion Create with (startIndex or startAfterRegExp)   AND   (endIndex or endAtRegExp)   generating Exceptions
}
