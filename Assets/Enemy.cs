using MoreMountains.Feedbacks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] MMF_Player killFeedback;

    public void KillEnemy()
    {
        killFeedback.PlayFeedbacks();
    }
}
