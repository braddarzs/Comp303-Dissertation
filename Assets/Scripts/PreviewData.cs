using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewData : MonoBehaviour
{
    public Slider slider;
    public RawImage rawImage;
    public GameObject selectButton;

    public TMP_Text enemyCount;
    public TMP_Text lootCount;

    public RoomDNA roomDNA;

    private GenerateRoom roomGenerator;

    private void Start()
    {
        roomGenerator = FindFirstObjectByType<GenerateRoom>();
    }

    public void Init()
    {
        enemyCount.text = "Enemies: " + roomDNA.enemyCount.ToString();
        lootCount.text = "Loot: " + roomDNA.lootCount.ToString();
    }

    public void RoomSelected()
    {
        roomGenerator.GenerateChosen(roomDNA);
    }

    public void UpdateFitness()
    {
        roomDNA.fitness = Convert.ToInt32(slider.value);
    }

 }
