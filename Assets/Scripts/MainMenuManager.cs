using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas _mainMenu;
    [SerializeField] private Canvas _tutorialMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        Time.timeScale = 1.0f;
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
