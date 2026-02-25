using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BasicPlayModeTests
{
    [UnityTest]
    public IEnumerator GameObject_CanBeCreated()
    {
        GameObject go = new GameObject("TestObject");

        Assert.IsNotNull(go);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Transform_Position_CanBeSet()
    {
        GameObject go = new GameObject("Mover");
        go.transform.position = Vector3.one;

        yield return null;

        Assert.AreEqual(Vector3.one, go.transform.position);
    }
}
