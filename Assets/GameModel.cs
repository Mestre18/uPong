using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class GameModel : MonoBehaviour
{

    //Atributos privados
    private int barSpeed;
    private int ballSpeed;
    private int winPoints;
    private Color objectsColor;
    //private Color backgroundColor;

    private Vector3 ballDirection;
    private int playerOneScore;
    private int playerTwoScore;

    //para o controller aceder aos atributos privados (apenas leitura)
    public int WinPoints { get { return winPoints; } }
    public int BarSpeed { get { return barSpeed; } }
    public int BallSpeed { get { return ballSpeed; } }
    public int PlayerOneScore { get { return playerOneScore; } }
    public int PlayerTwoScore { get { return playerTwoScore; } }
    public Color ObjectsColor { get { return objectsColor; } }
    //public Color BackgroundColor { get { return backgroundColor; } }

    //Delegates & Events
    public delegate void ChangeBallDirectionEventHandler(Vector3 pos);
    public static event ChangeBallDirectionEventHandler ChangeBallDirectionEvent;

    public delegate void PlayerScoresEventHandler(int playerOneScore, int playerTwoScore);
    public static event PlayerScoresEventHandler PlayerScoresEvent;


    // Start is called before the first frame update
    void Start()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
    }

    public void LoadConfigFile()
    {
        var xmlDoc = new XmlDocument();

        //lê as configuracoes do ficheiro config.xml
        //verifica se existe o ficheiro...se nao existir, lança excecao!
        if (!File.Exists("config.xml"))
            throw new ConfigFileMissingException("Ficheiro de configuração não encontrado!", "config.xml");

        xmlDoc.Load("config.xml");
        var xmlDocElem = xmlDoc.DocumentElement;

        try
        {
            //lê as configurações do jogo
            var gameConfig = xmlDocElem.SelectSingleNode("/config/game");
            this.barSpeed = Convert.ToInt32(gameConfig.Attributes["barSpeed"].Value);
            this.ballSpeed = Convert.ToInt32(gameConfig.Attributes["ballSpeed"].Value);
            this.winPoints = Convert.ToInt32(gameConfig.Attributes["winPoints"].Value);

            //costumizacoes
            var CostumConfig = gameConfig.SelectSingleNode("costumization");
            this.objectsColor = new Color(Convert.ToInt32(CostumConfig.Attributes["objectsColorRed"].Value), Convert.ToInt32(CostumConfig.Attributes["objectsColorGreen"].Value), Convert.ToInt32(CostumConfig.Attributes["objectsColorBlue"].Value));
            //this.backgroundColor = new Color(Convert.ToInt32(CostumConfig.Attributes["backgroundColorRed"].Value), Convert.ToInt32(CostumConfig.Attributes["backgroundColorGreen"].Value), Convert.ToInt32(CostumConfig.Attributes["backgroundColorBlue"].Value));

        }
        catch (Exception error)
        {
            throw new ConfigFileMissingException("Ficheiro de configuração contém erros!", "config.xml");
        }
    }

    //a bola colide com um objeto (barras/limites)
    public void OnBallCollision(Collision collision)
    {
        //se colidiu com os limites
        if (collision.gameObject.tag == "Boundaries")
            //inverte o eixo y da trajetoria da bola
            ballDirection.y *= -1;

       //se colidiu com as barras (esquerda/direita)
       else if (collision.gameObject.tag == "Bars")
            //inverte o eixo x (direcao da bola) da trajetoria da bola
            ballDirection.x *= -1;


        ChangeBallDirectionEvent(ballDirection);
    }

    //move a bola para nova posicao
    public void OnMoveBall(Vector3 pos)
    {
        ballDirection = pos;
        ChangeBallDirectionEvent(ballDirection);
    }

    //aumenta a pontuacao do jogador
    public void OnPlayerScores(int player)
    {
        if(player == 1)
            playerOneScore++;
        else if (player == 2)
            playerTwoScore++;

        PlayerScoresEvent(playerOneScore, playerTwoScore);

        //Debug.Log("Pontuacao: " + playerOneScore + "  -  " + playerTwoScore);
    }
}
