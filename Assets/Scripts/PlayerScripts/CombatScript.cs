using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public Animator animator;
    private bool grounded;
    [SerializeField] PlayerScript player_script;
    public Transform attack_point;
    public float attack_range = 0.5f;
    public LayerMask enemy_layers;
    public int attack_damage = 30;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        attack();
    }

    private void attack() {
        grounded = player_script.IsGrounded();
        if(grounded && Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("sword_attack");
            Collider2D[] hit_enemies = Physics2D.OverlapCircleAll(attack_point.position, attack_range, enemy_layers);
            foreach (var enemy in hit_enemies ) {
                enemy.GetComponent<FootmanScript>().takeDamage(40);
            }
        }
    }

    private void OnDrawGizmos() {
        if(attack_point == null) 
            return;
        Gizmos.DrawWireSphere(attack_point.position, attack_range);
    }
}
