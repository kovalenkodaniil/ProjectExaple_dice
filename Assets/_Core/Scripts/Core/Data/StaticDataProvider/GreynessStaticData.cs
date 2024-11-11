using System.Collections.Generic;

namespace Core.Data
{
    public class GreynessStaticData: IStaticDataProvider
    {
        public List<Effect> effects;

        public GreynessStaticData(List<Effect> effects)
        {
            this.effects = effects;
        }
    }
}