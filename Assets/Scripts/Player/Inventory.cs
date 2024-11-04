using KBCore.Refs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] private GameManager gameManager;
    [SerializeField] private OreValues oreValues;

    [field: Header("Settings")]
    [field: SerializeField] public int Money { get; private set; }
    public Dictionary<OreType, int> OreInventory { get; private set; }

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        oreValues.ConvertSerializedValuesToDict();

        gameManager.OnQuotaEnd += GameManager_OnQuotaEnd;
    }

    private void OnDestroy()
    {
        gameManager.OnQuotaEnd -= GameManager_OnQuotaEnd;
    }

    private void GameManager_OnQuotaEnd()
    {
        if (!CanAfford(gameManager.CurrentQuotaRequirement))
        {
            RemoveMoney(gameManager.CurrentQuotaRequirement);
            gameManager.ChangeState(GameManager.GameState.LOSE);

            AudioManager.Instance.Play(AudioManager.Instance.WrongSFX, 0.2f);
            return;
        }

        RemoveMoney(gameManager.CurrentQuotaRequirement);
        gameManager.StartNextQuota();
    }

    private void Start()
    {
        InitializeOreInventory();
    }

    private void InitializeOreInventory()
    {
        OreInventory = new Dictionary<OreType, int>
            {
                { OreType.COPPER, 0 },
                { OreType.IRON, 0 },
                { OreType.SILVER, 0 },
                { OreType.GOLD, 0 },
                { OreType.DIAMOND, 0 }
            };
    }

    public void AddOre(OreType oreType)
    {
        OreInventory[oreType]++;
    }

    // returns whether or not there was anything to sell
    public bool ConvertOresToMoney()
    {
        bool success = false;

        foreach(OreType oreType in new List<OreType>(OreInventory.Keys))
        {
            if (OreInventory[oreType] > 0) success = true;

            AddMoney(OreInventory[oreType] * oreValues.Values[oreType]);
            OreInventory[oreType] = 0;
        }

        return success;
    }

    public int GetTotalInventoryValue()
    {
        int total = Money;
        foreach (OreType oreType in new List<OreType>(OreInventory.Keys))
        {
            total += OreInventory[oreType] * oreValues.Values[oreType];
        }

        return total;
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void RemoveMoney(int amount)
    {
        Money -= amount;
    }

    public bool CanAfford(int purchaseAmount)
    {
        return Money >= purchaseAmount;
    }
}
