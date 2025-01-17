using UnityEngine;

public class MonoMessageReciever : MonoBehaviour
{
    public DataRepo DataRepo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SystemFunction.Start(DataRepo);
    }

    // Update is called once per frame
    void Update()
    {
        SystemFunction.Update(DataRepo);
    }

    void OnCountButtonClick()
    {
        SystemFunction.Count(DataRepo);
    }
}
