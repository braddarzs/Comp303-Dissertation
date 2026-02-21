using UnityEngine;

public class RoomDNA
{

    public int roomWidth;
    public int roomLength;

    public int enemyCount;
    public int lootCount;

    public int fitness = 1;
    public GameObject room;

    public RoomDNA()
    {
        roomLength = Random.Range(5, 25);
        roomWidth = Random.Range(5, 25);
        enemyCount = Random.Range(1, 10);
        lootCount = Random.Range(1, 10);
    }

    public RoomDNA Crossover(RoomDNA other)
    {
        return new RoomDNA
        {
            roomWidth = CrossoverLogic(roomWidth, other.roomWidth),
            roomLength = CrossoverLogic(roomLength, other.roomLength),
            enemyCount = CrossoverLogic(enemyCount, other.enemyCount),
            lootCount = CrossoverLogic(lootCount, other.lootCount)
        };
    }

    private int CrossoverLogic(int a, int b) => Random.value < 0.5f ? a : b;

    public void Mutate(float rate)
    {
        if (Random.value < rate) roomWidth = Random.Range(5, 25);
        if (Random.value < rate) roomLength = Random.Range(5, 25);
        if (Random.value < rate) enemyCount = Random.Range(1, 10);
        if (Random.value < rate) lootCount = Random.Range(1, 10);
    } 
}
