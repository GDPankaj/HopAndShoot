using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    GameObject EndLevelMenu;
    GameObject EndGameMenu;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject); 
        }

    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindMenus();
    }
    private void FindMenus()
    {
        EndLevelMenu = GameObject.FindGameObjectWithTag("EndLevelMenu");
        EndGameMenu = GameObject.FindGameObjectWithTag("EndGameMenu");

        if (EndLevelMenu == null || EndGameMenu == null)
        {
            Debug.LogWarning("EndLevelMenu or EndGameMenu is missing in the scene.");
        }


        if (EndLevelMenu != null)
        {
            EndLevelMenu.SetActive(false);
        }
        if(EndGameMenu != null)
        {
            EndGameMenu.SetActive(false);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenEndLevelMenu(bool isLastLevel)
    {
        if (!isLastLevel)
        {
            EndLevelMenu.SetActive(true);
        }
        else
        {
            EndGameMenu.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        int nextScene =  SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings) 
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        }
        else
        {
            Debug.Log("There is no last level assign this scene as Last level in SpaceShip OBJ");
        }
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("GameQuit");
    }

    public void ButtonClicked()
    {
        AudioManager.instance?.PlaySFX("Click");
    }
}
