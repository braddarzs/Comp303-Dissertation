using UnityEngine;

using NUnit.Framework;

public class BasicEditModeTests
{
    [Test]
    public void True_IsTrue()
    {
        Assert.IsTrue(true);
    }

    [Test]
    public void Math_WorksCorrectly()
    {
        int a = 2 + 2;
        Assert.AreEqual(4, a);
    }

    [Test]
    public void Strings_AreEqual()
    {
        string expected = "hello";
        string actual = "he" + "llo";

        Assert.AreEqual(expected, actual);
    }
}
