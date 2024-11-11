namespace _Core.Scripts.Core.Battle.SkillScripts
{
    public class SkillData
    {
        private readonly SkillConfig _config;

        public int CooldownMax { get; private set; }
        public bool IsUsed { get; private set; }
        public int TurnForActive { get; private set; }

        public SkillData(SkillConfig config)
        {
            _config = config;
            CooldownMax = config.cooldownMax;
            IsUsed = false;
        }

        public void Use()
        {
            TurnForActive = CooldownMax;
            IsUsed = true;
        }

        public void Tick()
        {
            TurnForActive--;

            IsUsed = TurnForActive > 0;
        }
    }
}