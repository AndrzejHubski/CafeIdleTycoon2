using UnityEngine;

public class RandomHead : MonoBehaviour
{
    public GameObject[] heads;
    public Transform spawnPoint;

    void Start()
    {
        Instantiate(heads[Random.Range(0, heads.Length)], spawnPoint);
    }

}
