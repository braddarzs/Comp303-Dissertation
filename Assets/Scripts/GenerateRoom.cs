
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GenerateRoom : MonoBehaviour
{
    [Header("Genetic Algorithm Settings")]

    public bool randomGeneration;

    [SerializeField] private int populationSize = 5;
    [SerializeField] private float mutationRate = 0.1f;

    private List<RoomDNA> population = new List<RoomDNA>();
    private int currentGeneration = 0;
    private int maxGeneration = 3;
    private bool firstGeneration = true;

    private List<RoomDNA> currentCandidates = new List<RoomDNA>();
    private bool waitingForSelection = false;
    private int rankingCount = 0;

    [Header("Room Preview & Tilemap")]
    [SerializeField] private GameObject roomPreviewPrefab;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private GameObject blankQuad;

    private List<GameObject> previews = new List<GameObject>();
    [SerializeField] private Transform[] spawnPositions;

    [Header("UI")]
    [SerializeField] private Button continueButton;

    [Header("Main Room")]
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private Transform roomParent;


    public void StartGeneration()
    {
        for (int i = 0; i < populationSize; i++)
            population.Add(new RoomDNA());

        RunEvolution();
    }


    public void RunEvolution()
    {
        if (firstGeneration)
        {
            ChooseRoomsForPlayer();
            firstGeneration = false;
            return;
        }
        foreach(GameObject preview in previews)
        {
            Destroy(preview);
        }
        population = population.OrderByDescending(dna => EvaluateFitness(dna)).ToList();
        Debug.Log($"Generation {currentGeneration + 1} Best Fitness: {EvaluateFitness(population[0])}");

        List<RoomDNA> nextGen = new List<RoomDNA>();
        while (nextGen.Count < populationSize)
        {
            RoomDNA parent1 = RouletteWheelSelect(population);
            RoomDNA parent2 = RouletteWheelSelect(population);
            RoomDNA child = parent1.Crossover(parent2);
            child.Mutate(mutationRate);
            nextGen.Add(child);
        }

        population = nextGen;

        ChooseRoomsForPlayer();
    }

    private void ChooseRoomsForPlayer()
    {
        currentCandidates = population
            .OrderBy(x => Random.value)
            .Take(populationSize)
            .ToList();

        GenerateMultiple(currentCandidates);

        continueButton.gameObject.SetActive(true);
        waitingForSelection = true;
        rankingCount = 0;
    }

    public void ContinueGeneration()
    {
        currentGeneration++;
        if(currentGeneration >= maxGeneration)
        {
            RunEvolution();
            FinalSelection();
            return;
        }
        RunEvolution();
    }

    private void FinalSelection()
    {
        continueButton.gameObject.SetActive(false);
        foreach(GameObject preview in previews)
        {
            if(preview == null ) continue;
            preview.GetComponent<PreviewData>().selectButton.SetActive(true);
            preview.GetComponent<PreviewData>().slider.gameObject.SetActive(false);
        }
    }


    private RoomDNA RouletteWheelSelect(List<RoomDNA> pool)
    {
        // Sum all fitness values
        float totalFitness = 0f;
        foreach (var dna in pool)
            totalFitness += EvaluateFitness(dna);

        // Random point on the wheel
        float randomPoint = Random.value * totalFitness;

        // Walk through until we hit the random point
        float cumulative = 0f;
        foreach (var dna in pool)
        {
            cumulative += EvaluateFitness(dna);
            if (cumulative >= randomPoint)
                return dna;
        }

        // Fallback (should rarely happen)
        return pool[pool.Count - 1];
    }


    public void GenerateMultiple(List<RoomDNA> rooms)
    {

        foreach (Transform spawnPos in spawnPositions)
        {
            foreach (Transform child in spawnPos)
            {
                Destroy(child.gameObject);
            }
        }  

        for(int i = 0; i < 5; i++)
        {
            GenerateSingle(rooms[i], spawnPositions[i]);
        }

        if (randomGeneration)
        {
            FinalSelection();
        }
    }

    public void GenerateSingle(RoomDNA dna, Transform uiParent)
    {
        Texture2D tex = GenerateRoomTexture(dna);

        GameObject display = Instantiate(roomPreviewPrefab, uiParent);
        previews.Add(display);
        display.transform.localScale = Vector3.one;

        display.transform.localPosition = Vector3.zero;

        PreviewData previewData = display.GetComponent<PreviewData>();
        previewData.roomDNA = dna;
        previewData.rawImage.texture = tex;
        previewData.Init();

        float scale = 10f;
        previewData.rawImage.rectTransform.sizeDelta = new Vector2(tex.width * scale, tex.height * scale);
    }

    public Texture2D GenerateRoomTexture(RoomDNA dna)
    {
        int w = dna.roomWidth;
        int h = dna.roomLength;

        Texture2D tex = new Texture2D(w, h);
        tex.filterMode = FilterMode.Point;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                bool isBorder = x == 0 || x == w - 1 || y == 0 || y == h - 1;

                Color c = isBorder ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.5f, 0.5f, 0.5f);

                tex.SetPixel(x, y, c);
            }
        }

        tex.Apply();
        return tex;
    }

    public void GenerateChosen(RoomDNA dna)
    {
        print("Generating...");

        foreach (GameObject preview in previews)
            Destroy(preview);
        previews.Clear();

        tileMap.ClearAllTiles();

        int width = dna.roomWidth;
        int height = dna.roomLength;

        List<Vector3> floorPositions = new List<Vector3>();
        List<Vector3> borderPositions = new List<Vector3>();
        Vector3 cellSize = tileMap.layoutGrid.cellSize;
        Vector3 tilemapOffset = tileMap.transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isBorder = x == 0 || x == width - 1 ||
                                y == 0 || y == height - 1;

                tileMap.SetTile(new Vector3Int(x, y, 0), isBorder ? wallTile : floorTile);

                if (!isBorder)
                {
                    floorPositions.Add(new Vector3(
                        x * cellSize.x + cellSize.x * 0.5f,
                        y * cellSize.y + cellSize.y * 0.5f,
                        0
                    ) + tilemapOffset);
                }
                else
                {
                    borderPositions.Add(new Vector3(
                        x * cellSize.x + cellSize.x * 0.5f,
                        y * cellSize.y + cellSize.y * 0.5f,
                        0
                    ) + tilemapOffset);
                }
            }
        }

        // Shuffle spawnable floor tiles
        floorPositions = floorPositions.OrderBy(p => Random.value).ToList();

        // ---- Spawn Enemies ----
        for (int i = 0; i < dna.enemyCount && i < floorPositions.Count; i++)
            Instantiate(enemyPrefab, floorPositions[i], Quaternion.identity, roomParent);

        // ---- Spawn Loot ----
        int offset = dna.enemyCount;
        for (int i = 0; i < dna.lootCount && (i + offset) < floorPositions.Count; i++)
            Instantiate(lootPrefab, floorPositions[i + offset], Quaternion.identity, roomParent);

        Vector3 exitPos = borderPositions[Random.Range(0, borderPositions.Count)];
        Instantiate(exitPrefab, exitPos, Quaternion.identity,roomParent);
    }

    public void RoomCompleted()
    {
        foreach(Transform child in roomParent)
        {
            Destroy(child.gameObject);
        }
        tileMap.ClearAllTiles();

        firstGeneration = true;
        population.Clear();

        for (int i = 0; i < populationSize; i++)
            population.Add(new RoomDNA());

        currentGeneration = 0;

        RunEvolution();
    }

    float EvaluateFitness(RoomDNA dna)
    {
        return dna.fitness;
    }

}