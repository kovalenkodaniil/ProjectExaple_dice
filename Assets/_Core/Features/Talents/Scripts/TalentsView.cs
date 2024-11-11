using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Talents.Data;
using PlayerScripts;
using Popups;
using TMPro;
using UnityEngine;
using VContainer;

namespace Core.Features.Talents.Scripts
{
    public class TalentsView : MonoBehaviour
    {
        [Inject] private MenuPopup menu;
        [Inject] private Player player;
        [Inject] private TalentManager talentManager;
        
        private float timerToRemovePresenters;
        private List<TalentPresenter> presenters = new(10);
        private Coroutine waitRemovePresentersRoutine;
        private TalentsViewExpObserver expObserver;
        
        [SerializeField] private GameObject container;
        [SerializeField] private List<Talent> talents;
        [SerializeField] private TMP_Text lbExp;
        [SerializeField] private TMP_Text lbHeader;

        public List<Talent> Talents => talents;

        private void Awake()
        {
            container.SetActive(false);
            expObserver = new TalentsViewExpObserver(lbExp, player);

            talentManager.OnResetData += HandlerResetData;
        }

        public void Open()
        {
            container.SetActive(true);
            expObserver.Enable();
            
            if (timerToRemovePresenters == 0)
            {
                foreach (var talent in talents)
                {
                    var pres = new TalentPresenter(talent, talentManager, player);
                    presenters.Add(pres);
                }
            }

            lbHeader.text = Managers.Localization.Translate(LocalizationKeys.talents_header);
        }

        public void Close()
        {
            timerToRemovePresenters = 60;
            if (waitRemovePresentersRoutine == null)
                StartCoroutine(RemovePresentersRoutine());
            
            expObserver.Disable();
            container.SetActive(false);
        }

        public void ClickClose()
        {
            Close();
        }

        private void HandlerResetData()
        {
            timerToRemovePresenters = 0;

            if (waitRemovePresentersRoutine != null)
            {
                StopCoroutine(RemovePresentersRoutine());
                waitRemovePresentersRoutine = null;
            }
            
            foreach (var presenter in presenters)
                presenter.Destroy();
            
            presenters.Clear();
        }

        public TalentData GetTalent(string id)
        {
            return talents.FirstOrDefault(x => x.Data.Id == id)?.Data;
        }

        private IEnumerator RemovePresentersRoutine()
        {
            while (timerToRemovePresenters > 0)
            {
                yield return new WaitForSeconds(1);
                timerToRemovePresenters--;
            }

            waitRemovePresentersRoutine = null;
            
            foreach (var presenter in presenters)
                presenter.Destroy();
            
            presenters.Clear();
        }
    }
}