using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool isReplay = false;

    [Header("References"), Space]
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject MoveCamera;
    [SerializeField] private GameObject StartGameMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject fadeOutImg;
    [SerializeField] private GameObject fadeInImg;
    [SerializeField] private GameObject scoreGO;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreText_GameOver;

    private int score = 0;

    public bool CanEscape { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        StartCoroutine(ResetGame());
    }

    private IEnumerator ResetGame()
    {
        if (!isReplay)
        {
            CanEscape = false;
        }
        else
        {
            CanEscape = true;
            MoveCamera.SetActive(false);
            MainCamera.SetActive(true);
            scoreGO.SetActive(true);

            TurnOffAllMenu();
        }

        fadeInImg.SetActive(true);
        fadeInImg.GetComponent<Animator>().Play("FadeIn");
        WayManager.Instance.UpdateWayControl(0);
        yield return new WaitForSecondsRealtime(2f);
        fadeInImg.SetActive(false);

        if (!isReplay)
        {
            WayManager.Instance.UpdateWayControl(0);
        }
        else
        {
            WayManager.Instance.UpdateWayControl(8);
        }
    }

    private void TurnOffAllMenu()
    {
        fadeOutImg.SetActive(false);
        GameOverMenu.SetActive(false);
        PauseMenu.SetActive(false);
        StartGameMenu.SetActive(false);
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();

        if (score % 5 == 0)
        {
            float currentSpeed = WayManager.Instance.MoveSpeed;
            float newSpeed = Mathf.Clamp(currentSpeed + .4f, 0, WayManager.Instance.MaxSpeed);
            WayManager.Instance.UpdateWayControl(newSpeed);
        }
    }

    public void StartGame()
    {
        StartGameMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        PauseMenu.SetActive(false);
        
        StartCoroutine(PlayCameraAnimation());
    }

    private IEnumerator PlayCameraAnimation()
    {
        CanEscape = false;
        MoveCamera.GetComponent<Animator>().Play("Cam Move Animation");

        yield return new WaitForSeconds(4f);

        MoveCamera.SetActive(false);
        MainCamera.SetActive(true);
        scoreGO.SetActive(true);

        Time.timeScale = 1;
        CanEscape = true;
        WayManager.Instance.UpdateWayControl(8);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        StartGameMenu.SetActive(false);
        GameOverMenu.SetActive(true);
        PauseMenu.SetActive(false);
        scoreText.enabled = false;
        CanEscape = false;
        
        scoreText_GameOver.text = score.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void Retry()
    {
        isReplay = true;

        Time.timeScale = 1;

        StartCoroutine(PlayFadeOut());
    }

    IEnumerator PlayFadeOut()
    {
        fadeOutImg.SetActive(true);
        fadeOutImg.GetComponent<Animator>().Play("FadeOut");
        WayManager.Instance.UpdateWayControl(0);
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        StartGameMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        CanEscape = true;
        TurnOffAllMenu();
    }

}
