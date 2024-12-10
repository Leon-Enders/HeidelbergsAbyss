using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour, IVFXInterface
{
    
    
    public void SpawnVFX(VFXData vfxData, Transform spawnTransform)
    {
        if (!vfxData.IsValid)
        {
            //print("vfxData not valid, no vfx spawned");
            return;
        }
        StartCoroutine(SpawnVFXCoroutine(vfxData, spawnTransform));
    }

    private IEnumerator SpawnVFXCoroutine(VFXData vfxData, Transform spawnTransform)
    {
        yield return new WaitForSeconds(vfxData.SpawnDelay);
       
        GameObject vfxEffect = Instantiate(vfxData.Prefab, spawnTransform);
        Destroy(vfxEffect, vfxData.LifeTime);
    }
}
