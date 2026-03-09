using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float attackCooldown = 2f;
    private bool canAttack = true;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] MMF_Player SwordFeedback;
    [SerializeField] GenerateRoom roomGenerator;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference attackInput;

    private void OnEnable()
    {
        moveInput.action.performed += OnMove;
        moveInput.action.canceled += OnMove;
        attackInput.action.performed += Attack;
    }

    private void OnDisable()
    {
        moveInput.action.performed -= OnMove;
        moveInput.action.canceled -= OnMove;
        attackInput.action.performed -= Attack;
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

    private void Attack(InputAction.CallbackContext ctx)
    {
        if (!canAttack) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.KillEnemy();
            }
        }

        SwordFeedback.PlayFeedbacks();
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void KillPlayer()
    {
        roomGenerator.RoomCompleted(true);
    }


#if UNITY_INCLUDE_TESTS
    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }
#endif
}