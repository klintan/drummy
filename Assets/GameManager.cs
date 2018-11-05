using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int multiplier = 1;
    int streak = 0;

    void Start()
    {
        PlayerPrefs.SetInt("Score", 0);

    }

    void Update()
    {

    }

    public void AddStreak()
    {
        streak++;
        if (streak % 8 == 0 && multiplier < 4)
        {
            multiplier++;
        }
        UpdateGUI();
    }


    public void ResetStreak()
    {
        streak = 0;
        multiplier = 1;
        UpdateGUI();
    }

    private void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
    }

    public int GetScore()
    {
        return 100 * multiplier;
    }

    void UpdateGUI(){
        PlayerPrefs.SetInt("Streak", streak);
        PlayerPrefs.SetInt("Mult", multiplier);
    }
}
