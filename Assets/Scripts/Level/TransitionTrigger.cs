using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionTrigger : MonoBehaviour
{
    
    [SerializeField] private List<int> sceneIndicesToLoad;
    
    [SerializeField] private List<int> sceneIndicesToUnLoad;
    
        private void OnTriggerEnter(Collider other)
    {
        foreach (var sceneIndexToUnLoad in sceneIndicesToUnLoad)
        {
            if (SceneManager.GetSceneByBuildIndex(sceneIndexToUnLoad).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneIndexToUnLoad);
            }
        }
        
        
        foreach (var sceneIndexToLoad in sceneIndicesToLoad)
        {
            if (!SceneManager.GetSceneByBuildIndex(sceneIndexToLoad).isLoaded)
            {
                SceneManager.LoadSceneAsync(sceneIndexToLoad, LoadSceneMode.Additive);
            }
        }
    }
}
