using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayer : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;
    
    [Header("Vertical Movement")]
    public float jumpSpeed = 5f;
    public float jumDelay = 0.25f;
    public float jumpTimer;
    public float distance;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;
    public LayerMask ladderLayer;
    public History HistScript;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 1.1f;
    public Vector3 colliderOffset;
    public bool isClimbing;
    public bool isCollidingEvent1;
    public bool isCollidingEvent2;


    // Update is called once per frame
    void Update() {
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        
        if (Input.GetButtonDown("Jump")) {
            jumpTimer = Time.time + jumDelay;
        }
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    void FixedUpdate() {
        moveCharacter(direction.x);
        if (jumpTimer > Time.time && onGround) {
            Jump();
        }
        /* SCALE */
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, ladderLayer);
        if (hitInfo.collider != null) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                isClimbing = true;
            }
        } else  {
            isClimbing = false;
        }
        if (isClimbing) {
            float inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * maxSpeed);
            rb.gravityScale = 0;
        }
        /* ZERO GRAVITY */
        if (HistScript.turnOffGravity) {
            float inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * maxSpeed);
            rb.gravityScale = 0;
        }
        modifyPhysics();
    }
    /* EVENTS */
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Event1") {
            isCollidingEvent1 = true;
        } else {
            isCollidingEvent1 = false;
        }
        if (col.gameObject.tag == "Event2") {
            isCollidingEvent2 = true;
        } else {
            isCollidingEvent2 = false;
        }
        if (col.gameObject.tag == "EventLoop") {
            transform.position = new Vector3(transform.position.x + 75, transform.position.y, transform.position.z);
        }
        if (col.gameObject.tag == "EventLoop2") {
            transform.position = new Vector3(transform.position.x - 75, transform.position.y, transform.position.z);
        }
    }
    void moveCharacter(float horizontal) {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        animator.SetFloat("vertical", rb.velocity.y);
        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
    }
    
    void modifyPhysics(){
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround) {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections) {
                rb.drag = linearDrag;
            } else {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        } else {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0) {
                rb.gravityScale = gravity * fallMultiplier;
            } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {  //planete avec moins de gravité
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }

    }
    void Flip() {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180,0);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}
