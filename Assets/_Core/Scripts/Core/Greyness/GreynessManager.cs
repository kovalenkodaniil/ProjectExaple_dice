using System.Collections.Generic;
using System.Linq;
using Core.Data;

namespace _Core.Scripts.Core.Greyness
{
    public class GreynessManager
    {
        private Dictionary<int, Effect> stageEffects;
        public GreynessData data;

        public int CurrentStage => data.Stage;
        public int MaxStage => stageEffects.Count-1;

        public GreynessManager()
        {
            data = new GreynessData();
        }

        public void Initialize()
        {
            GreynessStaticData provider = StaticDataProvider.Get<GreynessStaticData>();
            
            stageEffects = new Dictionary<int, Effect>()
            {
                {0, null},
                {1, provider.effects[0]},
                {2, provider.effects[1]},
                {3, provider.effects[2]},
                {4, provider.effects[3]},
                {5, provider.effects[4]},
            };
        }
        
        public List<Effect> GetAllEffect()
        {
            return stageEffects.Values.ToList();
        }

        public List<Effect> GetCurrentEffects()
        {
            List<Effect> currentEffect = new List<Effect>();
            
            foreach (var stage in stageEffects.Keys)
            {
                if (stage <= CurrentStage) currentEffect.Add(stageEffects[stage]);
            }

            return currentEffect;
        }
    }
}