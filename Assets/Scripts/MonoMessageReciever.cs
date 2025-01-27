using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoMessageReciever : MonoBehaviour
{
    public DataRepo DataRepo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {

        SystemFunction.StartMushroomMovement(DataRepo, this);
        DataRepo.InGameUIPanel.gameObject.SetActive(false);
        while (Mathf.Abs(Camera.main.fieldOfView - 60) > 0.1)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, 0.1f);
            yield return null;
        }
        foreach(PlayerData p in DataRepo.Players)
        {
            StartCoroutine(SystemFunction.StartPlayerWalkIn(DataRepo,p));
        }

        DataRepo.StartCountDownTimer.gameObject.SetActive(true);
        float remaingStartTime = 3;
        while (remaingStartTime > 0)
        {
            remaingStartTime -= Time.deltaTime;
            DataRepo.StartCountDownTimer.text = ((int)remaingStartTime + 1).ToString();
            yield return null;
        }
        DataRepo.StartCountDownTimer.gameObject.SetActive(false);
        DataRepo.InGameUIPanel.gameObject.SetActive(true);
        StartCoroutine(SystemFunction.StartTimer(DataRepo,this));
        yield return new WaitForSeconds(1);
        SystemFunction.Start(DataRepo,this);
    }

    // Update is called once per frame
    void Update()
    {
        SystemFunction.Update(DataRepo);
    }

    public void OnCountButtonClick()
    {
        SystemFunction.Count(DataRepo);
    }
    public void OnRestartButtton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
