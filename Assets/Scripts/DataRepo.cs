using System;
using System.Collections.Generic;
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
    public int Count;
    public bool IsUser;
}
[Serializable]
public class MushroomData
{
    //public Rigidbody MushroomRigidbody;
    //public GameObject MushroomGameObject;
    public Mushroom Mushroom;
    public Vector3 TargetPosition;
}