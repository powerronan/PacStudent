using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("PacStudent");
    }
}
