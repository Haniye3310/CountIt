using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class SystemFunction
{
    public static void Start(DataRepo dataRepo,MonoBehaviour mono) 
    {
        foreach(PlayerData p in dataRepo.Players)
        {
            if(!p.IsUser)
                mono.StartCoroutine(Robot(dataRepo,p));
        }
    }
    public static IEnumerator StartPlayerWalkIn(DataRepo dataRepo,PlayerData p)
    {
        while (Vector3.Distance(p.PlayerGameObject.transform.position,p.TargetPosition.position)>0.2f)
        {
            MoveTowardsTarget(dataRepo, p.PlayerGameObject, p.PlayerRigidbody, (p.TargetPosition.position - p.PlayerGameObject.transform.position).normalized);
            yield return null;
        }
    }
    public static void OnSideTriggerEnter(DataRepo dataRepo,Collider other)
    {
        dataRepo.Count++;
        dataRepo.CountText.text = dataRepo.Count.ToString();
        for( int i = 0; i < dataRepo.mushrooms.Count;i++)
        {
            if (dataRepo.mushrooms[i].Mushroom.gameObject == other.gameObject)
            {
                dataRepo.mushrooms.RemoveAt(i);
            }
        }
        GameObject.Destroy(other.gameObject);
    }
    public static void StartMushroomMovement(DataRepo dataRepo, MonoBehaviour mono)
    {
        dataRepo.NumberOfMushrooms = Random.Range(18, 30);
        for (int i = 0; i < dataRepo.NumberOfMushrooms; i++)
        {
            int r = Random.Range(0, 2);
            Vector3 pos;
            if (r == 0)
                pos = GetRandomPointOnPlane(dataRepo.Plane1);
            else
                pos = GetRandomPointOnPlane(dataRepo.Plane2);

            Mushroom mushroom = GameObject.Instantiate(dataRepo.MushroomPrefab, pos, Quaternion.identity);
            dataRepo.mushrooms.Add(new MushroomData { Mushroom = mushroom });
        }
        foreach (MushroomData m in dataRepo.mushrooms)
        {
            mono.StartCoroutine(SetTarget(dataRepo, m));
        }
    }
    public static IEnumerator StartTimer(DataRepo dataRepo,MonoMessageReciever mono)
    {
        dataRepo.RemainingTimeInGame = dataRepo.TimeOftheGame;
        while (dataRepo.RemainingTimeInGame > 0)
        {
            dataRepo.RemainingTimeInGame -= Time.deltaTime;
            if ((int)dataRepo.RemainingTimeInGame < 5)
            {
                dataRepo.TimerImage.color = Color.red;
            }
            dataRepo.RemainingTimeText.text = ((int)dataRepo.RemainingTimeInGame).ToString();
            yield return null;
        }

        dataRepo.IsGameFinished = true;
        dataRepo.InGameUIPanel.gameObject.SetActive(false);
        foreach (MushroomData m in dataRepo.mushrooms)
        {
            mono.StartCoroutine(MushroomWalkout(dataRepo, m));

        }
        while(dataRepo.mushrooms.Count != 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2);
        dataRepo.ResultPanel.gameObject.SetActive(true);
        string winners = "";
        foreach(PlayerData p in dataRepo.Players)
        {
            if(dataRepo.NumberOfMushrooms == p.Count)
            {
                winners += p.PlayerGameObject.name+",";
            }
        }
        if (winners == "")
            winners = "nobody";
        dataRepo.WinnersText.text = winners + "Win!";
    }
    public static IEnumerator MushroomWalkout(DataRepo dataRepo,MushroomData m)
    {
        int r = Random.Range(0, 2);
        Vector3 direction;
        if (r == 0)
        {
            direction = (dataRepo.RightPos.position - dataRepo.CenterPos.position).normalized;
        }
        else
        {
            direction = (dataRepo.LeftPos.position - dataRepo.CenterPos.position).normalized;

        }
        while (m.Mushroom!=null)
        {
            MoveTowardsTarget(dataRepo, m.Mushroom.gameObject, m.Mushroom.MushroomRigidbody, direction);
            yield return null;
        }
    }
    public static void MoveTowardsTarget(DataRepo dataRepo,GameObject gameObject,Rigidbody rigidbody,Vector3 direction)
    {
        // Move the player
        gameObject.transform.Translate(direction * dataRepo.SpeedOfMushroomMovement * Time.deltaTime, Space.World);
        Debug.DrawRay(gameObject.transform.position, direction, Color.red);
        // Handle rotation if there's movement
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            gameObject.transform.rotation = Quaternion.Slerp(rigidbody.transform.rotation, targetRotation, Time.deltaTime * 100);
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
            if(dataRepo.IsGameFinished) { return; }
            if(m.TargetPosition != Vector3.zero)
            {
                MoveTowardsTarget(dataRepo,m.Mushroom.gameObject,m.Mushroom.MushroomRigidbody,(m.TargetPosition - m.Mushroom.gameObject.transform.position).normalized);
            }
        }
    }

    public static IEnumerator SetTarget(DataRepo dataRepo,MushroomData mushroomData)
    {
        while(true)
        {
            float rt = Random.Range(3f,8f);
            int r = Random.Range(0, 2);
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
        float planeWidth = 3f * scale.x; // Unity Plane has a default size of 10x10
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
            float rt = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(rt);
        }

    }


}
