using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameOverScene : MonoBehaviour
{
    [Header("Scene")]
    [Tooltip("Name of the scene to load.")]
    public string sceneName;
    public BubbleController bubble;

    private void OnDestroy()
    {
        // Load the Game Over scene when the player is destroyed
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not set!");
        }
    }
}
