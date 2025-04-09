using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCam == null) return;

        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
    }
}
