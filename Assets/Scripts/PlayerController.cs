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

    public ParticleSystem dustMove;

    private GameObject interactableObject;

    private bool check = false;
    private bool isKnockbacked = false;

    private int playerLayer, groundLayer;
    public Collider2D groundCollider;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        interactableObject = null;
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
    }
    private bool freezeAction = false;

    private void FixedUpdate() {
        //move
        if (GameController.isInputEnable) {
            moveInput = Input.GetAxisRaw("Horizontal");
            //knockback and move was conflicting so we need to put movement in conditions
            if (isKnockbacked) {
                rb.AddForce(new Vector2(tf.localScale.x * -15, 10), ForceMode2D.Impulse);
                StartCoroutine(HitObstacles());
                isKnockbacked = false;
            }
            if(!freezeAction){
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            }
            //------------
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
        // isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        isGrounded = Physics2D.OverlapBox(feetPos.position, new Vector2(1.4f, checkRadius), 0, whatIsGround);

        //jump
        if (rb.velocity.y < 0) {
            check = true;
        }
        if (check) {
            if (isGrounded) {
                animator.SetBool("isJumping", false);
                check = false;
                DustMove();
            }
        }
        if (GameController.isInputEnable && isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            if (Input.GetKey(KeyCode.DownArrow)) {
                if (isGrounded && tf.position.y >= -4.5f) {
                    StartCoroutine(JumpOff());
                }
            } else {
                rb.velocity = Vector2.up * jumpForce;
                animator.SetBool("isJumping", true);
                DustMove();
            } 
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
            case "Obstacles":
                if (!freezeAction) {
                    isKnockbacked = true;
                    freezeAction = true;
                }
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

    private IEnumerator HitObstacles() {
        Renderer rd = GetComponent<Renderer>();
        rd.enabled = false;
        yield return new WaitForSeconds(0.1f);
        rd.enabled = true;
        yield return new WaitForSeconds(0.1f);
        rd.enabled = false;
        yield return new WaitForSeconds(0.1f);
        rd.enabled = true;
        yield return new WaitForSeconds(0.1f);
        rd.enabled = false;
        yield return new WaitForSeconds(0.1f);
        rd.enabled = true;
        freezeAction = false;
    }

    private IEnumerator JumpOff() {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Collider2D>().enabled = true;
    }
    
    private void DustMove(){
        dustMove.Play();
    }
}

