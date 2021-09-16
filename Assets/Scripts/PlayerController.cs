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
    private BoxCollider2D boxCollider2d;

    private Rigidbody2D theRB2D;

    public bool canMove;
    public bool isGrounded;

    public int direction;

    [SerializeField] private LayerMask platformLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        theRB2D = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0  && IsGrounded())
        {
            direction = -1;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0 && IsGrounded())
        {
            direction = 0;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && IsGrounded())
        {
            direction = 1;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        if (canMove && IsGrounded())
        {
            theRB2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, theRB2D.velocity.y);
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            if (Input.GetKey(KeyCode.Space) && chargeTime <= chargeMax)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
                theRB2D.velocity = new Vector2(0, theRB2D.velocity.y);
                canMove = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //canMove = false;
                theRB2D.velocity = new Vector2(0, theRB2D.velocity.y);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                theRB2D.velocity += new Vector2(horJumpForce * chargeTime * direction, verJumpForce * chargeTime);
                chargeTime = .25f;
                canMove = true;
            }
        }
    }

    private bool IsGrounded()
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
        Debug.DrawRay(boxCollider2d.bounds.center, Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText));
        return raycastHit.collider != null;
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //theRB2D.velocity = new Vector2(-theRB2D.velocity.x, theRB2D.velocity.y);
        theRB2D.AddForce(transform.right * chargeTime, ForceMode2D.Impulse);
        Debug.Log("yep");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("nope");
    }*/
}
