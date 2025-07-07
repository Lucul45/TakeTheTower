using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Canvas _winCanvas;

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void WinScreen(string deadPlayerID)
    {
        Time.timeScale = 0f;
        if (deadPlayerID == "Player1")
        {
            _text.text = "Player 2 wins !";
        }
        else if (deadPlayerID == "Player2")
        {
            _text.text = "Player 1 wins !";
        }
        _winCanvas.gameObject.SetActive(true);
    }
}
