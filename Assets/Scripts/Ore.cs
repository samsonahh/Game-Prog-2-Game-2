using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    private Inventory playerMoney;

    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private OreType oreType;

    private void Awake()
    {
        playerMoney = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        DOTween.Kill(gameObject);

        Sequence sequence = DOTween.Sequence().SetId(gameObject);
        sequence.Append(transform.DOScale(0.85f * Vector3.one, 0.05f).SetEase(Ease.OutQuint));
        sequence.Append(transform.DOScale(1f * Vector3.one, 0.15f).SetEase(Ease.InQuint));

        currentHealth -= damage;

        if(currentHealth <= 0f && maxHealth > 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        DOTween.Kill(gameObject);
        transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack).OnComplete(() => { Destroy(gameObject); });

        playerMoney.AddOre(oreType);
    }

    private void OnDestroy()
    {
        
    }
}

public enum OreType
{
    COPPER,
    IRON,
    SILVER,
    GOLD,
    DIAMOND
}