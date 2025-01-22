using System.Collections;
using System.Data.Common;
using UnityEngine;

public class SystemFunction
{
    public static void Start(DataRepo dataRepo,MonoBehaviour mono) 
    {
        dataRepo.NumberOfMushrooms = Random.Range(18, 30);
        if (dataRepo.NumberOfMushrooms > dataRepo.SpawnPositions.Count)
        {
            Debug.LogError("There isnt enough position to generate objects. increase the spawn positions");
        }
        for (int i = 0; i < dataRepo.NumberOfMushrooms; i++)
        {

            Mushroom mushroom = GameObject.Instantiate(dataRepo.MushroomPrefab, dataRepo.SpawnPositions[0].position, Quaternion.identity);
            dataRepo.mushrooms.Add(new MushroomData {Mushroom = mushroom });
            dataRepo.SpawnPositions.RemoveAt(0);
        }
        foreach (MushroomData m in dataRepo.mushrooms)
        {
            mono.StartCoroutine(SetTarget(dataRepo,m));
        }
        foreach(PlayerData p in dataRepo.Players)
        {
            if(!p.IsUser)
                mono.StartCoroutine(Robot(dataRepo,p));
        }
    }
    public static void MoveTowardsTarget(DataRepo dataRepo,MushroomData mushroomData,Vector3 direction)
    {
        // Move the player
        mushroomData.Mushroom.gameObject.transform.Translate(direction * dataRepo.SpeedOfMushroomMovement * Time.deltaTime, Space.World);
        // Handle rotation if there's movement
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            mushroomData.Mushroom.gameObject.transform.rotation = Quaternion.Slerp(mushroomData.Mushroom.MushroomRigidbody.transform.rotation, targetRotation, Time.deltaTime * 100);
        }
        //// Set animator parameters
        //if (direction.magnitude < 0.1f)
        //{
        //    playerData.PlayerAnimator.SetFloat("MoveSpeed", 0);
        //}
        //else
        //{
        //    playerData.PlayerAnimator.SetFloat("MoveSpeed", 1);
        //}
    }
    public static void Update(DataRepo dataRepo) 
    {
        foreach(MushroomData m in dataRepo.mushrooms)
        {
            if(m.TargetPosition != Vector3.zero)
            {
                MoveTowardsTarget(dataRepo,m,(m.TargetPosition - m.Mushroom.gameObject.transform.position).normalized);
            }
        }
    }

    public static IEnumerator SetTarget(DataRepo dataRepo,MushroomData mushroomData)
    {
        while(true)
        {
            float rt = Random.Range(3f,8f);
            int r = Random.Range(0, 3);
            if(r == 0)
            {
                mushroomData.TargetPosition = GetRandomPointOnPlane(dataRepo.Plane1);
            }
            else
            {
                mushroomData.TargetPosition = GetRandomPointOnPlane(dataRepo.Plane2);
            }

            yield return new WaitForSeconds(rt);
        }
    }
    public static Vector3 GetRandomPointOnPlane(Transform planeTransform)
    {
        // Get the plane's size from its local scale
        Vector3 scale = planeTransform.localScale;
        float planeWidth = 2f * scale.x; // Unity Plane has a default size of 10x10
        float planeHeight = 2f * scale.z;

        // Generate random values within the plane's dimensions
        float randomX = Random.Range(-planeWidth / 2f, planeWidth / 2f);
        float randomZ = Random.Range(-planeHeight / 2f, planeHeight / 2f);

        // Transform the point from local to world space
        Vector3 localPoint = new Vector3(randomX, 0, randomZ);
        return planeTransform.TransformPoint(localPoint);
    }
    public static void Count(DataRepo dataRepo) 
    {
        foreach(PlayerData p in dataRepo.Players)
        {
            if(p.IsUser)
            {
                p.Count++;
                p.CountText.text = p.Count.ToString();
            }
        }
    }
    public static IEnumerator Robot(DataRepo dataRepo,PlayerData p)
    {
        int finalCount = Random.Range(dataRepo.NumberOfMushrooms - 5, dataRepo.NumberOfMushrooms + 8);
        while (p.Count < finalCount)
        {
            p.Count++;
            p.CountText.text = p.Count.ToString();
            yield return new WaitForSeconds(1f);
        }

    }


}
