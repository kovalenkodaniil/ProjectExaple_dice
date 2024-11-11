using System;
using System.Collections;
using System.Collections.Generic;
using Core.Data;
using DG.Tweening;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace _Core.Scripts.Core.Battle.Dice
{
    public class Dice : MonoBehaviour
    {
        [Inject] private BattleUIPresenter _battleUIPresenter;
        
        public Action OnLand;
        public Action OnReturn;

        [SerializeField] public DiceHolder diceHolder;
        [SerializeField] private Transform _diceFrameTransform;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DiceSettings _diceSettings;
        [SerializeField] private List<MeshRenderer> _diceMeshes;
        [SerializeField] private Material _blockBorderDiceMaterial;
        
        public BattleDiceData _data;
        public bool isBlocking;

        private DiceEdgeSetter[] _edges;
        private MeshRenderer[] _edgeMeshRenderers;
        private Material[] _edgeMaterials;
        
        private bool _isFirstThrow = true;
        private bool _isEmptyRoll;
        private bool _isStopRolling;
        private float _holdPositionZ;
        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private Action _afterRolling;
        
        public bool IsDropped { get; set; }
        
        public void Initialize(BattleDiceData battleDiceData, Vector3 localStartPosition)
        {
            _data = battleDiceData;
            SetupEdges();

            transform.localPosition = localStartPosition;
            _isEmptyRoll = true;

            _diceMeshes.ForEach(mesh => { mesh.material = battleDiceData.Config.DiceBorderMaterial;});
            
            SetupDiceHolder();

            IsDropped = true;
            
            _holdPositionZ = -0.415f;
        }

        public void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Reset()
        {
            _data.SetColor(_data.CurrentEdge.edgePattern.colors);

            var indexOf = _data.Edges.IndexOf(_data.CurrentEdge);
            
            if (_data.CurrentEdge.edgePattern.colors.Length > 1)
                _edges[indexOf].SetColors(_data.CurrentEdge.edgePattern.colors[0].color, _data.CurrentEdge.edgePattern.colors[1].color);
            else
                _edges[indexOf].SetColor(_data.CurrentEdge.edgePattern.colors[0].color);
        }

        public void PlayReturnAnimation(float delay = 0f)
        {
            Sequence animationOrder = DOTween.Sequence();
            
            _rigidbody.isKinematic = true;
            int index = _data.Edges.IndexOf(_data.CurrentEdge);

            Vector3 rotate = new Vector3();
            
            switch (index)
            {
                case 0:
                    rotate = new Vector3(0f, -90f, 90f);
                    break;
                
                case 1:
                    rotate = new Vector3(0f,-90f,180f);
                    break;
                
                case 2:
                    rotate = new Vector3(0f,-180f,180f);
                    break;
                
                case 3:
                    rotate = new Vector3(90f,0f,0f);
                    break;
                
                case 4:
                    rotate = new Vector3(90f,0f,90f);
                    break;
                
                case 5:
                    rotate = new Vector3(0f,0f,90f);
                    break;
            }

            _startPosition.z = _holdPositionZ;

            animationOrder.Append(transform.DOLocalMove(_startPosition, 0.6f).SetDelay(delay)).OnComplete(ReturnDice);
            animationOrder.Join(transform.DOLocalRotate(rotate, 0.4f));
        }

        public virtual void Roll(int rollNumber, int diceNumber, Action afterRolling = null)
        {
            if (diceHolder.IsBlocking) return;
            
            if (diceHolder.IsHeld) return;

            IsDropped = false;
            
            diceHolder.ResetFlyAnimation();
            diceHolder.CanBeHold = false;

            _data.GetRandomEdge();

            StopAllCoroutines();
            StartCoroutine(YouSeeMeRolling(rollNumber, diceNumber));

            _afterRolling = afterRolling;
        }

        public void ChangeColor(EdgeColor color)
        {
            _data.SetColor(color);

            var indexOf = _data.Edges.IndexOf(_data.CurrentEdge);
            
            _edges[indexOf].SetColor(color.color);
        }

        public void OnMouseEnter()
        {
            _battleUIPresenter.ShowDiceTooltip(_data.CurrentEdge,
                _edges[_data.Edges.IndexOf(_data.CurrentEdge)].transform.position);
        }

        public void OnMouseExit() => _battleUIPresenter.HideDiceTooltip();

        public void BlockDice(bool isBlocking)
        {
            diceHolder.BlockDice(isBlocking);

            this.isBlocking = isBlocking;
        }

        public void SetBlockingView(bool isBlocking)
        {
            if (isBlocking)
            {
                _diceMeshes.ForEach(mesh => { mesh.material = _blockBorderDiceMaterial;});
            }
            else
            {
                _diceMeshes.ForEach(mesh => { mesh.material = _data.Config.DiceBorderMaterial;});
            }
        }
        
        private void SetupDiceHolder()
        {
            diceHolder.Initialize(_diceSettings, transform);
        }

        private void DropDice()
        {
            IsDropped = true;
            OnLand?.Invoke();
            _afterRolling?.Invoke();
        }
        
        private void ReturnDice()
        {
            if (_isEmptyRoll == false)
                _isEmptyRoll = true;
            else
                OnReturn?.Invoke();
        }

        private void SetupEdges()
        {
            _edges = new DiceEdgeSetter[_data.Edges.Count];
            _edgeMeshRenderers = new MeshRenderer[_data.Edges.Count];
            _edgeMaterials = new Material[_data.Edges.Count];
            
            for (int i = 0; i < _data.Edges.Count; ++i)
            {
                _edges[i] = Instantiate(_data.Edges[i].edgePrefab, _diceFrameTransform);
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
                _edges[i].SetEdgeMaterial(_data.Edges[i].edgeMaterial);
                
                if (_data.Edges[i].edgePattern.edgeIcon != null)
                    _edges[i].SetIcon(_data.Edges[i].edgePattern.edgeIcon);

                if (_data.Edges[i].edgePattern.colors.Length > 0)
                {
                    if (_data.Edges[i].edgePattern.colors.Length > 1)
                        _edges[i].SetColors(_data.Edges[i].edgePattern.colors[0].color, _data.Edges[i].edgePattern.colors[1].color);
                    else
                        _edges[i].SetColor(_data.Edges[i].edgePattern.colors[0].color);
                }

                
                
                _edgeMeshRenderers[i] = _edges[i].GetComponentInChildren<MeshRenderer>();
                _edgeMaterials[i] = _edgeMeshRenderers[i].material;
            }
        }

        public void PlayCastAnimation()
        {
            _rigidbody.transform.DOLocalMoveZ(-_diceSettings.dicejumpDisatance, _diceSettings.diceHoldTime)
                .SetRelative();
            _rigidbody.transform.DOLocalMoveZ(_diceSettings.dicejumpDisatance, _diceSettings.diceHoldTime)
                .SetRelative()
                .SetDelay(_diceSettings.diceHoldTime);
        }

        private IEnumerator YouSeeMeRolling(int rollNumber, int diceNumber)
        {
            _rigidbody.mass = 1;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.zero;

            _isStopRolling = false;
            _startPosition = transform.localPosition;
            
            yield return new WaitForFixedUpdate();

            float ZForseVector = 0;

            float XForseVector = 0;

            if (diceNumber < 3)
            {
                ZForseVector = Random.Range(_diceSettings.ZStartVector + _diceSettings.ZSpreding/2, _diceSettings.ZStartVector - _diceSettings.ZSpreding/2);
                XForseVector = 0 - _diceSettings.XSpreding * (3 - diceNumber);
            }
            else if (diceNumber > 4)
            {
                ZForseVector = Random.Range(_diceSettings.ZStartVector + _diceSettings.ZSpreding/2, _diceSettings.ZStartVector - _diceSettings.ZSpreding/2);
                XForseVector = 0 + _diceSettings.XSpreding * (diceNumber - 4);
            }
            else
            {
                ZForseVector = Random.Range(_diceSettings.ZStartVector + _diceSettings.ZSpreding, _diceSettings.ZStartVector - _diceSettings.ZSpreding);
            }

            _rigidbody.AddForce(new Vector3(XForseVector,_diceSettings.YStartVector,ZForseVector) * Random.Range(_diceSettings.throwMinImpulse, _diceSettings.throwMaxImpulse), ForceMode.Impulse);
                
            yield return new WaitForSeconds(_diceSettings.spinDelay);

            _rigidbody.AddRelativeTorque(Random.onUnitSphere * _diceSettings.spinMuliplier, ForceMode.Impulse);
            _isFirstThrow = false;

            yield return new WaitForSeconds(1f);

            StartCoroutine(WaitStopRolling());
            
            yield return new WaitForSeconds(4f);

            if (!_isStopRolling)
            {
                SetTopEdge();
                DropDice();
            }
        }

        private void SetTopEdge()
        {
            _data.SetEdgeIndex(FindTopEdgeIndex());
            _data.SetColor(_data.CurrentEdge.edgePattern.colors);
        }

        private int FindTopEdgeIndex()
        {
            int topEdgeIndex = 0;
            
            for (int i = 0; i < _edges.Length; i++)
            {
                if (_edges[topEdgeIndex]._meshRenderer.transform.position.z > _edges[i]._meshRenderer.transform.position.z)
                    topEdgeIndex = i;
            }

            return topEdgeIndex;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag is "Wall" or "Dice")
            {
                Vector3 topEdgePosition = _edges[FindTopEdgeIndex()]._meshRenderer.transform.position;
                Vector3 dicePosition = transform.position;
                Vector3 forceVector = topEdgePosition - dicePosition + topEdgePosition;
                
                _rigidbody.AddForce(forceVector, ForceMode.Impulse);
            }
        }

        private IEnumerator WaitStopRolling()
        {
            yield return new WaitUntil(() =>_rigidbody.velocity == Vector3.zero);

            _isStopRolling = true;
            
            yield return new WaitForSeconds(0.2f);
            
            SetTopEdge();
            DropDice();
        }
    }
}