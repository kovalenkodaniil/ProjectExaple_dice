using Managers;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.Dice
{
    public class DiceChecker
    {
        private Ray _rayForChecking;
        private RaycastHit[] _raycastHits = new RaycastHit[10];
        
        public bool IsDiceInPosition(Vector3 position, out Dice searchingDice)
        {
            searchingDice = null;
            
            Ray tempRay = GlobalCamera.Camera.ScreenPointToRay(Input.mousePosition);

            Physics.RaycastNonAlloc(tempRay, _raycastHits);

            foreach (var hit in _raycastHits)
            {
                if (hit.collider == null) continue;
                
                if (hit.collider.TryGetComponent(out Dice dice))
                {
                    searchingDice = dice;
                    return true;
                }
            }
            
            return false;
        }
    }
}