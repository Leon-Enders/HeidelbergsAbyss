using UnityEngine;

public class BossDoorTrigger : MonoBehaviour
{
    public DoorManager doorManager;
    [SerializeField] GameObject door;

    void OnTriggerEnter(Collider other)
    {
        doorManager.HandleCloseDoor();
    }






}
