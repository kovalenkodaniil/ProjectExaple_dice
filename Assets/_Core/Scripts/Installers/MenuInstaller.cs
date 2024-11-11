using Core.Data;
using Core.Saves;
using Managers;
using PlayerScripts;
using Popups;
using UnityEngine;

namespace Installers
{
    public class MenuInstaller
    {
        [SerializeField] private SceneLoader _sceneLoader;
        
        public void Bind()
            {
                /*binder.Bind(Camera.main);
                binder.Bind(_sceneLoader); 

                binder.Bind(FindObjectOfType<GameManager>());
                binder.Bind(FindObjectOfType<DataManager>());
                binder.Bind(FindObjectOfType<MenuPopup>());

                binder.Bind(new Player());
                binder.Bind(new GameSaver());
                binder.Bind(new DataGameKeeper());*/
            }
    }
}