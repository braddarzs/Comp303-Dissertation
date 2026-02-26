using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTest
{
    private GameObject playerGO;
    private Rigidbody2D rb;
    private TopDownPlayer player;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        var prefab = Resources.Load<GameObject>("Player");
        Assert.IsNotNull(prefab, "Player prefab not found in Resources");

        playerGO = Object.Instantiate(prefab);

        player = playerGO.GetComponent<TopDownPlayer>();
        Assert.IsNotNull(player, "TopDownPlayer component missing on prefab");

        rb = playerGO.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb, "Rigidbody2D missing on prefab");

        rb.gravityScale = 0f;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(playerGO);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Player_Moves_Up()
    {
        yield return AssertMoves(Vector2.up, p => p.y > 0f, "up");
    }

    [UnityTest]
    public IEnumerator Player_Moves_Down()
    {
        yield return AssertMoves(Vector2.down, p => p.y < 0f, "down");
    }

    [UnityTest]
    public IEnumerator Player_Moves_Left()
    {
        yield return AssertMoves(Vector2.left, p => p.x < 0f, "left");
    }

    [UnityTest]
    public IEnumerator Player_Moves_Right()
    {
        yield return AssertMoves(Vector2.right, p => p.x > 0f, "right");
    }

    private IEnumerator AssertMoves(
        Vector2 direction,
        System.Func<Vector2, bool> condition,
        string label
    )
    {
        Vector2 startPos = rb.position;

        player.SetMoveDirection(direction);

        yield return new WaitForFixedUpdate();

        Vector2 delta = rb.position - startPos;

        Assert.IsTrue(
            condition(delta),
            $"Player did not move {label}. Delta was {delta}"
        );
    }
}