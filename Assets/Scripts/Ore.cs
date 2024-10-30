using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        DOTween.Kill(transform);
        transform.DOScale(0.85f * Vector3.one, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuint);

        currentHealth-= damage;

        if(currentHealth <= 0f && maxHealth > 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        DOTween.Kill(transform);
        transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack).OnComplete(() => { Destroy(gameObject); });
    }

    private void OnDestroy()
    {
        
    }
}
