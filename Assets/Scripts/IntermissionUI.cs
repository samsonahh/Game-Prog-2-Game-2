using DG.Tweening;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntermissionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] private GameManager gameManager;
    [SerializeField] private TMP_Text timerText;

    private bool closeStarted;
    private float timer;

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        gameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDestroy()
    {
        gameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (gameManager.CurrentState != GameManager.GameState.INTERMISSION) return;

        timer -= Time.unscaledDeltaTime;

        timerText.text = $"{timer.ToString("F0")}";

        if(timer < 0 && !closeStarted)
        {
            Close();
        }
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.INTERMISSION)
        {
            return;
        }

        Open();
    }

    private void Open()
    {
        closeStarted = false;

        DOTween.Kill(transform);

        gameObject.SetActive(true);

        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() => {

        }).SetUpdate(true);

        timer = gameManager.QuotaIntermissionDuration;
    }

    private void Close()
    {
        closeStarted = true;

        gameManager.ChangeState(GameManager.GameState.PLAYING);

        DOTween.Kill(transform);

        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).OnComplete(() => {
            gameObject.SetActive(false);
        }).SetUpdate(true);
    }
}