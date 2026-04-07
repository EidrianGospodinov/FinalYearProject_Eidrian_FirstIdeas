public class RespawnService
{
    private CheckpointCamp _currentCheckpoint;

    public bool IsActiveCheckpoint(CheckpointCamp checkpointCamp) => checkpointCamp == _currentCheckpoint;
    public void SetCheckpoint(CheckpointCamp checkpoint)
    {
        _currentCheckpoint = checkpoint;
    }

    public void Respawn(PlayerController player)
    {
        _currentCheckpoint?.PlacePlayerInCamp(player);
        player.ResetState();
    }
}