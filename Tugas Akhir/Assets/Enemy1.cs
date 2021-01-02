using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public SpriteRenderer sr;

    public int maxHealth = 100;
    int currentHealth;

    public float attackRange = 0.5f;
    public int attackDelay = 2;
    float lastAttacked = -9999;
    public LayerMask playerLayers;

    //materials
    private Material matWhite;
    private Material matDefault;

    Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;

        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Attack();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        sr.material = matWhite;
        if (currentHealth <= 0)
        {
            Invoke("ResetMaterial", .03f);
            Die();
        }
        else
        {
            KnockBack();
            Invoke("ResetMaterial", .1f);
        }
    }

    private void KnockBack()
    {
        // Update position
        if (transform.position.x < player.position.x)
        {
            rigidbody.AddForce(transform.right * -300);
        }
        else
        {
            rigidbody.AddForce(transform.right * 300);
        }

    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);
        //animator.SetBool("PlayerApproach",false);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 2);
    }

    void Attack()
    {
        //Detect player
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayers);
        //Damage him
        foreach (Collider2D player in hitPlayer)
        {
            if (Time.time > lastAttacked + attackDelay)
            {
                animator.SetBool("Attack", true);
                lastAttacked = Time.time;
            }
        }
    }

}

