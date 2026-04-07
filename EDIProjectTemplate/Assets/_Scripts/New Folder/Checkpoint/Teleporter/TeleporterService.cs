using System.Collections.Generic;

namespace _Scripts.New_Folder.Checkpoint
{
    public class TeleporterService
    {
        private List<CheckpointCamp> discoveredCamps = new List<CheckpointCamp>();

        public void RegisterCamp(CheckpointCamp camp)
        {
            if (!discoveredCamps.Contains(camp))
                discoveredCamps.Add(camp);
        }

        public void TeleportPlayerTo(CheckpointCamp checkpointCamp)
        {
            if (discoveredCamps.Contains(checkpointCamp))
            {
                checkpointCamp.PlacePlayerInCamp();
            }
        }

        public IReadOnlyList<CheckpointCamp> GetDiscoveredCamps() => discoveredCamps;

    }
}