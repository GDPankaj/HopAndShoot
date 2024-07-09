using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Attributes")]
    [SerializeField] float _movementSpeed = 7.5f;
    [SerializeField] float _jumpForce = 10f;
    [SerializeField][Range(0f, 100f)] float _maxFallSpeed = 40f;
    [SerializeField][Range(0f,.5f)] float _variableJumpTime = .35f;
    [SerializeField][Range(1f, 3f)] float _increasedGravityFactor = 1.25f;
    [SerializeField][Range(.05f, .15f)] float _jumpBufferTime = .05f;
    [SerializeField][Range(.05f, .15f)] float _coyoteTime = .08f;


    [Header("Miscelleneous Attributes")]
    [SerializeField] Vector2 _castBox = new Vector2(.7f, .15f);
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _feetPos;
    [SerializeField] Tilemap _bgTileMap;
    Rigidbody2D _playerRb;
    SpriteRenderer _playerSR;
    bool _isGrounded;
    bool _isJumping;
    float _inputFactor;
    float _variableJumpTimer;
    float _originalGravity;
    float _increasedGravity;
    float _jumpBufferTimeCounter;
    float _coyoteTimeCounter;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _isGrounded = true;
        _originalGravity = _playerRb.gravityScale;
        _increasedGravity = _playerRb.gravityScale * _increasedGravityFactor;
        _playerSR = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Move(_inputFactor);
        ClampMovement();
    }

    private void OnDrawGizmos()
    {
        if (_feetPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_feetPos.position, _castBox);
        }
    }

    void Update()
    {
        _isGrounded = Physics2D.OverlapBox(_feetPos.position, _castBox, 0f, _groundLayer);
        _inputFactor = Input.GetAxisRaw("Horizontal");
        Jump();
    }

    void Jump()
    {
        if(_isGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpBufferTimeCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferTimeCounter -= Time.deltaTime;
        }
        if (_coyoteTimeCounter>0f && _jumpBufferTimeCounter>0f)
        {
            _playerRb.velocity = Vector2.up * _jumpForce;
            _isJumping = true;
            _variableJumpTimer = _variableJumpTime;
            _jumpBufferTimeCounter = 0f;
            AudioManager.instance?.PlaySFX("Jump");
        }
        if(Input.GetKey(KeyCode.Space) && _isJumping)
        {
            if (_variableJumpTimer > 0)
            {
                _playerRb.velocity = Vector2.up * _jumpForce;
                _variableJumpTimer -= Time.deltaTime;
            }else
            {
            _isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isJumping = false;
            _coyoteTimeCounter = 0f;
        }
        if (_playerRb.velocity.y < 0f)
        {
            _playerRb.gravityScale = _increasedGravity;
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, Mathf.Max(_playerRb.velocity.y, -_maxFallSpeed));
        }
        else
        {
            _playerRb.gravityScale = _originalGravity;
        }

    }

    void Move(float inputFactor)
    {
        _playerRb.velocity = new Vector2(inputFactor * _movementSpeed, _playerRb.velocity.y);
    }

    public float GetInputFactor()
    {
        return _inputFactor;
    }

   public bool IsJumpingAndNotGrounded()
   {
        if (_isJumping && !_isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
   }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void ClampMovement()
    {
        float _halfPlayerWidth = _playerSR.size.x / 2f;
        float _halfplayerHeight = _playerSR.size.y / 2f;

        //Get PlayerPosition after it has been changed
        Vector3 playerPosition = transform.position;

        //BoundsInt is similar to Bounds 
        //but as it uses integer instead of floating point like bound it is usefull in grid based system like tilemap
        //CellBounds is property of tilemap that return value of minimum and maximum location of cell in particular tilemap on a grid
        BoundsInt tilemapBounds = _bgTileMap.cellBounds;
        //we convert the cell location to world location
        Vector3 minTilemapPosition = _bgTileMap.CellToWorld(tilemapBounds.min);
        Vector3 maxTilemapPosition = _bgTileMap.CellToWorld(tilemapBounds.max);

        playerPosition.x = Mathf.Clamp(playerPosition.x, minTilemapPosition.x + _halfPlayerWidth, maxTilemapPosition.x - _halfPlayerWidth);
        playerPosition.y = Mathf.Clamp(playerPosition.y, minTilemapPosition.y + _halfplayerHeight, maxTilemapPosition.y - _halfPlayerWidth);

        transform.position = playerPosition;
    }
}
