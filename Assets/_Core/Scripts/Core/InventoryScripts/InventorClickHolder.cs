using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class InventorClickHolder : MonoBehaviour
    {
        public event Action OnClicked;
        private List<RectTransform> _rectTransforms;

        public void AddDicePreview(DiceInBattlePreview diceInBattlePreview)
        {
            if (_rectTransforms == null)
                _rectTransforms = new List<RectTransform>();
            
            _rectTransforms.Add(diceInBattlePreview.GetComponent<RectTransform>());
        }
        
        public void AddSpellPreview(InBattlePreview spellPreview)
        {
            if (_rectTransforms == null)
                _rectTransforms = new List<RectTransform>();
            
            _rectTransforms.Add(spellPreview.GetComponent<RectTransform>());
        }

        public void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_rectTransforms == null) return;
                
                var worldMousePosition = GlobalCamera.Camera.ScreenToWorldPoint(Input.mousePosition);

                foreach (var rect in _rectTransforms)
                {
                    var localMousePosition = rect.InverseTransformPoint(worldMousePosition);

                    if (rect.rect.Contains(localMousePosition))
                    {
                        return;
                    }
                }
                
                OnClicked?.Invoke();
            }
        }
    }
}