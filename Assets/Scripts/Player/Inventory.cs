using KBCore.Refs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OreValues oreValues;

    [field: Header("Settings")]
    [field: SerializeField] public int Money { get; private set; }
    public Dictionary<OreType, int> OreInventory { get; private set; }

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

    public void ConvertOresToMoney()
    {
        foreach(OreType oreType in new List<OreType>(OreInventory.Keys))
        {
            AddMoney(OreInventory[oreType] * oreValues.Values[oreType]);
            OreInventory[oreType] = 0;
        }
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
