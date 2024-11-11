using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using UnityEngine;

namespace Core.Data
{
    public class BattleDiceData
    {
        public List<Edge> Edges;
        public Edge CurrentEdge { get; private set; }
        public Edge LastEdge { get; private set; }
        public DiceConfig Config { get; private set; }
        public EdgeColor[] TopEdgeColor { get; private set; }

        public BattleDiceData(DiceConfig config)
        {
            Config = config;
            Edges = new List<Edge>();
            Edges.AddRange(config.Edges);
            TopEdgeColor = new EdgeColor[2];
        }
        
        public Edge GetRandomEdge()
        {
            CurrentEdge = Edges[Random.Range(0, Edges.Count)];
            return CurrentEdge;
        }
        
        public void SetTargetEdge(EnumEdgeType targetEdge) => CurrentEdge = Edges.Find(edge => edge.edgePattern.edgeType == targetEdge);
        
        public void SetEdgeIndex(int edgeIndex) => CurrentEdge = Edges[edgeIndex];

        public void UpdateLastEdge()
        {
            LastEdge = CurrentEdge;
        }

        public void SetColor(EdgeColor color)
        {
            TopEdgeColor = new[] {color};
        }
        
        public void SetColor(EdgeColor[] color)
        {
            TopEdgeColor = color;
        }
    }
}