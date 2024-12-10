using System.Collections;
using UnityEngine;

public class BossLight : MonoBehaviour
{
    [SerializeField] private GameObject[] lightGroups;
    [SerializeField] private float delayBetweenLights = 1f;

    private void Awake()
    {
       if (lightGroups.Length == 0)
        {
            print("No lights assigned in the Inspector.");
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        StartCoroutine(EnableLightGroups());
            
    }

    IEnumerator EnableLightGroups()
    {
        print("LightingUp");
        
        foreach (GameObject group in lightGroups)
        {
            if (group != null)
            {
                yield return new WaitForSeconds(delayBetweenLights);
                group.SetActive(true);
            }
        }
    }


}
