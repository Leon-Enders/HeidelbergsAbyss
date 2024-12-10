using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnRandomLightnings : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn; 
    [SerializeField] private float spawnInterval = 0.1f;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float abilityLifeTime = 5f;
    [SerializeField] private float thunderLifeTime;
    
    private void Start()
    {
        
    }


    private void OnEnable()
    {
        StartCoroutine(ActivateAbility());
        StartCoroutine(DisableAbility());
    }
    

    IEnumerator ActivateAbility()
    {
        while (enabled)
        {
            Vector3 spawnPosition = GetRandomPositionInSphere(); 

            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            Destroy(spawnedObject, thunderLifeTime);

            yield return new WaitForSeconds(spawnInterval); 
        }
    }

    IEnumerator DisableAbility()
    {
        yield return new WaitForSeconds(abilityLifeTime);
        enabled = false;
    }
    
    Vector3 GetRandomPositionInSphere()
    {
        Vector2 randomPointInCircle = Random.insideUnitCircle * spawnRadius;
        
        Vector3 randomPosition = new Vector3(randomPointInCircle.x, transform.localPosition.y, randomPointInCircle.y);
       
        return transform.TransformPoint(randomPosition);
    }
}