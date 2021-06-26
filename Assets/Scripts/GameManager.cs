using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOver;
    bool canRestart = false;
    public void PlayerIsDead()
    {
        gameOver.enabled = true;
        canRestart = true;
    }
    private void Start()
    {
        gameOver.enabled = false;
    }
    private void Update()
    {
        if(canRestart && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
