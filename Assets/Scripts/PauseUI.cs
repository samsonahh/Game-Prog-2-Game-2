using DG.Tweening;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] private GameManager gameManager;
    [SerializeField] private Button resumeButton;

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        resumeButton.onClick.AddListener(Resume);

        gameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(Resume);

        gameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.PAUSED)
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

    private void Resume()
    {
        DOTween.Kill(transform);

        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).OnComplete(() => {
            gameObject.SetActive(false);
            gameManager.ChangeState(GameManager.GameState.PLAYING);
        }).SetUpdate(true);
    }
}
