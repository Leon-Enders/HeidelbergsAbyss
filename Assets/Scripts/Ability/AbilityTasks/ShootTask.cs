using GameStates;
using UnityEngine;
[CreateAssetMenu(fileName = "Shoot Task", menuName = "CastleAdventure/Ability/ShootTask")]
public class ShootTask : AbilityTask
{

    [SerializeField] private LayerMask shootLayer;
    [SerializeField] private GameObject projectile;


    private bool isShooting = false;
    protected override bool HandleAbility()
    {
        if (projectile == null || isShooting || !interfaceContainer.AbilityInterface.IsAiming) return false;
        
        isShooting = true;
        interfaceContainer.PlayableInterface.OnAnimationFinished += OnShootFinished;
       
        Quaternion spawnRotation = interfaceContainer.AbilityInterface.AimTransform.rotation;
        Vector3 spawnPosition = interfaceContainer.AbilityInterface.CrossbowSocket.position + interfaceContainer.AbilityInterface.AimTransform.forward * 1.25f;
        
        
        //TODO: playerCam reference variable on abilityInterface or maybe even combatInterface?
        
        Camera playerCam = interfaceContainer.AbilityInterface.Owner.GetComponentInChildren<Camera>();
        if (playerCam != null)
        {
            Vector3 cameraPosition = playerCam.transform.position;
            Vector3 viewDir = playerCam.transform.forward;
            
            if(Physics.Raycast(cameraPosition, viewDir,out RaycastHit hitInfo,10000f, shootLayer))
            {
                Vector3 hitDirection = ( hitInfo.point - spawnPosition).normalized;
                spawnRotation = Quaternion.LookRotation(hitDirection);
            }
        }

        
            
        GameObject spawnedProjectile = Instantiate(projectile,
            spawnPosition,
            spawnRotation);
        
        Destroy(spawnedProjectile, 10f);
        
        
        base.HandleAbility();
        return true;
    }

    private void OnShootFinished()
    {
        isShooting = false;
        interfaceContainer.PlayableInterface.OnAnimationFinished -= OnShootFinished;
    }
}
