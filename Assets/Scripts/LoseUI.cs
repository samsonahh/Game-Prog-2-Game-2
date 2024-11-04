using DG.Tweening;
using KBCore.Refs;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] private GameManager gameManager;
    [SerializeField] private Button retryButton;

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        retryButton.onClick.AddListener(Retry);

        gameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDestroy()
    {
        retryButton.onClick.RemoveListener(Retry);

        gameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.LOSE)
        {
            return;
        }

        Open();
    }

    private void Open()
    {
        DOTween.Kill(transform);

        gameObject.SetActive(true);

        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() => {

        }).SetUpdate(true);
    }

    private void Retry()
    {
        SceneManager.LoadScene("Menu");
    }
}
