using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "EdgeInfo", menuName = "Dice/Create edge")]
    public class Edge : ScriptableObject
    {
        public DiceEdgeSetter edgePrefab;
        public Material edgeMaterial;
        public EdgePattern edgePattern;
    }

    public enum EnumEdgeType
    {
        Attack,
        Armor,
        Vampire,
        Berserk,
        Reflection,
        Mana,
        Heal,
        Refresh,
        Skill,
        Empty,
        Heal_Armor,
        Spike,
        Stun,
        AttackAll
    }
}