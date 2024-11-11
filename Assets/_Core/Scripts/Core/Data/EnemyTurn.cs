using System;
using Managers;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class EnemyTurn
    {
        public EnemyAction action;
        public int power;
        [HideInInspector] public string description;
        [HideInInspector] public bool IsStuned;
        
        private int _duration;

        public int Duration => _duration;
        

        public void UpdateDescription(EnemyData enemyData, Managers.Localization localization)
        {
            if (action.effects.Contains(EnumEffects.Empty)) return;
            
            _duration = action.duration;

            description = "";
            
            string[] tempArray = localization.GetTranslate(action.description).Split(' ');
            
            for (int i = 0; i < tempArray.Length; i++)
            {
                description += tempArray[i];
                description += ' ';
            }
        }
    }
}