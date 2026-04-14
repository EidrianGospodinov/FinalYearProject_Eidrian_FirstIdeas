using _Scripts.StateMachine.PlayerActionStateMachine;

public class RespawnService
{
    private CheckpointCamp currentCheckpoint;
    private readonly IDeathMenu deathMenu;
    
    public RespawnService(IDeathMenu deathMenu)
    {
        this.deathMenu = deathMenu;
    }

    public void ShowMenu(PlayerController playerController) => deathMenu.Show(playerController);
    public void HideMenu() => deathMenu.Hide();
    
    public bool IsActiveCheckpoint(CheckpointCamp checkpointCamp) => checkpointCamp == currentCheckpoint;
    public void SetCheckpoint(CheckpointCamp checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void Respawn(PlayerController player)
    {
        currentCheckpoint?.PlacePlayerInCamp();
        player.ResetState();
    }
}