using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "EdgePattern", menuName = "Dice/Create edge pattern")]
    public class EdgePattern : ScriptableObject
    {
        public Sprite edgeIcon;
        public string edgeDescription;
        public EnumEdgeType edgeType;
        public EdgeColor[] colors;
        [SerializeField] public List<Effect> _effects;
    }
}