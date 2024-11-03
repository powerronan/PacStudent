using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartScreenManager : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;

    void Start()
    {
        UpdateBestTimeUI();
    }

    void UpdateBestTimeUI()
    {
        // Retrieve the best time from PlayerPrefs
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);

        if (bestTime > 0f && bestTime < float.MaxValue)
        {
            int minutes = Mathf.FloorToInt(bestTime / 60f);
            int seconds = Mathf.FloorToInt(bestTime % 60f);
            int hundredths = Mathf.FloorToInt((bestTime * 100f) % 100f);

            bestTimeText.text = string.Format("Best Time: {0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
        }
        else
        {
            bestTimeText.text = "Best Time: 00:00:00";
        }
    }
}
