using System.Collections.Generic;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Data/Battle/Battle")]
    public class BattleData : ScriptableObject
    {
        [SerializeField] private int expiriense;
        [SerializeField] private List<EnemyConfig> enemies;
        [SerializeReference] private List<Loot> loot;

        public int Expirience => expiriense;
        public List<Loot> Loot => loot;
        public List<EnemyConfig> Enemies => enemies;    
    }
}