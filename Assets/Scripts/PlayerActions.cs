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
                Destroy(collision.gameObject);
                break;
            case "Enemy":
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }




}




