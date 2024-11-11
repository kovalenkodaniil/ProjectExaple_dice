using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Setting/Create new Sound Settings")]
    public class SoundConfig : ScriptableObject
    {
        [Header("Music")]
        public AudioClip mainTheme;
        public AudioClip battleTheme;
        public AudioClip winTheme;
        public AudioClip LoseTheme;
        
        [Header("Sound")]
        public AudioClip ButtonClick;
        public AudioClip RewardSound;
        public AudioClip BuySound;
        public AudioClip DiceRollSound;
        public AudioClip DiceLock;
        public AudioClip HitSound;
        
        [Header("Spell Sound")]
        public AudioClip FireBallSound;
        public AudioClip BarrierSound;
        public AudioClip FurySound;
    }
}