using TMPro;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.SkillScripts
{
    public class SkillInfoHelper : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpInfo;

        public string InfoText { set => tmpInfo.text = value; }
    }
}