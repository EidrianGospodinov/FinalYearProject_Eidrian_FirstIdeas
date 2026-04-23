using _Scripts.Units.Player.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [Inject] private GameManager gameManager;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject inputsScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (gameManager.GetCurrentGameState)
            {
                case GameState.InGame:
                    gameManager.SetGameState(GameState.Pause);
                    pauseMenu.SetActive(true);
                    break;
                case GameState.Pause:
                    gameManager.SetGameState(GameState.InGame);
                    pauseMenu.SetActive(false);
                    inputsScreen.SetActive(false);
                    break;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log(gameManager.GetCurrentGameState);
        }
    }
    public void OpenInputsScreen()
    {
        inputsScreen.SetActive(true);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
