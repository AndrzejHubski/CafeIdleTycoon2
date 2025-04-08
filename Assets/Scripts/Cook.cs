using System.Collections;
using UnityEngine;

public class Cook : MonoBehaviour
{
    [Header("Links")]
    public Transform fridge;
    public Transform stove;
    public Transform register;
    public Transform holdPoint;

    [Header("Parameters")]
    public float speed = 2f;
    public GameObject progressUIPrefab;

    private Client currentClient;
    private DishData currentDish;
    private ProgressBar progressBar;
    private GameObject carriedItem;

    public void StartOrder(DishData dish, Client client)
    {
        currentDish = dish;
        currentClient = client;
        StartCoroutine(ProcessOrder());
    }

    private IEnumerator ProcessOrder()
    {

        
        foreach (var ingredient in currentDish.requiredIngredients)
        {
            if (!ingredient.isUnlimited)
            {
                bool used = InventoryManager.Instance.TryUse(ingredient, 1);
                if (!used)
                {
                    currentClient = null;
                    currentDish = null;
                    yield break;
                }
            }
        }

        yield return WalkTo(fridge.position);

        SpawnCarriedObject(currentDish.rawModelPrefab);

        yield return WalkTo(stove.position);

        float adjustedCookTime = currentDish.baseCookTime * UpgradeManager.Instance.GetCookSpeedMultiplier();
        yield return StartCoroutine(ShowProgressBar(adjustedCookTime));


        ReplaceCarriedObject(currentDish.modelPrefab);

        yield return WalkTo(register.position);

        if (carriedItem != null)
            Destroy(carriedItem);

        MoneyManager.Instance.AddMoney(currentDish.price);

        currentClient.ReceiveOrder(currentDish.modelPrefab);
        currentClient = null;
        currentDish = null;
    }

    private IEnumerator WalkTo(Vector3 target)
    {
        target.y = transform.position.y;

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            Vector3 dir = (target - transform.position).normalized;

            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 8f);
            }

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator ShowProgressBar(float duration)
    {
        GameObject ui = Instantiate(progressUIPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        ui.transform.SetParent(null);
        progressBar = ui.GetComponent<ProgressBar>();
        bool done = false;
        progressBar.StartProgress(duration, () => done = true);
        while (!done) yield return null;
    }

    private void SpawnCarriedObject(GameObject prefab)
    {
        if (prefab == null || holdPoint == null) return;

        carriedItem = Instantiate(prefab, holdPoint);
        carriedItem.transform.localPosition = Vector3.zero;
        carriedItem.transform.localRotation = Quaternion.Euler(0, 180, 0);
        carriedItem.transform.localScale = Vector3.one * 0.5f;
    }

    private void ReplaceCarriedObject(GameObject newPrefab)
    {
        if (carriedItem != null)
            Destroy(carriedItem);

        SpawnCarriedObject(newPrefab);
    }
}
