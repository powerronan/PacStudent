using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI gameTimerText;

    private float gameTime = 0f;
    private bool isGameRunning = false;

    void Awake()
    {
        // Implementing Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;

            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize the game timer
        gameTime = 0f;
        isGameRunning = true;
    }

    void Update()
    {
        if (isGameRunning)
        {
            gameTime += Time.deltaTime;

            UpdateGameTimerUI();
        }
    }

    void UpdateGameTimerUI()
    {
        if (gameTimerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60f);
            int seconds = Mathf.FloorToInt(gameTime % 60f);
            int hundredths = Mathf.FloorToInt((gameTime * 100f) % 100f);

            gameTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
        }
    }

    public void StopGame()
    {
        isGameRunning = false;
        SaveBestTime();
    }

    void SaveBestTime()
    {
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (gameTime < bestTime)
        {
            // Save the new best time
            PlayerPrefs.SetFloat("BestTime", gameTime);
            PlayerPrefs.Save();
        }
    }
}

