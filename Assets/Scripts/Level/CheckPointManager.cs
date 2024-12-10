using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public CheckpointComponent CurrentCheckPoint => currentCheckpoint;

    private CheckpointComponent currentCheckpoint;

    public void UpdateCheckPoint(CheckpointComponent checkpointComponent)
    {
        currentCheckpoint = checkpointComponent;
    }
}
