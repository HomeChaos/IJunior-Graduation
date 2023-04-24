using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class RestartScene: MonoBehaviour
    {
        private void Restart()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
    }
}