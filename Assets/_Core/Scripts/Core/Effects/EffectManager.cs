using System;
using System.Collections.Generic;
using Core.Data;

namespace Core.Effects
{
    public class EffectManager
    {
        private Dictionary<EnumEffects, Stack<Effect>> effectPool;

        public EffectManager()
        {
            var effects = Enum.GetValues(typeof(EnumEffects));
            
            effectPool = new(effects.Length);
            
            foreach (EnumEffects effect in effects)
                effectPool[effect] = new(3);
        }

        public Effect GetEffect(EnumEffects type)
        {
            return effectPool[type].Count > 0 ? effectPool[type].Pop() : CreateEffect(type);
        }

        public void ReturnEffect(Effect effect, EnumEffects type)
        {
            effectPool[type].Push(effect);
        }

        private Effect CreateEffect(EnumEffects type)
        {
            return type switch
            {
                EnumEffects.Attack     => new AttackInstantEffect(),
                EnumEffects.Heal       => new HealInstantEffect(),
                EnumEffects.Armor      => new ShieldInstantEffect(),
                EnumEffects.DamageBuff => new DamageBuff(),
                EnumEffects.Mana       => null,
                EnumEffects.Empty      => null,
                EnumEffects.Skill      => null,
                EnumEffects.Fixation   => null,
                EnumEffects.ArmorBuff  => null,
                EnumEffects.Spike      => null,
                EnumEffects.MagicArmor => null,
                EnumEffects.Stun       => new StunInstantEffect(),
                EnumEffects.MagicArrow => null,
                EnumEffects.Mastery    => null,
                EnumEffects.Connection => null,
                EnumEffects.Trap       => null,
                EnumEffects.StunIntention => new StunIntentionInstantEffect(),
                EnumEffects.NoLethalDamage  => null,
                EnumEffects.Invulnerability => null,
                EnumEffects.HealthBuff => new HealthBuffEffect(),
                EnumEffects.Regeneration => null,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}