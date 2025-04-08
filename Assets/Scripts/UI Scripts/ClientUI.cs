using UnityEngine;

public class ClientUI : MonoBehaviour
{
    public Transform orderPreviewRoot;

    public void ShowOrder(DishData dish)
    {
        foreach (Transform child in orderPreviewRoot)
            Destroy(child.gameObject);

        GameObject model = Instantiate(dish.modelPrefab, orderPreviewRoot);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(0, 180, 0);
        model.transform.localScale = Vector3.one * 0.2f;

        foreach (var col in model.GetComponentsInChildren<Collider>())
            Destroy(col);
    }
}