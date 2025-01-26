using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameOverScene : MonoBehaviour
{
    [Header("Game Over Scene")]
    [Tooltip("Name of the Game Over scene to load.")]
    public string gameOverSceneName = "GameOver";

    private void OnDestroy()
    {
        // Load the Game Over scene when the player is destroyed
        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("Game Over scene name is not set!");
        }
    }
}
