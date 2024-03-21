using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreTextField;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private static int m_highScore = 0;

    private bool m_GameOver = false;
    

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        m_highScore = ReadHighScoreFromFile();
        UpdateBestScoreText();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        if(m_Points > m_highScore)
        {
            m_highScore = m_Points;
        }

        UpdateBestScoreText();
    }

    void UpdateBestScoreText()
    {
        BestScoreTextField.text = $"Best Score: {m_highScore} Name: {StartMenuManager.Shared.PlayerName}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveHighScoreData();
    }

    private int GetHighScore()
    {
        return m_highScore;
    }

    void SaveHighScoreData()
    {
        HighScoreData data = new HighScoreData();
        data.HighScore = m_highScore;

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/highscore.json";
        File.WriteAllText(path, json);
    }

    int ReadHighScoreFromFile()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        string json = File.ReadAllText(path);

        HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
        if(data != null)
        {
            return data.HighScore;
        }
        else
        {
            return 0;
        }
    }
}

[System.Serializable]
class HighScoreData
{
    public int HighScore = 0;
}
