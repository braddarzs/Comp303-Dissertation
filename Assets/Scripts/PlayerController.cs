using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    [SerializeField] InputActionReference moveInput;

    private void OnEnable()
    {
        moveInput.action.performed += OnMove;
        moveInput.action.canceled += OnMove;
    }

    private void OnDisable()
    {
        moveInput.action.performed -= OnMove;
        moveInput.action.canceled += OnMove;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}