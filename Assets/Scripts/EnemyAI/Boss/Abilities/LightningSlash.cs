using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSlash : MonoBehaviour
{
    [SerializeField] private GameObject lightningPrefab;  
    [SerializeField] private int numberOfLightnings = 6; 
    [SerializeField] private float abilityRange = 10f;   
    [SerializeField] private BossController bossController;


    [SerializeField] private float spawnInterval = 0.1f;
    [SerializeField] private float thunderLifeTime = 1f;

    private void OnEnable()
    {
        print("slashing...");
        StartCoroutine(ActivateAbility());
    }
    IEnumerator ActivateAbility()
    {
        Vector3 bossPosition = transform.position;
        Vector3 playerPosition = bossController.playerPos;

        Vector3 directionToPlayer = (playerPosition - bossPosition).normalized;

        float distanceBetweenLightnings = abilityRange / (numberOfLightnings - 1);

        for (int i = 0; i < numberOfLightnings; i++)
        {
            Vector3 lightningPosition = bossPosition + directionToPlayer * distanceBetweenLightnings * i;

            GameObject spawnedObject = Instantiate(lightningPrefab, lightningPosition, Quaternion.identity);
            Destroy(spawnedObject, thunderLifeTime);
            yield return new WaitForSeconds(spawnInterval);
        }
        enabled = false;
    }
}
