using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class SetYourNamePanel : MonoBehaviour
{
    [Inject] private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //adding this so i don't see it in the inspector
        GetComponent<CanvasGroup>().alpha = 1;
        gameManager.SetGameState(GameState.InMenu);
    }
    

    public void SetYourName(string name)
    {
        GameConfig.PlayerName = name;
    }

    public void ContinueToGame()
    {
        gameManager.SetGameState(GameState.InGame);
        gameObject.SetActive(false);
    }
}
