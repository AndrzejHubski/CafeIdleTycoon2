using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;

    public Slider volumeSlider;

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("volume", 1f);

        AudioListener.volume = volume;
        volumeSlider.value = volume; 

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("GameScene");
        mainPanel.SetActive(false); 
    }

    public void OnSettingsClicked()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

    public void OnBackClicked()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OnResetProgressClicked()
    {
        PlayerPrefs.DeleteAll();

        string path = System.IO.Path.Combine(Application.persistentDataPath, "save.json");
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);

        UpgradeManager.Instance.ResetUpgrades();

        Debug.Log("Data reset");
    }

    private void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
        PlayerPrefs.Save();

        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(value);
    }
}