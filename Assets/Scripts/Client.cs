using System;
using System.Collections;
using UnityEngine;

public class Client : MonoBehaviour
{
    [Header("Moving")]
    public Transform enterPoint;
    public Transform targetPoint;
    public Transform exitPoint;
    public float moveSpeed = 5f;

    [Header("Visualisation")]
    public Transform holdPoint;
    public Transform orderPreviewRoot;

    [HideInInspector] public DishData currentOrder;

    public Action OnServed;
    public Action<Client, DishData> OnArrivedAtRegister;

    private GameObject carriedItem;

    private void Start()
    {
        StartCoroutine(MoveTo(targetPoint.position));
    }

    public void SetOrder(DishData dish)
    {
        currentOrder = dish;
    }

    public void ReceiveOrder(GameObject dishPrefab)
    {
        HideOrderVisual();

        carriedItem = Instantiate(dishPrefab, holdPoint);
        carriedItem.transform.localPosition = Vector3.zero;
        carriedItem.transform.localRotation = Quaternion.identity;
        carriedItem.transform.localScale = Vector3.one * 0.5f;

        StartCoroutine(MoveTo(exitPoint.position, leaveAfter: true));
    }

    private IEnumerator MoveTo(Vector3 destination, bool leaveAfter = false)
    {
        destination.y = transform.position.y;

        while (Vector3.Distance(transform.position, destination) > 0.05f)
        {
            Vector3 dir = (destination - transform.position).normalized;

            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 8f);
            }

            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (!leaveAfter)
        {
            ShowOrderVisual();
            OnArrivedAtRegister?.Invoke(this, currentOrder);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            OnServed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void ShowOrderVisual()
    {
        if (currentOrder != null && orderPreviewRoot != null)
        {
            foreach (Transform child in orderPreviewRoot)
                Destroy(child.gameObject);

            GameObject model = Instantiate(currentOrder.modelPrefab, orderPreviewRoot);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.Euler(0, 180, 0);
            model.transform.localScale = Vector3.one * 0.2f;

            foreach (var col in model.GetComponentsInChildren<Collider>())
                Destroy(col);
        }
    }

    private void HideOrderVisual()
    {
        if (orderPreviewRoot == null) return;

        foreach (Transform child in orderPreviewRoot)
            Destroy(child.gameObject);
    }
}