using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mouse : MonoBehaviour, IEnemy
{
    [SerializeField] float _speed  = 10f;
    SpriteRenderer _mouseSR;
    [SerializeField] Transform negativefacePos;
    [SerializeField] Transform positivefacePos;
    [SerializeField] LayerMask groundLayer;
    private void Start()
    {
        _mouseSR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        CheckWall();
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollided(collision);
    }
    public void Move()
    {
        Vector2 moveDirection = Vector2.left;
        if(_mouseSR.flipX == false)
        {
            moveDirection = Vector2.right;
        }
        else if(_mouseSR.flipX == true)
        {
            moveDirection = Vector2.left;
        }

        transform.Translate(moveDirection * _speed * Time.deltaTime);
    }

    public void OnCollided(Collision2D col)
    {
        Debug.Log("Collided with: " + col.gameObject.name);
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerMovement>().RestartLevel();
        }
        else if (col.gameObject.CompareTag("Bullet"))
        {
            AudioManager.instance?.PlaySFX("EnemyImpact");
            Destroy(gameObject);
        }
    }

    void CheckWall()
    {
        bool isWall = false;
        if(_mouseSR.flipX == true)
        {
            isWall = Physics2D.OverlapCircle(negativefacePos.position, .2f, groundLayer);
            
        }
        else 
        {
            isWall = Physics2D.OverlapCircle(positivefacePos.position, .2f, groundLayer);
        }

        if(isWall)
        {
            _mouseSR.flipX = !_mouseSR.flipX;
            isWall = false;
        }
        
    }
}
