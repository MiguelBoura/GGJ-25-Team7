using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour
{
    public BubbleController bubbleController;
    [SerializeField] private float windForce;
    [SerializeField] private bool isBlowing;
    [SerializeField] private Transform fanForward;


    // Start is called before the first frame update
    void Start()
    {
        isBlowing = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Something entered the trigger: " + other.name);

        if (isBlowing && other.CompareTag("Player"))
        {
            Debug.Log("Bubble in wind area!");
            Rigidbody2D theRB = other.GetComponent<Rigidbody2D>();
            if (theRB != null)
            {
                Vector2 windDirection = fanForward.right.normalized;

                theRB.AddForce(windDirection * windForce, ForceMode2D.Force);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (fanForward != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(fanForward.position, fanForward.position + fanForward.right * 2f);
        }
    }
}
