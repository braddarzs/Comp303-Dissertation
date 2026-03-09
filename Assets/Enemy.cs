using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField] MMF_Player killFeedback;
    [SerializeField] float speed = 3f;

    [SerializeField] float attackRadius = 2f;
    [SerializeField] float attackCooldown = 2f;
    private bool canAttack = true;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] MMF_Player attackFeedback;

    GameObject player;

    public void KillEnemy()
    {
        killFeedback.PlayFeedbacks();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(AttackCooldown());
    }

    void Update()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.transform.position,
            speed * Time.deltaTime
        );
    }

    private void Attack()
    {
        if (!canAttack) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRadius,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            TopDownPlayer topDownPlayer = hit.GetComponent<TopDownPlayer>();
            if (topDownPlayer != null)
            {
                topDownPlayer.KillPlayer();
            }
        }

        attackFeedback.PlayFeedbacks();
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);
            Attack();
        }
    }

}