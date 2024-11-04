using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
