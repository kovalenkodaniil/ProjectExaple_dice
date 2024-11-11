using System;

namespace _Core.Scripts.Core.Battle.Stat
{
    public class ArmorComponent : IStatsComponent
    {
        private int _armor;
        private int _armorBuf;
        private int _magicArmor;
        public int MaxValue { get; set; }
        public Action<int> OnValueChanged { get; set; }
        
        public int CurrentValue => _armor;
        
        public int ArmorBuf => _armorBuf;

        public ArmorComponent(int startArmor = 0)
        {
            _armor = startArmor;
        }

        public void SetArmorBuff(int value)
        {
            if (value <= 0) return;
            
            _armorBuf = value;
        }
        
        public void ResetArmorBuf() => _armorBuf = 0;
        
        public void AddMagicArmor(int value)
        {
            if (value <= 0) return;
            
            _magicArmor = value;
        }

        public void AddArmor(int value)
        {
            if (value <= 0) return;

            _armor += (value + _armorBuf);
            OnValueChanged?.Invoke(_armor);
        }

        public void ResetArmor()
        {
            _armor = 0;
            _magicArmor = 0;
            OnValueChanged?.Invoke(_armor);
        }

        public int GetDamageAfterBlock(int value, int magicDamage = 0)
        {
            if (magicDamage < _magicArmor) return 0;
            
            int startValue = value;
            value -= _armor;

            if (value <= 0)
            {
                _armor -= startValue;
                OnValueChanged?.Invoke(_armor);
                
                return 0;
            }
            
            _armor = 0;
            OnValueChanged?.Invoke(_armor);
            
            return value;
        }
    }
}