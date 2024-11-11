using System;
using UnityEngine;

namespace _Core.Scripts.Core.Greyness
{
    public class GreynessData
    {
        private const int MAX_STAGE = 5;
        
        public event Action<int> OnValueChanged;

        private int _stage;
        
        public int Stage
        {
            get => _stage;
            set
            {
                _stage = Mathf.Max(value, 0);
                _stage = Mathf.Min(_stage, MAX_STAGE);

                OnValueChanged?.Invoke(_stage);
            }
        }

        public GreynessData()
        {
            _stage = 1;
        }
    }
}