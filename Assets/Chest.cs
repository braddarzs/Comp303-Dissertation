using MoreMountains.Feedbacks;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] MMF_Player collectFeedback;

    public void OpenChest()
    {
        collectFeedback.PlayFeedbacks();
    }
}
