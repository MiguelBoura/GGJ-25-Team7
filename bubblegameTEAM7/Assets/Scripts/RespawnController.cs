using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Vector3 respawnPoint;
    public float waitToRespawn;

    private GameObject bubble;

    // Start is called before the first frame update
    void Start()
    {
       // bubble = BubbleController.instance.gameObject;
       // respawnPoint = bubble.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo()
    {
        //bubble.SetActive(false);

        yield return new WaitForSeconds(waitToRespawn);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //bubble.transform.position = respawnPoint;
        //bubble.SetActive(true);
    }
}
