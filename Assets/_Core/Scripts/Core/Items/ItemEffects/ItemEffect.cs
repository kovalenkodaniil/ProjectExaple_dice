using _Core.Scripts.Core.Battle.Combinations;
using PlayerScripts;
using VContainer;

namespace Core.Items.ItemEffects
{
    public class ItemEffect
    {
        [Inject] protected CombinationResolver _combinationResolver;
        [Inject] protected Player _player;
        
        protected int value;
        protected ItemType type;
        protected ItemConfig _config;

        public virtual void Initialize(ItemConfig config)
        {
            _config = config;
            value = config.value;
            type = config.type;
        }

        public virtual void Apply()
        {
        }

        public virtual void Reset()
        {
        }
    }
}