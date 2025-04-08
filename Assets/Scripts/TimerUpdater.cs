using UnityEngine;

public class TimerUpdater : MonoBehaviour
{
    private void Update()
    {
        DeliveryTimerManager.Instance.UpdateTimers();
    }
}