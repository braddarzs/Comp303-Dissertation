using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GenerateRoom roomGenerator;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Exit":
                roomGenerator.RoomCompleted(false);
                break;
            case "Loot":
                collision.GetComponent<Chest>().OpenChest();
                break;
            default:
                break;
        }
    }




}




