using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GenerateRoomTest
{
    private GenerateRoom generator;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        var go = Object.Instantiate(
            Resources.Load<GameObject>("GenerateRoom_TestRig")
        );

        generator = go.GetComponent<GenerateRoom>();

        yield return null;
    }

    [UnityTest]
    public IEnumerator StartGeneration_Creates_Previews()
    {
        generator.StartGeneration();

        yield return null;

        var previews = GameObject.FindObjectsByType<PreviewData>(FindObjectsSortMode.None);

        Assert.Greater(previews.Length, 0, "No previews were generated");
    }
}