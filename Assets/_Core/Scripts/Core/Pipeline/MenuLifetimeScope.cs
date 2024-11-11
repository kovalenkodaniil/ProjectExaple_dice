using _Core.Scripts.Core.Greyness;
using Core.Features.Talents.Scripts;
using Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utils.Pipeline
{
    public class MenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private MenuPopup _menuPopup;
        [SerializeField] private GreynessPresenter _greynessPresenter;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_menuPopup);
            builder.RegisterComponent(_greynessPresenter);
            builder.RegisterComponentInHierarchy<TalentsView>();
        }
    }
}