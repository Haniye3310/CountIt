using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataRepo : MonoBehaviour
{
    public List<PlayerData> Players;
    [NonSerialized]public int NumberOfMushrooms;
    public Mushroom MushroomPrefab;
    public float SpeedOfMushroomMovement =5;
    public Transform Plane1;
    public Transform Plane2;
    [NonSerialized] public List<MushroomData> mushrooms = new List<MushroomData>();
    public GameObject ResultPanel;
    public GameObject InGameUIPanel;
    [NonSerialized] public int TimeOftheGame = 30;
    [NonSerialized] public float RemainingTimeInGame;
    public TextMeshProUGUI RemainingTimeText;
    public Image TimerImage;
    public TextMeshProUGUI StartCountDownTimer;
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