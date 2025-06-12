using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas _mainMenu;
    [SerializeField] private Canvas _tutorialMenu;

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Tuto()
    {
        _mainMenu.gameObject.SetActive(false);
        _tutorialMenu.gameObject.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void MainMenu()
    {
        _mainMenu.gameObject.SetActive(true);
        _tutorialMenu.gameObject.SetActive(false);
    }
}
