using System;
using System.Collections.Generic;
using Core.Data;
using DG.Tweening;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.Dice
{
    public class DiceUIPreview : MonoBehaviour
    {
        [SerializeField] private Transform _diceFrameTransform;
        [SerializeField] private DiceSettings _diceSettings;
        [SerializeField] private List<MeshRenderer> _diceMeshes;

        private DiceConfig _config;
        private DiceEdgeSetter[] _edges;

        public void InitializeInventory(DiceConfig config, float startScale)
        {
            _config = config;
            
            _diceMeshes.ForEach(mesh => mesh.material = _config.DiceBorderMaterial);
            
            transform.DOScale(startScale, 0.4f);
            
            SetupEdges();
        }

        public void PlayHideAnimation(Action callback = null)
        {
            transform.DOScale(0, 0.4f).OnComplete(() => callback?.Invoke());
        }

        private void SetupEdges()
        {
            if (_edges != null)
            {
                foreach (var edge in _edges)
                {
                    Destroy(edge.gameObject);
                }
            }

            _edges = new DiceEdgeSetter[_config.Edges.Count];

            for (int i = 0; i < _config.Edges.Count; ++i)
            {
                _edges[i] = Instantiate(_config.Edges[i].edgePrefab, _diceFrameTransform);
                Vector3 rotation = Vector3.zero;
                switch (i)
                {
                    case 0: rotation = _diceSettings.diceEdgeRotationUp; break;
                    case 1: rotation = _diceSettings.diceEdgeRotationRight; break;
                    case 2: rotation = _diceSettings.diceEdgeRotationForward; break;
                    case 3: rotation = _diceSettings.diceEdgeRotationDown; break;
                    case 4: rotation = _diceSettings.diceEdgeRotationLeft; break;
                    case 5: rotation = _diceSettings.diceEdgeRotationBack; break;
                    default: rotation = Vector3.zero; break;
                }

                _edges[i].transform.localRotation = Quaternion.Euler(rotation);
                _edges[i].SetEdgeMaterial(_config.Edges[i].edgeMaterial);
                
                if (_config.Edges[i].edgePattern.edgeIcon != null)
                    _edges[i].SetIcon(_config.Edges[i].edgePattern.edgeIcon);
                
                if (_config.Edges[i].edgePattern.colors.Length > 0)
                    _edges[i].SetColor(_config.Edges[i].edgePattern.colors[0].color);
            }
        }
    }
}