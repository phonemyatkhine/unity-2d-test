using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D box_collider;
    public float side_force = 4f;
    public float upward_force = 4f;
    private float max_force = 8f;
    private bool can_double_jump = true;
    private int max_jump_count = 2;
    private bool facing_right;
    private bool grounded;
    private Color ray_color;
    [SerializeField] private LayerMask layer_mask;
    
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        box_collider = GetComponent<BoxCollider2D>();    
    }
    //update is called every second of the script is active 
    void Update() {
        forceLimit(); //limit the side_force to halt speed abusing
        run(); //run reeee
        doubleJump();
        attack();
    }  
    //Fixed Update is called for physics engine specs 
    // XXXDown and XXXExit doesnt work well with fixed update causes the engine to skip input and keydowns 
    void FixedUpdate() {
        move();
    }

    //flip the sprite
    private void flip() {
        facing_right = !facing_right;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void flipCheck(float movement) {
        if(movement < 0 && !facing_right) {
            flip();
        } else if(movement > 0 && facing_right) {
            flip();
        }
    }
    
    //Grounded Check
    private bool IsGrounded() {
        RaycastHit2D ray_hit = Physics2D.Raycast(box_collider.bounds.center, Vector2.down, box_collider.bounds.extents.y + 0.05f, layer_mask);
        if (ray_hit.collider != null)
        {
            ray_color = Color.green;
        } else if (ray_hit.collider == null) {
            ray_color = Color.red;
        }
        Debug.DrawRay(box_collider.bounds.center, Vector2.down *(box_collider.bounds.extents.y + 0.05f) ,  ray_color);
        return ray_hit.collider != null;
    }

    //Detect collision with platforms
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            animator.SetBool("is_jumping", false);
        }
    }

    //move around
    private void move() {
        var movement = Input.GetAxisRaw("Horizontal"); //get raw x input 
        transform.position = transform.position + new Vector3(movement * side_force * Time.deltaTime, 0, 0); //movement
        animator.SetFloat("speed", Mathf.Abs(movement));
        flipCheck(movement);
    }
    
    //run around
    private void run() {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            side_force *= 1.5f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            side_force /= 1.5f;
        }
    }

    //limit force
    private void forceLimit() {
        if(side_force > max_force) {
            side_force = 4f;
        }
    }
   
    //jumping
    private void jump() {
        animator.SetBool("is_jumping", true);
        rigidbody2D.AddForce(new Vector2(0 ,upward_force), ForceMode2D.Impulse);    
    }

    private void doubleJump() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(IsGrounded()) {
                jump();
                can_double_jump = true;
            } else {
                if(can_double_jump) {
                    jump();
                    can_double_jump = false;
                }
            }
        }
    }
    
    private void attack() {
        if(IsGrounded() && Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("sword_attack");
        }
    }
  
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if (other.gameObject.tag == "Floor")
    //     {
    //         grounded = false;
    //     }
    // }


    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "Floor") 
    //     {
    //         grounded = false;
    //     }
    // }
    
}
