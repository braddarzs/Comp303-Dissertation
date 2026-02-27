using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GenerateRoom roomGenerator;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Exit":
                roomGenerator.RoomCompleted();
                break;
            case "Loot":
                collision.GetComponent<Chest>().OpenChest();
                break;
            case "Enemy":
                collision.GetComponent<Enemy>().KillEnemy();
                break;
            default:
                break;
        }
    }




}




