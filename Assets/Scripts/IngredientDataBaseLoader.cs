using UnityEngine;

public class IngredientDatabaseLoader : MonoBehaviour
{
    public IngredientDatabase database;

    private void Awake()
    {
        IngredientDatabase.Instance = database;
    }
}
