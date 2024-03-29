﻿using System.Diagnostics;

namespace General.SequenceParsing.Generic;


[DebuggerDisplay("Blocks - Nb. Block={List.Count}")]
public record Blocks<TTT>
{
    public List<Block<TTT>> List { get; init; } = new();

    protected Blocks(List<Block<TTT>> list)
    {
        List = list;
    }

    public static Blocks<TTT> Create(List<Block<TTT>> list)
    {
        Blocks<TTT> result = new Blocks<TTT>(list);
        return result;
    }

    private string GetElementsAsStringsList()
    {
        return $"({List.Count})['"+string.Join("', '", List.Select(b => b.Type!))+"']";
    }

    public string GetStateAsString()
    {
        List<string> state = new()
        {
            //$"{GetType().GetName_()}: ["+string.Join(", ",List.Select(b => b.GetStateAsString()))+"]"
            $"{GetType().GetName_()}: {GetElementsAsStringsList()}"
        };
        
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }
}
