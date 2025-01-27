using UnityEngine;

public class ScreenSide : MonoBehaviour
{
    public DataRepo DataRepo;
    private void OnTriggerEnter(Collider other)
    {
        SystemFunction.OnSideTriggerEnter(DataRepo,other);
    }
}
