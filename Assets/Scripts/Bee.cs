using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bee : MonoBehaviour, IEnemy
{
    [SerializeField] float speed = 10f;
    [SerializeField] Transform pointsParent;
    Transform[] points;
    int _pointIndex = 0;
    int _pointCount;
    SpriteRenderer _beeSRender;


    private void Start()
    {
        _beeSRender = GetComponent<SpriteRenderer>();
        points = new Transform[pointsParent.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = pointsParent.GetChild(i);
        }

        _pointCount = points.Length;
        _pointIndex = 0;
        
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollided(collision);
    }
    public void Move()
    {
        var step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, points[_pointIndex].position, step);

        if(transform.position == points[_pointIndex].position)
        {
            FlipBeeSprite();
            Debug.Log("reached");
            NextPoint();
        }
    }

    public void OnCollided(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if(collision.gameObject.CompareTag("Bullet"))
        {
            AudioManager.instance?.PlaySFX("EnemyImpact");
            Destroy(gameObject);
        }
    }

    void NextPoint()
    {
        if(_pointIndex ==  points.Length - 1)
        {
            _pointIndex = 0;
        }
        else
        {
            _pointIndex++;
        }
    }

    void FlipBeeSprite()
    {
        _beeSRender.flipX = !_beeSRender.flipX;
    }
}
