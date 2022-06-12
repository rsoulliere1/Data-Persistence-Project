using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{

    [SerializeField] Text BScoreText;
    [SerializeField] InputField nameField;

    public string name = "" ;

    public int bScore;
    public string bScoreName;

    public static GameManager Instance;


    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
    }



    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(bScoreName);
        BScoreText.text = $"Best : {bScoreName}: {bScore}";

    }

    public void SetBestScore(int score)
    {
        if (score > bScore)
        {
            bScore = score;
            bScoreName = name;
            SaveHighScore();
            MainManager.Instance.BestScoreText.text = $"Best Score : {bScoreName}: {bScore}";
        }
        Debug.Log("Score: " + score + "  Player: " + name);
    }

    public void StartNew()
    {
        if (nameField.text != "")
        {
            name = nameField.text;
            SceneManager.LoadScene(1);
        } else
        {
            Debug.LogWarning("No name!");
        }
    }
    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }



    [System.Serializable]
    public class SaveData
    {
        public int BestScore;
        public string BestScoreName;
    }

    public void SaveHighScore()
    {

        SaveData data = new SaveData();
        data.BestScore = bScore;
        data.BestScoreName = bScoreName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }


    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
       
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
          
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bScore = data.BestScore;
            bScoreName = data.BestScoreName;


        }
    }
}
