using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class RespawnPanel : MonoBehaviour, IDeathMenu
{
    [Inject] private GameManager gameManager;
    [Inject] private RespawnService respawnService;
    [Inject] private ICameraService cameraService;
    private PlayerController player;

    public void ContinueToGame()
    {
        gameManager.SetGameState(GameState.InGame);
        cameraService.EnableDeathCam(false);
        respawnService.Respawn(player);
        gameObject.SetActive(false);
    }

    public void Show(PlayerController playerController)
    {
        player = playerController;
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);
}
public interface IDeathMenu
{
    void Show(PlayerController playerController);
    void Hide();
}

