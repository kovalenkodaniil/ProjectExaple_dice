using System;
using System.Collections.Generic;
using Core.Data;
using Core.Effects;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public abstract class CombatEntity
    {
        public event Action OnDie;
        public event Action OnHeal;
        public event Action OnTurnEnd;
        public event Action<int> OnShieldChanged;
        public event Action<int> OnTakeDamage;
        public event Action<int, CombatEntity> OnTakeOverDamage;
        public event Action<int> OnDealDamage;

        public int Health { get; protected set; }
        public int Shield { get; protected set; }
        public int DamageBuff { get; protected set; }
        
        public int BaseHealth { get; protected set; }
        public bool IsDied { get; protected set; }
        public bool IsStuned { get; protected set; }
        
        public Queue<EnemyTurn> QueueIntentions { get; private set; } = new(3);

        public List<BuffBase> Buffs { get; protected set; } = new(3);
        
        public virtual void TurnStart()
        {
            Shield = 0;
            IsStuned = false;
        }

        public virtual void TurnEnd()
        {
            foreach (var buff in Buffs)
                buff.OnTurnEnd();
            
            Buffs.RemoveAll(buff => buff.IsExpired());
            
            OnTurnEnd?.Invoke();
        }
        
        
        public void Stun()
        {
            IsStuned = true;
        }

        public int CalculateDamage(int currentDamage)
        {
            return currentDamage + DamageBuff;
        }
        
        public virtual void Heal(int amount)
        {
            Health = Mathf.Min(BaseHealth, amount + Health);
            OnHeal?.Invoke();
        }
        
        public virtual void IncreaseBaseHealth(int amount)
        {
            BaseHealth += amount;
            Heal(amount);
        }

        public virtual void DealDamage(int damage)
        {
            OnDealDamage?.Invoke(CalculateDamage(damage));
        }
        
        public virtual void TakeDamage(int damage)
        {
            int damageToApply = Mathf.Max(damage - Shield, 0);
            int overDamage = damageToApply - Health;
            Shield = Mathf.Max(Shield - damage, 0);
            Health -= damageToApply;

            OnTakeDamage?.Invoke(damage);
            OnShieldChanged?.Invoke(Shield);
            
            if (Health <= 0)
            {
                IsDied = true;
                OnDie?.Invoke();
            }
            
            if (overDamage > 0)
                OnTakeOverDamage?.Invoke(overDamage, this);
        }

        public virtual void AddShield(int amount)
        {
            Shield += amount;
            
            OnShieldChanged?.Invoke(Shield);
        }

        public void ApplyDamageBuff(int power)
        {
            DamageBuff += power;
        }

        public void AddEffect(global::Core.Effects.Effect effect, int power, int duration = 0)
        {
            if (effect is InstantEffectBase instEffect)
            {
                instEffect.Apply(this, power);
            }
            else if (effect is BuffBase buff)
            {
                buff.Apply(this, power, duration, duration < 0);
                Buffs.Add(buff);   
            }
        }
    }
}