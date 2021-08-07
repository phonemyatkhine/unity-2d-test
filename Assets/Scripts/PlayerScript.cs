using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float side_force = 4f;
    public float upward_force = 4f;
    private int jump_count = 0;
    private Rigidbody2D player;
    public Animator animator;
    private bool facing_right;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }
    
    void Update() 
    {

    }
    
    //Fixed Update is called for physics engine specs
    void FixedUpdate()
    {
        var movement = Input.GetAxisRaw("Horizontal"); //get raw x input 
        transform.position = transform.position + new Vector3(movement * side_force * Time.deltaTime, 0, 0); //movement

        animator.SetFloat("speed", Mathf.Abs(movement)); //set animator variable

        if(Mathf.Abs(player.velocity.y) < 0.01f) { //grounded 
            animator.SetBool("is_jumping", false); //set animator variable
            jump_count = 0;
            //allows run (shift only on ground)
            if(Input.GetButtonDown("Fire3")) {
                side_force *=2;
            } else if (Input.GetButtonUp("Fire3")) {
                side_force /=2;
            }

        } else if (Mathf.Abs(player.velocity.y) > 0.01f) { //in air

        }

        //jumping 
        if(Input.GetButtonDown("Jump") && (jump_count < 2)) {
            animator.SetBool("is_jumping", true); //set animator variable
            player.AddForce(new Vector2(0 ,upward_force), ForceMode2D.Impulse);
            jump_count +=1;
        }
        //flip the sprite
        if(movement < 0 && !facing_right) {
            Flip();
        } else if(movement > 0 && facing_right) {
            Flip();
        }
    }

    void Flip() 
    {
        facing_right = !facing_right;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
