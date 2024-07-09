using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateManager : MonoBehaviour
{
    public UnityEvent BulletShot;
    
    PlayerBaseState _currentState;
    PlayerDefaultState _playerDefaultState = new PlayerDefaultState();
    PlayerWithGunState _playerWithGunState = new PlayerWithGunState();
    Animator _animator;
    PlayerMovement _playerMovement;
    [SerializeField]Sprite _defaultSprite;
    [SerializeField]Sprite _withGunSprite;
    [SerializeField] GameObject _negativeSparkObject;
    [SerializeField] GameObject _positiveSparkObject;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _negativeSpawnPos;
    [SerializeField] Transform _positiveSpawnPos;
    BulletOjectPool _BulletOjectPool;
    GameObject _sparkObject;
    const string _idleAnimation = "Idle";
    const string _moveAnimation = "Movement";
    const string _jumpAnimation = "Jump";
    const string _idleWithGunAnimation = "IdleWithGun";
    const string _moveWithGunAnimation = "MovementWithGun";
    const string _jumpWithGunAnimation = "JumpWithGun";
    const string _gunShootAnimation = "GunShoot";
    bool _isShooting = false;
    bool _flipBulletSprite;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentState = _playerDefaultState;
        _currentState.EnterState(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _BulletOjectPool = FindObjectOfType<BulletOjectPool>();
        _sparkObject = _positiveSparkObject;
        if(_sparkObject.activeInHierarchy)
        {
            _sparkObject.SetActive(false);
        }
    }

    void Update()
    {
        _currentState.UpdateState(this);
        if (_playerMovement.GetInputFactor() < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if (!IsShooting()) { _sparkObject = _negativeSparkObject; }
            _flipBulletSprite = true;
        }
        else if(_playerMovement.GetInputFactor() > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            if (!IsShooting()) { _sparkObject = _positiveSparkObject; }
            _flipBulletSprite = false;
        }
    }

    public void SwitchState(PlayerBaseState newState)
    {
        _currentState.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gun"))
        {
            SwitchState(_playerWithGunState);
            AudioManager.instance?.PlaySFX("GotAGun");
            other.gameObject.SetActive(false);
        }
    }

    public void Shoot()
    {
        if(!IsShooting())
        {
            StartCoroutine(HandleShooting());
            BulletShot.Invoke();
        }
    }

    IEnumerator HandleShooting()
    {
        _isShooting = true;
        PlayAnimation(_gunShootAnimation);
        _sparkObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);

        GameObject thisBullet = _BulletOjectPool.GetInactiveBulletFromPool();
        if (thisBullet != null)
        {
            Transform spawnPos = _flipBulletSprite ? _negativeSpawnPos : _positiveSpawnPos;
            Vector2 direction = _flipBulletSprite ? Vector2.left : Vector2.right;

            thisBullet.transform.position = spawnPos.position;
            thisBullet.GetComponent<Bullet>().SetDirection(direction);
            thisBullet.GetComponent<Bullet>().FlipSprite(_flipBulletSprite);

            thisBullet.SetActive(true);
            AudioManager.instance.PlaySFX("Shoot");

        }

        _sparkObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _isShooting = false;
    }
    public void PlayAnimation(string animation)
    {
        _animator.Play(animation);
    }

    public Sprite GetDefaultSprite()
    {
        return _defaultSprite;
    }
    public Sprite GetWithGunSprite()
    {
        return _withGunSprite;
    }

    public string GetIdleWithGun()
    {
        return _idleWithGunAnimation;
    }
    public string GetMoveWithGun() 
    {
        return _moveWithGunAnimation;
    }
    public string GetJumpWithGun()
    {
        return _jumpWithGunAnimation;
    }

    public string GetIdle()
    {
        return _idleAnimation;
    }

    public string GetMove()
    {
        return _moveAnimation;
    }

    public string GetJump()
    {
        return _jumpAnimation;
    }

    public string GetGunShoot()
    {
        return _gunShootAnimation;
    }

    public bool IsShooting()
    {
        return _isShooting;
    }
}
