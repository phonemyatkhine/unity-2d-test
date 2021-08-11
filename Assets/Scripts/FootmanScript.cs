using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootmanScript : MonoBehaviour
{
    public Animator animator;

    public int max_health = 100;
    private int current_health;
    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void takeDamage(int damage) {
        current_health -= damage;
        animator.SetTrigger("attacked");
        if(current_health <= 0) {
            die();
        }
    }

    void die() {
        animator.SetBool("is_death", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        this.enabled = false;
        Debug.Log("Enemy died");
    }
}
