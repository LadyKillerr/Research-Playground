using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    private const string _IDLE = "Idle";
    private const string _WALKING = "Run";
    private const string _JUMPING = "Jump";
    private const string _JUMPING2 = "Jump2";

    [Header("Animation Controller")]
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] AnimationReferenceAsset idle, walking, jumping, jumping2, attacking;
    [SerializeField] string currentAnimState;

    [Header("Player Input Config")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool isIdling;

    Vector2 inputVector;
    [SerializeField] bool isWalking;
    [SerializeField] bool isJumping;


    [SerializeField] float walkSpeed = 100f;
    //[SerializeField] float sprintSpeed = 200f;
    //float currentSpeed;

    [SerializeField] float jumpForce = 50f;
    [SerializeField] float jumpDelay = 0.33f;
    [SerializeField] int jumpCount = 2;
    [SerializeField] int currentJumpCount;

    Rigidbody2D playerRigidbody;
    CapsuleCollider2D capsuleCollider2D;

    void Awake()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        playerRigidbody = GetComponent<Rigidbody2D>();


    }

    void Start()
    {

        currentAnimState = _IDLE;
        SetCharacterState(currentAnimState);

    }

    IEnumerator DisablePortalState(float delay)
    {
        yield return new WaitForSeconds(delay);

        currentAnimState = _IDLE;
        SetCharacterState(currentAnimState);

    }

    void FixedUpdate()
    {
        CharacterMovement();
        FlipSprite();

    }

    private void Update()
    {
        CheckIsGrounded();
        ToggleIdleState();
    }

    #region CharacterMovement
    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();

        //if (inputVector != Vector2.zero)
        //{
        //    isWalking = true;
        //}
        //else
        //{
        //    isWalking = false;
        //}
    }

    //private void OnSprint(InputValue value)
    //{
    //    if (value.isPressed)
    //    {
    //        currentSpeed = sprintSpeed;
    //        isRunning = true;
    //    }
    //    else
    //    {
    //        currentSpeed = walkSpeed;
    //        isRunning = false;
    //    }
    //}

    private void OnJump(InputValue value)
    {
        if (value.isPressed && currentJumpCount > Mathf.Epsilon)
        {
            isJumping = true;


            if (currentJumpCount == 2)
            {
                SetAnimState(_JUMPING);
            }
            else if (currentJumpCount == 1)
            {
                SetAnimState(_JUMPING2);
            }

            playerRigidbody.AddForce(new Vector2(playerRigidbody.velocity.x, jumpForce));

            currentJumpCount--;


        }

    }



    void CharacterMovement()
    {
        playerRigidbody.velocity = new Vector2(inputVector.x * walkSpeed * Time.deltaTime, playerRigidbody.velocity.y);

        if (playerRigidbody.velocity.x != 0 && isGrounded)
        {
            isWalking = true;
        }
        else { isWalking = false; }

        if (isGrounded && isWalking)
        {
            if (currentAnimState != _WALKING)
            {
                SetAnimState(_WALKING);
            }
        }

    }

    void FlipSprite()
    {
        // Biến check xem có đang di chuyển hay không, vận tốc -1 hay 1 thì vẫn là movement nên phải dùng Abs,
        bool playerIsMovingHorizontal = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

        // Nếu velocity = 0 thì Mathf.Sign cũng trả về +1, từ đó flip sprite lại bên phải
        // Vì vậy nên phải tạo biến bool
        if (playerIsMovingHorizontal)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
    }

    private void CheckIsGrounded()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            currentJumpCount = jumpCount;
            isGrounded = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
            isJumping = true;
        }

    }

    private void ToggleIdleState()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && !isWalking && !isJumping
             && isGrounded)
        {
            if (!isIdling)
            {
                SetAnimState(_IDLE);
                isIdling = true;
            }
        }
        else
        {
            isIdling = false;
        }
    }

    #endregion

    #region CharacterAnimation
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void SetCharacterState(string state)
    {

        if (state.Equals(_IDLE))
        {
            SetAnimation(idle, true, 1);
        }
        else if (state.Equals(_WALKING))
        {
            SetAnimation(walking, true, 1);
        }
        else if (state.Equals(_JUMPING))
        {
            SetAnimation(jumping, false, 1);
        }
        else if (state.Equals(_JUMPING2))
        {
            SetAnimation(jumping2, false, 1);
        }

    }

    IEnumerator ToggleDelayTime(float delay)
    {
        yield return new WaitForSeconds(delay);

    }

    public void SetAnimState(string state)
    {
        currentAnimState = state;
        SetCharacterState(currentAnimState);
    }

    #endregion
}
