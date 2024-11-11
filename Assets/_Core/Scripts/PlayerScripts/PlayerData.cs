using System;
using System.Collections.Generic;
using Core.Data.Consumable;

namespace PlayerScripts
{
    [Serializable]
    public class PlayerData
    {
        public int healthMax;
        public int manaMax;
        public int fixation;
        public int coin;
        public int karma;
        public string name;

        public List<DiceSaveData> availableDice;
        public List<string> availableSpells;
        public CurrencyStorage currencyStorage;
        public ConsumableStorage consumableStorage;
        public List<ConsumableData> consumablesInBattle;
        public PlayerModel model;

        public PlayerData()
        {
            availableDice = new List<DiceSaveData>();
            availableSpells = new List<string>();
        }
    }

    [Serializable]
    public class DiceSaveData
    {
        public string id;
        public int quantity;
    }
}