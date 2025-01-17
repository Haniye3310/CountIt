using System.Collections;
using UnityEngine;

public class SystemFunction
{
    public static void Start(DataRepo dataRepo) 
    {
        dataRepo.NumberOfObjects = Random.Range(18, 30);
    }
    public static void Update(DataRepo dataRepo) { }

    public static void Count(DataRepo dataRepo) 
    {
        foreach(PlayerData p in dataRepo.Players)
        {
            if(p.IsUser)
            {
                p.Count++;
            }
        }
    }
    public static IEnumerator Robot(DataRepo dataRepo,PlayerData p)
    {
        int finalCount = Random.Range(dataRepo.NumberOfObjects - 5, dataRepo.NumberOfObjects + 8);
        while (p.Count < finalCount)
        {
            p.Count++;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
