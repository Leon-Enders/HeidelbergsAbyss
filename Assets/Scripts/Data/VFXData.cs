using UnityEngine;


[CreateAssetMenu(fileName = "VFX Data", menuName = "CastleAdventure/VFXData")]
public class VFXData : ScriptableObject
{
    public float SpawnDelay => spawnDelay;
    public float LifeTime => lifetime;
    public GameObject Prefab => prefab;
    
    
    [SerializeField] private float spawnDelay = 0.3f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private GameObject prefab;
    
    
    public bool IsValid => prefab != null;
}
