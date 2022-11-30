using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int Score;
    public int comboMeter;
    float maxComboTime = 5;
    public float comboTimer = 5;
    public float currentTime = 60;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI ComboNum;
    public Image comboMeterUI;

    int highestCombo;

    [SerializeField] GameObject[] activeNormalTargets;
    [SerializeField] GameObject[] activeExplosiveTargets;
    [SerializeField] GameObject[] activeGhostTargets;
    [SerializeField] GameObject[] targetPatterns;
    [SerializeField] GameObject targetRange;
    [SerializeField] GameObject comboUI;
    [SerializeField] TextMeshProUGUI comboplusUI;
    [SerializeField] GameObject finishScreen;
    [SerializeField] TextMeshProUGUI HighestCombo;
    [SerializeField] TextMeshProUGUI FinalScore;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Instantiate(targetPatterns[0], targetRange.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        activeExplosiveTargets = GameObject.FindGameObjectsWithTag("Explosive Target");
        activeNormalTargets = GameObject.FindGameObjectsWithTag("Normal Target");
        activeGhostTargets = GameObject.FindGameObjectsWithTag("Pass Through Target");

        if(activeNormalTargets.Length <= 0 && activeExplosiveTargets.Length <= 0 && activeGhostTargets.Length <= 0)
        { SpawnNewRange(); }

        comboMeterUI.fillAmount = comboTimer / maxComboTime;
        ComboNum.text = comboMeter.ToString();
        comboplusUI.text = "+" + (comboMeter * 100).ToString();
        currentTime -= 1 * Time.deltaTime;
        if(currentTime <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currentTime = 0;
            comboTimer = 0;
            finishScreen.SetActive(true);
            HighestCombo.text = highestCombo.ToString();
            FinalScore.text = Score.ToString();
        }
        ScoreText.text = Score.ToString();
        Timer.text = currentTime.ToString("0");

        if(comboMeter >= 1)
        { comboUI.SetActive(true); }
        else if(comboMeter == 0)
        { comboUI.SetActive(false); }
        
        if(!AxeScript.instance.itemHit)
        {
            comboTimer = 5;
            Score += comboMeter * 100;
            comboMeter = 0; 
        }
        else if(AxeScript.instance.itemHit)
        {
            comboTimer -= 1 * Time.deltaTime;
        }

        if(comboTimer <= 0)
        {
            if(comboMeter > highestCombo)
            { highestCombo = comboMeter; }
            Score += comboMeter * 100;
            AxeScript.instance.itemHit = false;
        }
    }

    void SpawnNewRange()
    {
        var hitTargets = GameObject.FindGameObjectsWithTag("Hit Target");
        for(int i = 0; i < GameObject.FindGameObjectsWithTag("Hit Target").Length; i++)
        {
            Destroy(hitTargets[i]);
        }
        int randNum = Random.Range(1, targetPatterns.Length);
        Instantiate(targetPatterns[randNum], targetRange.transform.position, Quaternion.identity);
    }

    public void Retry()
    {
        Score = 0;
        comboMeter = 0;
        currentTime = 60;
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
    }

    public void ReturnToMain()
    {
        Score = 0;
        comboMeter = 0;
        currentTime = 60;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
