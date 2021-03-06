using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform tf;

    public float jumpForce;
    public float speed;
    private float moveInput;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public GameObject interactableObject;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        interactableObject = null;
    }

    private void FixedUpdate() {
        //move
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        //change sprite direction
        if (moveInput > 0) {
            tf.localScale = new Vector2(1, tf.localScale.y);
        }
        if (moveInput < 0) {
            tf.localScale = new Vector2(-1, tf.localScale.y);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //check ground
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        //jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = Vector2.up * jumpForce;
        }

        if (interactableObject != null && Input.GetKeyDown(KeyCode.F)) {
            interactableObject.GetComponent<Animator>().Play("DoorAnim");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag)
        {
            case "Edibles": 
                Destroy(other.gameObject);
                break;
            case "Interactables":
                interactableObject = other.gameObject;
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        switch (other.gameObject.tag)
        {
            case "Interactables":
                interactableObject = null;
                break;
            default:
                break;
        }
    }
}

