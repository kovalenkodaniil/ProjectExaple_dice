using System;

namespace _Core.Scripts.Core.Battle.Stat
{
    public class DamageComponent: IStatsComponent
    {
        private int _damageBuf;
        private int damage;

        public int Damage
        {
            get { return CalculateDamage(); }
            set => damage = value;
        }

        public int DamageBuf => _damageBuf;

        public int MaxValue { get; set; }
        public Action<int> OnValueChanged { get; set; }
        
        public DamageComponent(int startDamage = 0)
        {
            damage = startDamage;
        }
        
        public int CurrentValue => damage;

        public void SetDamageBuff(int value)
        {
            if (value <= 0) return;
            
            _damageBuf = value;
        }
        
        public void AddDamage(int value)
        {
            if (value <=0) return;

            damage += (value + _damageBuf);
            OnValueChanged?.Invoke(damage);
        }
        
        public void Reset()
        {
            damage = 0;
            _damageBuf = 0;
            OnValueChanged?.Invoke(damage);
        }

        private int CalculateDamage()
        {
            int dmg = damage;

#if UNITY_EDITOR
            if(DebugSettingsEditor.EnablePlayerDamage)
                dmg = DebugSettingsEditor.PlayerDamage;
#endif
            
            return dmg;
        }
    }
}