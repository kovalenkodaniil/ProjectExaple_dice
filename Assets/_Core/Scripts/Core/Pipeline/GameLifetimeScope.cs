using System;
using _Core.Scripts.Core.Data;
using _Core.Scripts.Core.Greyness;
using Core.Data;
using Core.Effects;
using Core.Features.Talents.Scripts;
using Core.Localization;
using Managers;
using PlayerScripts;
using Popups;
using Popups.PopupReward;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("StaticData")]
    [SerializeField] private GameDataConfig _gameStaticData;
    [SerializeField] private FontSettings _fontSetting;
    [SerializeField] private VFXSetting _vfxSetting;
    [SerializeField] private SoundConfig _soundSetting;
    
    [Header("Managers")]
    [SerializeField] private PopupManager _popupManager;
    [SerializeField] private SoundManager _soundManager;
    
    [Header("Popups")]
    [SerializeField] private PopupReward popupReward;
    [SerializeField] private PopupEffector _popupEffector;
    [SerializeField] private LoadingPopup _loadingPopup;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<Player>(Lifetime.Singleton);
        builder.Register<SceneLoader>(Lifetime.Singleton);
        builder.Register<Managers.Localization>(Lifetime.Singleton);
        builder.Register<GameStats>(Lifetime.Singleton);
        builder.Register<EffectManager>(Lifetime.Singleton);
        builder.Register<GreynessManager>(Lifetime.Singleton);

        builder.RegisterComponent(_popupManager);
        builder.RegisterComponent(_soundManager);
        
        builder.RegisterComponent(popupReward);
        builder.RegisterComponent(_loadingPopup);
        builder.RegisterComponent(_popupEffector);
        
        builder.RegisterInstance(_gameStaticData);
        builder.RegisterInstance(_fontSetting);
        builder.RegisterInstance(_vfxSetting);
        builder.RegisterInstance(_soundSetting);
        
        builder.RegisterEntryPoint<EntryPoint>();
        
        builder.Register(container =>
        {
            Lazy<Player> lazyPlayer = new Lazy<Player>(() => container.Resolve<Player>());
            return new TalentManager(lazyPlayer);
        }, Lifetime.Singleton);
    }
}
