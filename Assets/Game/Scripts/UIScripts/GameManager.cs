using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool IsMenuOpened = false;
    public GameObject menuUI;

    public GameObject scoreUI;
    public Slider sensitivitySlider;
    public static GameManager instance;
    public GameObject resultsUI;         
    public Text resultsText;
    public bool IsGameEnded = false;
    public Toggle soundToggle;
    private GameObject playerUI;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 2f);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerManager pm = FindObjectOfType<PlayerManager>();
        Debug.Log("PlayerManager found: " + pm);
        if (pm != null)
        {
            CameraManager cam = pm.GetComponentInChildren<CameraManager>();
            Debug.Log("CameraManager found: " + cam);
            if (cam != null)
            {
                cam.camLookSpeed = value;
                cam.camPivotSpeed = value;
                Debug.Log("Sensitivity set to: " + value);
            }
        }
    }

    public void EndGame()
    {
        IsGameEnded = true;
        Time.timeScale = 0f;
        int blue = ScoreBoard.instance.blueTeamScore;
        int red = ScoreBoard.instance.redTeamScore;

        string winner = blue > red ? "Синяя команда победила!"
                      : red > blue ? "Красная команда победила!"
                      : "Ничья!";

        resultsText.text = $"Синяя: {blue}  |  Красная: {red}\n{winner}";

        menuUI.SetActive(false);
        scoreUI.SetActive(false);
        resultsUI.SetActive(true);        

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioListener.volume = 0f;
    }

    public void OnSoundToggleChanged(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("ShowTitleMenu", 1);
        PhotonNetwork.Disconnect(); 
    }

    void Update()
    {
        if (playerUI == null)
        {
            PlayerMovement[] allPM = FindObjectsOfType<PlayerMovement>();
            foreach (PlayerMovement pm in allPM)
            {
                if (pm.playerUI != null)
                {
                    playerUI = pm.playerUI;
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && IsMenuOpened == false && !IsGameEnded)
        {
            scoreUI.SetActive(false);
            menuUI.SetActive(true);
            if (playerUI != null) playerUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            IsMenuOpened = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && IsMenuOpened == true)
        {
            scoreUI.SetActive(true);
            menuUI.SetActive(false);
            if (playerUI != null) playerUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            IsMenuOpened = false;
            AudioListener.volume = soundToggle.isOn ? 1f : 0f;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
