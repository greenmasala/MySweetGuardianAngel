using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public float enemyTimeScale = 1;
    public float timerTimeScale = 1;
    public List<GameObject> obstacles = new List<GameObject>();
    [SerializeField] float timer = 25f;
    public TextMeshProUGUI TimerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacles.AddRange(GameObject.FindGameObjectsWithTag("Obstacle"));
    }

    // Update is called once per frame
    void Update()
    {
        TimerText.text = timer.ToString("00.000");

        timer -= Time.deltaTime * timerTimeScale;
        Debug.Log(timer);

        if (obstacles.Count <= 0)
        {
            Debug.Log("WIN!!");
        }
    }
}
