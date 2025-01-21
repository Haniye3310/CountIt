using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataRepo : MonoBehaviour
{
    public List<PlayerData> Players;
    [NonSerialized]public int NumberOfMushrooms;
    public Mushroom MushroomPrefab;
    public List<Transform> SpawnPositions;
    public float SpeedOfMushroomMovement =5;
    public Transform Plane1;
    public Transform Plane2;
    [NonSerialized] public List<MushroomData> mushrooms = new List<MushroomData>();

}

[Serializable]
public class PlayerData
{
    [NonSerialized]public int Count;
    public bool IsUser;
    public TextMeshProUGUI CountText;
}
[Serializable]
public class MushroomData
{
    public Mushroom Mushroom;
    public Vector3 TargetPosition;
}