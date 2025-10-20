using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneMan : MonoBehaviour
{
    //[SerializeField] string sceneName;

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
