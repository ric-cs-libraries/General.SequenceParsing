using Transverse._Common.SequenceParsing.Generic;

namespace Transverse._Common.SequenceParsing.Char;

//Pour du Debug en l'occurrence.
//Juste une façon rapide et simplifiée de parcourir le Tree, avec ici une vision à plat : myParseResult.RootNode.AcceptVisitor(new CharParseResultBasicVisitor())
public class CharParseResultBasicVisitor : IVisitor<char>
{
    public void Visit(Leaf<char> leaf)
    {
        Console.WriteLine($"  Leaf: {leaf.GetDataAsString()}");
    }

    public void Visit(Node<char> node)
    {
        Console.WriteLine($">NODE : {node.GetSimplifiedStateAsString()}");
    }
}
