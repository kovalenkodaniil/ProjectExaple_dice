using System.Collections.Generic;
using Core.Data;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [Inject] private SoundConfig _soundConfig;

        public static SoundManager Instance;
        
        private struct AudioData
        {
            public readonly float volume;
            public AudioData(float volume) => this.volume = volume;
        } 
        
        private Tween musicTween;
        private Coroutine muteMusicCoroutine;
        private AudioSourceFactory audioSourceFactory;
        private readonly Dictionary<AudioClip, AudioData> audioDatas = new();

        private AudioSource musicChannel;

        [SerializeField] public float musicVolume = 0.3f;
        [SerializeField] public float soundVolume = 0.3f;

        public SoundConfig SoundList => _soundConfig;

        public void Initialize()
        {
            Instance = this;

            audioSourceFactory = new AudioSourceFactory(transform);

            musicChannel = audioSourceFactory.Get(musicVolume);
            
            SetMusicVolume(musicVolume);
            SetSoundVolume(soundVolume);

            InitAudioDatas();
        }
        
        private void InitAudioDatas()
        {   
            audioDatas.Add(SoundList.RewardSound, new(.35f));
            audioDatas.Add(SoundList.winTheme,    new(.45f));
            audioDatas.Add(SoundList.ButtonClick, new(.35f));
        }

        private void SetMusicVolume(float value) => musicChannel.volume = musicVolume = value;

        private void SetSoundVolume(float value) => soundVolume = value;

        public void PlayTheme(AudioClip _clip)
        {
            musicChannel.clip = _clip;
            musicChannel.loop = true;
            musicChannel.Play();
            
            float correctVolume = 1f;
            // если не сделать проверку на null, то в билде на webgl возникает исключение null аргумента переданного в TryGetValue
            if (_clip != null && audioDatas.TryGetValue(_clip, out AudioData data))
                correctVolume = data.volume;
                
            musicChannel.volume = musicVolume * correctVolume;
        }

        public void PauseTheme()
        {
            if (musicTween is { active: true })
                musicTween.Kill();
            
            if(muteMusicCoroutine != null)
                StopCoroutine(muteMusicCoroutine);
            
            musicChannel.Pause();
        }

        public void PlayEffect(AudioClip _clip)
        {
            var source = audioSourceFactory.Get(soundVolume);

            if (source == null)
                return;

            source.Stop();
            source.clip = _clip;
            source.loop = false;
                
            float correctVolume = 1f;
            // если не сделать проверку на null, то в билде на webgl возникает исключение null аргумента переданного в TryGetValue
            if (_clip != null && audioDatas.TryGetValue(_clip, out AudioData data))
                correctVolume = data.volume;
                
            source.volume = soundVolume * correctVolume;
                
            source.Play();
        }
    }
}