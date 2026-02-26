using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

public class GenerateRoomTest
{
    private GenerateRoom generator;
    private GameObject rig;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        var prefab = Resources.Load<GameObject>("GenerateRoom_TestRig");
        Assert.IsNotNull(prefab, "GenerateRoom_TestRig prefab not found in Resources");

        rig = Object.Instantiate(prefab);

        generator = rig.GetComponent<GenerateRoom>();
        Assert.IsNotNull(generator, "GenerateRoom component missing on TestRig");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(rig);
        yield return null;
    }

    [UnityTest]
    public IEnumerator FirstPreview_Can_Be_Selected_And_Generated()
    {
        generator.StartGeneration();
        yield return null;

        var previews = Object.FindObjectsByType<PreviewData>(FindObjectsSortMode.None);
        Assert.Greater(previews.Length, 0, "No previews were generated");

        PreviewData firstPreview = previews[0];
        Assert.IsNotNull(firstPreview.roomDNA, "Preview has no RoomDNA");

        generator.GenerateChosen(firstPreview.roomDNA);
        yield return null;

        Tilemap tilemap = Object.FindFirstObjectByType<Tilemap>();
        Assert.IsNotNull(tilemap, "Tilemap not found");

        int tileCount = tilemap.GetUsedTilesCount();
        Assert.Greater(tileCount, 0, "No tiles were generated for chosen room");
    }
}