using UnityEngine;
using UnityEngine.SceneManagement;

namespace PopovRadio.Scripts.Common
{
    public class SceneLoader : MonoBehaviour
    {
        public static void LoadScene(string name)
        {
            SceneManager.LoadSceneAsync(name);
        }
    }
}