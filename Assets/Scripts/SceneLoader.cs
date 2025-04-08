using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameManager gameManager;

    public void LoadMainMenu()
    {
        gameManager.SaveGame();

        SceneManager.LoadScene("MainMenu");
    }
}