using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    public GameObject menu;
    public GameObject labelWinner;
    public GameObject labelError;
    public GameObject startButton;

    public void StartGame()
    {
        //faz o load do jogo
        SceneManager.LoadScene(1);
    }

    //sai do jogo
    public void ExitGame()
    {
        Application.Quit();
    }
}
