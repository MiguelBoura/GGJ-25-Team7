using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BubbleController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public bool canMove;
    public float moveSpeed;
    public float puffForce;
    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;
    public float burstTime;
    private float timeToBurst;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        isOnGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        //move left to right
        theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);

        //puff up to move up
        if (Input.GetButtonDown("Jump"))
        {
            theRB.velocity = new Vector2(theRB.velocity.x, puffForce);
        }

        //check if on ground
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

        //if on ground, start countdown
        if (isOnGround)
        {
            timeToBurst = burstTime;
            timeToBurst -= Time.deltaTime;

            if (timeToBurst < 0)
            {
                Destroy(gameObject);
            }
        }

    }

}
