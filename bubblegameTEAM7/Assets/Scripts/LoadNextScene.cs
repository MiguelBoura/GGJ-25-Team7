using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [Header("Scene")]
    [Tooltip("Name of the scene to load.")]
    public string sceneName;
    public BubbleController bubble;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bubble.LevelWon();
            StartCoroutine(waitToLoadScene());
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private IEnumerator waitToLoadScene()
    {
        yield return new WaitForSeconds(1f);
    }
}
