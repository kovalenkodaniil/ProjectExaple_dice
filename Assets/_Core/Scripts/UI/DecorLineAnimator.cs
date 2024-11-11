using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace UI
{
    public class DecorLineAnimator : MonoBehaviour
    {
        private float ANIM_DURATION = 5f;
        
        [SerializeField] private Transform _repeatPoint;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private List<Transform> _lines;

        private float _animationDistance;
        private float _currentDuration;
        private List<float> _startPositions = new List<float>(3);
        private List<TweenerCore<Vector3, Vector3, VectorOptions>> _tweens = new List<TweenerCore<Vector3, Vector3, VectorOptions>>(6);

        public void PlayAnimation()
        {
            for (int i = 0; i < _lines.Count; i++)
            {
                Transform line = _lines[i];
                _startPositions.Add(line.position.x);

                TweenerCore<Vector3, Vector3, VectorOptions> tween = line
                    .DOMoveX(_repeatPoint.position.x, ANIM_DURATION * (i + 1)).SetEase(Ease.Linear)
                    .SetLink(gameObject)
                    .OnComplete(() =>
                    {
                        line.position = _startPoint.position;
                        _tweens.Add(line.DOMoveX(_repeatPoint.position.x, ANIM_DURATION * _lines.Count)
                            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetLink(gameObject));
                    });
                
                _tweens.Add(tween);
            }
        }

        public void Reset()
        {
            _tweens.ForEach(tween => tween.Kill());
            _tweens.Clear();
            
            print("Reset");
            
            for (int i = 0; i < _lines.Count; i++)
            {
                Transform line = _lines[i];

                line.position = new Vector3(_startPositions[i], line.position.y, line.position.z);
            }
            
            _startPositions.Clear();
        }
    }
}