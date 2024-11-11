using System.Collections.Generic;
using Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _Core.Scripts.Core.Battle.Combinations
{
    public class CombinationView : MonoBehaviour
    {
        [SerializeField] private List<Image> _egdeIcons;

        public void SetCombination(CombinationConfig combinationConfig)
        {
            for (int i = 0; i < _egdeIcons.Count; i++)
            {
                _egdeIcons[i].color = combinationConfig.comboSequence[i].color;
            }
        }
    }
}