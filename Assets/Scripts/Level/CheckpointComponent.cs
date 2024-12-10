using UnityEngine;

public class CheckpointComponent : MonoBehaviour
{
    public bool IsActivated => isActivated;
    public int SceneIndexToReload => sceneIndexToReload;
    
    [SerializeField] private GameObject checkPointVFX;
    [SerializeField] private int sceneIndexToReload;

    private bool isActivated = false;
    private CheckPointManager checkPointManager;
    private void Awake()
    {
        checkPointManager = GetComponentInParent<CheckPointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ICombatInterface combatInterface = other.gameObject.GetComponent<ICombatInterface>();
        if(combatInterface != null && combatInterface.GetCharacterTag() == "Player" && !combatInterface.IsDead)
        {
            isActivated = true;
            checkPointVFX.SetActive(true);
            checkPointManager.UpdateCheckPoint(this);
        }
    }
}
