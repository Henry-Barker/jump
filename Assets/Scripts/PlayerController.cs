using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float chargeTime;
    public float chargeMax;
    public float chargeSpeed;
    public float horJumpForce;
    public float verJumpForce;
    public bool tabPressed;
    private BoxCollider2D boxCollider2d;

    private Rigidbody2D theRB2D;

    public bool canMove;

    public int direction;

    public Animator animator;

    public GameObject cameraObject;

    private SpriteRenderer _renderer;
    public PhysicsMaterial2D physicsMaterial;

    [SerializeField] private LayerMask platformLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        theRB2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0  && IsGrounded())
        {
            direction = -1;
            _renderer.flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0 && IsGrounded())
        {
            direction = 0;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && IsGrounded())
        {
            direction = 1;
            _renderer.flipX = false;
        }

        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Tab))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            tabPressed = true;
        }
        else
        {
            tabPressed = false;
        }

        if(theRB2D.velocity.x != 0f && IsGrounded())
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        //Bounce();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        if (IsGrounded())
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    void Move()
    {
        if (canMove && IsGrounded())
        {
            theRB2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, theRB2D.velocity.y);
            //transform.rotation = Quaternion.AngleAxis(direction * 180, Vector3.up);
        }
    }

    void Jump()
    {
        if (IsGrounded() && !Input.GetKey(KeyCode.Tab))
        {
            if (Input.GetKey(KeyCode.Space) && chargeTime <= chargeMax)
            {
                chargeTime += Time.fixedDeltaTime * chargeSpeed;
                theRB2D.velocity = new Vector2(0, theRB2D.velocity.y);
                animator.SetBool("isCharging", true);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //canMove = false;
                theRB2D.velocity = new Vector2(0, theRB2D.velocity.y);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                theRB2D.velocity += new Vector2(horJumpForce * chargeTime * direction, verJumpForce * chargeTime);
                chargeTime = 1f;
                animator.SetBool("isCharging", false);
            }
        }
    }

    public bool IsGrounded()
    {
        float extraHeightText = .1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        Color rayColor;
        if(raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y), Vector2.right * (boxCollider2d.bounds.extents.x), rayColor);
        return raycastHit.collider != null;
    }

    /*
    private void Bounce()
    {
        float extraHeightText = .1f;
        RaycastHit2D leftRaycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.left, extraHeightText, platformLayerMask);
        if (leftRaycastHit.collider != null)
        {
            Debug.Log("hit left");
            //physicsMaterial.bounciness = 1;
        }
        else
        {
            //physicsMaterial.bounciness = 0;
        }
        RaycastHit2D rightRaycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.right, extraHeightText, platformLayerMask);
        if(rightRaycastHit.collider != null)
        {
            Debug.Log("hit right");
            //physicsMaterial.bounciness = 1;
        }
        else
        {
            //physicsMaterial.bounciness = 0;
        }
    }/*

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        theRB2D.velocity = new Vector2(-theRB2D.velocity.x, theRB2D.velocity.y);
        //theRB2D.AddForce(transform.right * chargeTime, ForceMode2D.Impulse);
        Debug.Log("yep");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("nope");
    }*/
}
