using System;
using System.Collections.Generic;
using UnityEngine;

public class DataRepo : MonoBehaviour
{
    public List<PlayerData> Players;
    public int NumberOfObjects;
}

[Serializable]
public class PlayerData
{
    public int Count;
    public bool IsUser;
}