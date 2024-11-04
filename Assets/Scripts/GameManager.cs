using KBCore.Refs;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] private Inventory playerInventory;
    [SerializeField] private TMP_Text quotaText;
    [SerializeField] private TMP_Text quotaTimerText;
    [SerializeField] private Transform oreSpawnsParent;
    [SerializeField] private List<Ore> orePrefabs;
    private List<Ore> oresList = new List<Ore>();

    #region State Machine
    // using enum state machine instead of generic BaseState classes for less bloat
    public enum GameState
    {
        PLAYING,
        PAUSED,
        SHOPPING,
        START,
        LOSE,
        INTERMISSION
    }
    public GameState CurrentState { get; private set; }
    public Action<GameState> OnStateChanged = delegate { };
    #endregion

    [Header("Quota Settings")]
    [SerializeField] private int baseQuota = 30;
    [SerializeField] private float quotaLinearGrowth = 20f;
    [SerializeField] private float quotaExponentialGrowth = 1.025f;
    [SerializeField] private float quotaDuration = 30f;
    [field: SerializeField] public float QuotaIntermissionDuration { get; private set; } = 3f;
    public int CurrentQuota { get; private set; }
    public int CurrentQuotaRequirement { get; private set; }
    private float quotaTimer;
    public Action OnQuotaEnd = delegate { };

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Start()
    {
        ChangeState(GameState.START);

        CurrentQuotaRequirement = baseQuota;
        quotaTimer = quotaDuration;

        RespawnAllOres();
    }

    private void Update()
    {
        UpdateState(CurrentState);

        HandleShopInput();
        HandlePauseInput();

        HandleQuota();
    }

    #region State Machine Functions
    private void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.PLAYING:
                break;
            case GameState.PAUSED:
                break;
            case GameState.SHOPPING:
                break;
            case GameState.START:
                break;
            case GameState.LOSE:
                break;
            case GameState.INTERMISSION:
                break;
            default:
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        // exit current state
        switch(CurrentState)
        {
            case GameState.PLAYING:
                break;
            case GameState.PAUSED:
                break;
            case GameState.SHOPPING:
                break;
            case GameState.START:
                break;
            case GameState.LOSE:
                break;
            case GameState.INTERMISSION:
                break;
            default:
                break;
        }

        CurrentState = newState;

        // enter new state
        switch (newState)
        {
            case GameState.PLAYING:
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                break;
            case GameState.PAUSED:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            case GameState.SHOPPING:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            case GameState.START:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            case GameState.LOSE:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            case GameState.INTERMISSION:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            default:
                break;
        }

        OnStateChanged?.Invoke(CurrentState);
    }
    #endregion

    private void RespawnAllOres()
    {
        foreach(Ore ore in new List<Ore>(oresList))
        {
            if(ore != null) Destroy(ore.gameObject);
        }
        oresList.Clear();

        foreach(Transform spot in oreSpawnsParent)
        {
            float randomFloat = UnityEngine.Random.value;
            int randomIndex = Mathf.FloorToInt(orePrefabs.Count * Mathf.Pow(randomFloat, Mathf.Clamp(20 * Mathf.Pow(0.8f, CurrentQuota), 2, 20)));

            Quaternion randomRot = Quaternion.Euler(UnityEngine.Random.Range(0, 180f), UnityEngine.Random.Range(0, 180f), UnityEngine.Random.Range(0, 180f));
            oresList.Add(Instantiate(orePrefabs[randomIndex], spot.position, randomRot, transform));
        }
    }

    private void HandleShopInput()
    {
        if (CurrentState != GameState.PLAYING) return;

        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeState(GameState.SHOPPING);
        }
    }

    private void HandlePauseInput()
    {
        if (CurrentState != GameState.PLAYING) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(GameState.PAUSED);
        }
    }

    private void HandleQuota()
    {
        if(quotaTimer > -1f) quotaTimer -= Time.deltaTime;

        if (quotaTimer < 0)
        {
            quotaTimer = quotaDuration;
            OnQuotaEnd?.Invoke();
        }

        quotaText.text = $"QUOTA {CurrentQuota + 1}: {playerInventory.Money}/{CurrentQuotaRequirement}";

        if (playerInventory.GetTotalInventoryValue() < CurrentQuotaRequirement) quotaText.color = Color.red;
        if (playerInventory.GetTotalInventoryValue() >= CurrentQuotaRequirement && !playerInventory.CanAfford(CurrentQuotaRequirement)) quotaText.color = Color.yellow;
        if (playerInventory.CanAfford(CurrentQuotaRequirement)) quotaText.color = Color.green;
        quotaTimerText.text = $"REMAINING TIME: {quotaTimer.ToString("F2")} s";
    }

    public void StartNextQuota()
    {
        AudioManager.Instance.Play(AudioManager.Instance.SuccessSFX, 0.35f, 0.15f * UnityEngine.Random.Range(-1f, 0.5f) + 1f);

        playerInventory.transform.position = Vector3.up;

        ChangeState(GameState.INTERMISSION);

        CurrentQuota++;
        CurrentQuotaRequirement = (int)Math.Floor(baseQuota + CurrentQuota * quotaLinearGrowth + Mathf.Pow(CurrentQuota, quotaExponentialGrowth));

        playerInventory.GetComponent<Player>().BuffMineDuration(CurrentQuota);

        RespawnAllOres();
    }
}
