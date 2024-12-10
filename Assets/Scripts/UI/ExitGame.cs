using UnityEngine.SceneManagement;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("mainmenue-scene");     
    }
}
