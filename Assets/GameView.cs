using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{

    public GameObject playerOneScoreboard;
    public GameObject playerTwoScoreboard;

    public void OnObjectsConfiguration(object ob, Color color)
    {
        (ob as Rigidbody).GetComponent<MeshRenderer>().material.color = color;
        //...poderia ter mais configurações
    }

    //quando um erro é gerado
    public void OnError(string erro)
    {
        //mostra em debug
        Debug.Log(erro);
        //por implementar: guardar os logs um ficheiro
    }

    //atualiza a pontuação dos jogadores
    public void OnPlayerScores(int playerOneScore, int playerTwoScore)
    {
        if(playerOneScoreboard != null && playerTwoScoreboard != null)
        {
            playerOneScoreboard.GetComponent<Text>().text = playerOneScore.ToString();
            playerTwoScoreboard.GetComponent<Text>().text = playerTwoScore.ToString();
        }
    }
}
