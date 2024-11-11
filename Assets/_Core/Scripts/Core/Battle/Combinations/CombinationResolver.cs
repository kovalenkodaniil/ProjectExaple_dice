using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using _Core.Scripts.Core.Data;
using Core.Data;
using Core.Features.Talents.Scripts;
using PlayerScripts;
using VContainer;

namespace _Core.Scripts.Core.Battle.Combinations
{
    public class CombinationResolver
    {
        [Inject] private Player _player;
        [Inject] private BattleUIPresenter _battleUIPresenter;
        [Inject] private BattleVFXEffector _battleVFXEffector;
        [Inject] private GameDataConfig _gameDataConfig;
        [Inject] private CombinationResoverView _combinationResoverView;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private TalentManager talentManager;

        public event Action OnAllCombinationsResolved;
        public event Action OnCombinationResolved;
        
        private List<CombinationPresenter> _combinationPresenters;
        private List<CombinationPresenter> _presentersForResolving;
        private List<CombinationConfig> _combinations;
        private List<EnumEdgeColor> _edgeTypes;

        public CombinationResolver()
        {
            _edgeTypes = new List<EnumEdgeColor>();
            _combinations = new List<CombinationConfig>();
            _combinationPresenters = new List<CombinationPresenter>();
        }

        public void Initialize()
        {
            CreateCombinations();
        }

        public void Reset()
        {
            _combinationResoverView.ClearAll();
            
            _combinationPresenters.Clear();
            _combinations.Clear();
        }

        public void ResolveCombination(List<Dice.Dice> diceList)
        {
            _edgeTypes.Clear();
            
            diceList.ForEach(dice =>
            {
                foreach (var color in dice._data.TopEdgeColor)
                {
                    _edgeTypes.Add(color.type);
                }
            });

            ResolveNextCombination(0);
        }

        private void ResolveNextCombination(int index)
        {
            if (index >= _combinationPresenters.Count)
            {
                OnAllCombinationsResolved?.Invoke();
                return;
            }

            if (_combinationPresenters[index].TryResolveEffect(_edgeTypes, () => ResolveNextCombination(++index)))
            {
                OnCombinationResolved?.Invoke();
            }
        }

        private void CreateCombinations()
        {
            _gameDataConfig.combinationConfigs.ForEach(combination =>
            {
                if (combination.IsUnlocked(talentManager))
                {
                    _combinations.Add(combination);
                }
            });
            
            _combinations.ForEach(combination =>
            {
                _combinationResoverView.CreatePreview(combination);
                
                CombinationPresenter presenter = new CombinationPresenter(combination);
                
                _objectResolver.Inject(presenter);
                
                _combinationPresenters.Add(presenter);
            });
        }
    }
}