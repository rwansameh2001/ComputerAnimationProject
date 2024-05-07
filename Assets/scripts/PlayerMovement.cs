using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpHight;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpCoolDown;
    private float horizontalInput;

    private void Awake()
    {
        //Grabs references for rigidbody and animator from game object.
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");


        //Flip player when facing left/right.
        if (horizontalInput > 0.01f)
            transform.localScale = Vector2.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-1, 1);


        //sets animation parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //wallJumpLogic
        if (wallJumpCoolDown > 0.2f)
        {

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);


            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 2.5f;

            if (Input.GetKey(KeyCode.Space))
                Jump();

        }
        else
            wallJumpCoolDown += Time.deltaTime;

    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpHight);
            anim.SetTrigger("jump");

        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                int PushFromWallPower = 10;
                int wallJumpPower = 0;
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * PushFromWallPower, wallJumpPower);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                int PushFromWallPower = 3;
                int wallJumpPower = 6;
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * PushFromWallPower, wallJumpPower);
            }
            wallJumpCoolDown = 0;



        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}







