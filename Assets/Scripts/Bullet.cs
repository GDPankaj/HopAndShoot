using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] float _lifeTime = 4f;
    Vector2 _direction = Vector2.right;

    private void OnEnable()
    {
        StartCoroutine(DisableBullet(_lifeTime));
    }
    private void Update()
    {
        transform.Translate(_direction * _bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(DisableBullet(0f));
    }

    public void FlipSprite(bool flipSprite)
    {
        GetComponent<SpriteRenderer>().flipX = flipSprite;
    }

    public void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection;
    }

    IEnumerator DisableBullet(float secondsToDisable)
    {
        yield return new WaitForSeconds(secondsToDisable);
        gameObject.SetActive(false);
    }
}
