using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    public int TriggerIndex => triggerIndex;
    
    
    [SerializeField] private int triggerIndex;
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private Material success;
    [SerializeField] private Material fail;


    private Material defaultmaterial;
    private Renderer meshRenderer;
    private bool succeeded;
    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        defaultmaterial = meshRenderer.material;

        puzzleManager.OnPuzzleReset += OnReset;
        puzzleManager.OnPuzzleSucceeded += OnSuccess;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!succeeded && other.gameObject.CompareTag("Projectile"))
        {
            meshRenderer.material = success;
            puzzleManager.AddTrigger(this);
        }
    }

    private void OnReset()
    {
        meshRenderer.material = fail;
        Invoke(nameof(SetDefaultColor),1f);
    }

    private void SetDefaultColor()
    {
        meshRenderer.material = defaultmaterial;
    }
    private void OnSuccess()
    {
        succeeded = true;
    }
}
