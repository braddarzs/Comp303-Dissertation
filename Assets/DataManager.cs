using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RoomLogger : MonoBehaviour
{
    [System.Serializable]
    public class RoomLogEntry
    {
        public int roomWidth;
        public int roomLength;
        public int roomArea;

        public int enemyCount;
        public int lootCount;

    }

    [System.Serializable]
    public class RoomLogWrapper
    {
        public string playerId;
        public string sessionId;
        public string sessionType;
        public int deathCount;
        public float sessionLength;
        public List<RoomLogEntry> rooms = new();
    }


    public static RoomLogger Instance;
    public RoomLogWrapper logData = new();
    public RoomLogEntry currentRoomEntry;
    private string sessionId;
    [SerializeField] string playerId;
    [SerializeField] string sessionType;

    private void Awake()
    {
        Debug.Log(Application.persistentDataPath);
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sessionId = System.Guid.NewGuid().ToString();
        logData.sessionId = sessionId;
        logData.playerId = playerId;
        logData.sessionType = sessionType;
    }
    
    public RoomLogEntry AddRoomEntry()
    {
        if (currentRoomEntry != null) return null;
        currentRoomEntry = new();
        logData.rooms.Add(currentRoomEntry);
        return currentRoomEntry;
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
    private void OnDisable()
    {
        if (Application.isPlaying)
            SaveToJson();
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(logData, true);

        string path = Path.Combine(
            Application.persistentDataPath,
            $"room_log_{playerId}_{System.DateTime.Now:yyyyMMdd_HHmmss}.json"
        );

        File.WriteAllText(path, json);

        Debug.Log($"Room log saved to: {path}");
    }
}
