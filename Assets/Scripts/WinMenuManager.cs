using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuManager : Singleton<WinMenuManager>
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Canvas _winCanvas;

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void WinScreen(string winnerPlayerID)
    {
        Time.timeScale = 0f;
        if (winnerPlayerID == "Player1")
        {
            _text.text = "Player 1 wins !";
        }
        else if (winnerPlayerID == "Player2")
        {
            _text.text = "Player 2 wins !";
        }
        _winCanvas.gameObject.SetActive(true);
    }
}
