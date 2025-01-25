using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BubbleController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public CircleCollider2D bubbleCollider;
    public BoxCollider2D soapCollider;
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
    [SerializeField] private LayerMask whatIsSoap;
    private bool isOnSoap;
    private bool bubblePopped;
    [SerializeField] private float iceTime;
    private float timeUntilNoLongerIce;
    [SerializeField] private float iceFriction = 0.5f;
    [SerializeField] private float normalDrag = 1f;
    [SerializeField] DishSoapController soapController;
    [SerializeField] private int requiredButtonPresses = 5;
    private int jumpPressed = 0;
    private float resetPressTimer = 1f;
    private float pressTimer = 0f;

    private bool hasTouchedDangerObject;
    [SerializeField] private LayerMask whatIsDanger;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        isOnGround = false;
        isOnSoap = false;
        theRB = GetComponent<Rigidbody2D>();
        bubbleCollider = GetComponent<CircleCollider2D>();
        soapCollider = GetComponent<BoxCollider2D>();
        timeUntilNoLongerIce = iceTime;
        bubblePopped = false;
        hasTouchedDangerObject = false;
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

        //check if on soap
        isOnSoap = Physics2D.OverlapCircle(groundPoint.position, .1f, whatIsSoap);

        //if on ground and not iced, start countdown. Does not run if player is iced.
        if (isOnGround && !isIced)
        {
            anim.SetBool("isOnGround", true);
            timeToBurst -= Time.deltaTime;

            if (timeToBurst < 0)
            {
                StartCoroutine(BalloonPop());
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

        if(isOnSoap && !isIced)
        {
            isOnGround = false;
            canMove = false;
            theRB.velocity = new Vector2(0, 0);

            //bubble stuck
            if (Input.GetButtonDown("Jump"))
            {
                jumpPressed++;
                pressTimer = resetPressTimer;

                if (jumpPressed >= requiredButtonPresses)
                {
                    FreeBubble();
                }
            }
            if (pressTimer > 0)
            {
                pressTimer -= Time.deltaTime;
            }
            else
            {
                jumpPressed = 0;
            }

        }
        else
        {
            canMove = true;
        }
    }

    public void killBubble()
    {
        StartCoroutine(BalloonPop());
    }

    private void FreeBubble()
    {
        theRB.velocity = new Vector2(theRB.velocity.x, puffForce);
        isOnSoap = false;
        canMove = true;
        jumpPressed = 0;

        if (soapController != null)
        {
            soapController.DisableSoapSurface();
        }
    }
    IEnumerator BalloonPop()
    {
        bubblePopped = true;
        anim.SetBool("isPopped", true);
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}
