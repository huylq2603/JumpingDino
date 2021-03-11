using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform tf;
    public Animator animator;

    public float jumpForce;
    public float speed;
    private float moveInput;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    private GameObject interactableObject;

    private bool check = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        interactableObject = null;
    }

    private void FixedUpdate() {
        //move
        if (GameController.isInputEnable) {
            moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            animator.SetFloat("speed", Mathf.Abs(moveInput * speed));
            //change sprite direction
            if (moveInput > 0) {
                tf.localScale = new Vector2(1, tf.localScale.y);
            }
            if (moveInput < 0) {
                tf.localScale = new Vector2(-1, tf.localScale.y);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //check ground
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        //jump
        if (rb.velocity.y < 0) {
            check = true;
        }
        if (check) {
            if (rb.velocity.y == 0) {
                animator.SetBool("isJumping", false);
                check = false;
            }
        }
        if (GameController.isInputEnable && isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = Vector2.up * jumpForce;
            animator.SetBool("isJumping", true);
        }

        if (interactableObject != null) {
            InteractableAlert scriptInstance = interactableObject.GetComponent<InteractableAlert>();
            if (GameController.isInputEnable && scriptInstance.canInteract && Input.GetKeyDown(KeyCode.F)) {
                interactableObject.GetComponent<Animator>().Play("InteractableAnim");
                scriptInstance.DestroyGuideText();
                StartCoroutine(scriptInstance.DoInteraction());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag)
        {
            case "Edibles": 
                Destroy(other.gameObject);
                GameController.Instance.decreaseCarrot();
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

