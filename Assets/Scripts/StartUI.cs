using DG.Tweening;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] private GameManager gameManager;
    [SerializeField] private Button startButton;

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);

        gameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(StartGame);

        gameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void Start()
    {
        
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.START)
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

    private void StartGame()
    {
        DOTween.Kill(transform);

        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).OnComplete(() => {
            gameObject.SetActive(false);
            gameManager.ChangeState(GameManager.GameState.PLAYING);
        }).SetUpdate(true);
    }
}
