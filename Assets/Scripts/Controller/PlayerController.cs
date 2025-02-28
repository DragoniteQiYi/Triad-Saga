using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField][Range(0, 10f)]
    private float movementSpeed = 2f;

    private Vector2 moveInput;

    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        HandleRotation();
    }

    private void Move()
    {
        Vector3 move = movementSpeed * Time.deltaTime * (Vector3)moveInput;
        transform.Translate(move, Space.World);

        playerAnimator.SetBool("IsWalking", moveInput != Vector2.zero);
    }

    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>().normalized;
    }

    private void HandleRotation()
    {
        if (moveInput != Vector2.zero)
        {
            playerAnimator.SetFloat("FaceX", moveInput.x);
            playerAnimator.SetFloat("FaceY", moveInput.y);
        }
    }
}
