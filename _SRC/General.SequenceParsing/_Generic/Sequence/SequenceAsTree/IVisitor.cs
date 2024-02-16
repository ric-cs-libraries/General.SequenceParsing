namespace General.SequenceParsing.Generic;

public interface IVisitor<TTT>
{
    void Visit(Leaf<TTT> leaf);
    void Visit(Node<TTT> node);
}
