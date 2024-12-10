using System.Collections;
using UnityEngine;

public class LightningHunt : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn; 
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float abilityLifeTime = 5f;
    [SerializeField] private float thunderLifeTime = 1f;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private BossController bossController;
    private Vector3 playerPos;
    private Vector3 spawnPos;
    
    void FixedUpdate()
    {
        playerPos = bossController.playerPos;
    }
    private void OnEnable()
    {
        print("hunting...");
        StartCoroutine(ActivateAbility());
        StartCoroutine(DisableAbility());
    }

    IEnumerator ActivateAbility()
    {
        while(enabled)
        {
            spawnPos = playerPos;
            yield return new WaitForSeconds(spawnDelay);

            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            Destroy(spawnedObject, thunderLifeTime);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator DisableAbility()
    {
        yield return new WaitForSeconds(abilityLifeTime);
        enabled = false;
    }
}
