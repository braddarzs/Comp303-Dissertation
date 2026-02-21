using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GenerateRoom roomGenerator;

    public void SetGenerationMode(bool state)
    {
        roomGenerator.randomGeneration = state;

        roomGenerator.StartGeneration();

        gameObject.SetActive(false);
    }
}
