using DG.Tweening;
using KBCore.Refs;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] private GameManager gameManager;
    [SerializeField, Scene] private Inventory inventory;
    [SerializeField] private Button closeShopButton;

    [Header("Inventory References")]
    [SerializeField] private TMP_Text oreCountsText;
    [SerializeField] private TMP_Text moneyText;

    [Header("Sell References")]
    [SerializeField] private Button sellInventoryButton;

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        sellInventoryButton.onClick.AddListener(SellInventory);
        closeShopButton.onClick.AddListener(Close);

        gameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDestroy()
    {
        sellInventoryButton.onClick.RemoveListener(SellInventory);
        closeShopButton.onClick.RemoveListener(Close);

        gameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        if(state != GameManager.GameState.SHOPPING)
        {
            return;
        }

        Open();
    }

    private void Open()
    {
        DOTween.Kill(transform);

        gameObject.SetActive(true);
        UpdateOreCounts(inventory.OreInventory);

        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() => {
            
        }).SetUpdate(true);
    }

    private void Close()
    {
        DOTween.Kill(transform);

        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).OnComplete(() => {
            gameObject.SetActive(false);
            gameManager.ChangeState(GameManager.GameState.PLAYING);
        }).SetUpdate(true);
    }

    private void UpdateOreCounts(Dictionary<OreType, int> oreCounts)
    {
        string oreCountsString = "";

        foreach (KeyValuePair<OreType, int> oreCount in oreCounts)
        {
            oreCountsString += $"{oreCount.Key}: {oreCount.Value}\n";
        }

        oreCountsText.text = oreCountsString;
    }

    private void SellInventory()
    {
        bool success = inventory.ConvertOresToMoney();
        UpdateOreCounts(inventory.OreInventory);

        if(success) AudioManager.Instance.Play(AudioManager.Instance.ChaChingSFX, 0.25f);

        moneyText.text = $"MONEY: ${inventory.Money}";
    }
}
