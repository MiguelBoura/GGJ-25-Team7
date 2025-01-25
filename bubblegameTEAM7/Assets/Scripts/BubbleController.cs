using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BubbleController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public CircleCollider2D bubbleCollider;
    public bool canMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationSpeed = 1f;
    [SerializeField] private float decelerationSpeed = 1f;
    public float puffForce;
    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;
    public float burstTime;
    private float timeToBurst;
    private float currentVelocityX = 0f;
    public LayerMask whatIsIce;
    private bool isOnIce;
    private bool isIced;
    [SerializeField] private float iceTime;
    private float timeUntilNoLongerIce;
    [SerializeField] private float iceFriction = 0.5f;
    [SerializeField] private float normalDrag = 1f;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        isOnGround = false;
        theRB = GetComponent<Rigidbody2D>();
        bubbleCollider = GetComponent<CircleCollider2D>();
        timeUntilNoLongerIce = iceTime;
    }

    // Update is called once per frame
    void Update()
    {
        float targetSpeed = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            //gradually increasr towards speed
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetSpeed, accelerationSpeed * Time.deltaTime);
        } else
        {
            //gradually slow down
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, 0, decelerationSpeed * Time.deltaTime);
        }

        //move left to right
        theRB.velocity = new Vector2(currentVelocityX, theRB.velocity.y);

        //puff up to move up
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("isPuffed");
            theRB.velocity = new Vector2(theRB.velocity.x, puffForce);
        }

        //check if on ground
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);
        //check if on ice
        isOnIce = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsIce);

        //if on ground and not iced, start countdown. Does not run if player is iced.
        if (isOnGround && !isIced)
        {
            anim.SetBool("isOnGround", true);
            timeToBurst -= Time.deltaTime;

            if (timeToBurst < 0)
            {
                Destroy(gameObject);
            }
        }

        else
        {
            timeToBurst = burstTime;
            anim.SetBool("isOnGround", false);
        }

        //if on ice, turn to ice for time period
        if (isOnIce)
        {
            isIced = true;
            anim.SetBool("isIced", true);
        }

        if (isIced) { 
            timeUntilNoLongerIce -= Time.deltaTime;

            theRB.gravityScale = 2f;
            theRB.drag = iceFriction;

            theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y);

           if (timeUntilNoLongerIce <= 0)
            {
                isIced = false;
                anim.SetBool("isIced", false);
                timeUntilNoLongerIce = iceTime;

                theRB.gravityScale = 0.25f;
                theRB.drag = normalDrag;
            }
        }
    }

}
