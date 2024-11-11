using System.Linq;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public abstract class EnemyCombatEntity : CombatEntity
    {
        public void StunIntention(int index)
        {
            var intention = QueueIntentions.ElementAt(index);

            if (intention != null)
                intention.IsStuned = true;
        }
    }
}