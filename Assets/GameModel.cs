using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class GameModel : MonoBehaviour
{

    public List<IBall>  balls = new List<IBall>();
    public List<IMovable> bars = new List<IMovable>();

    //Atributos privados
    private int winPoints;
    private int playerOneScore;
    private int playerTwoScore;

    //para o controller aceder aos atributos privados (apenas leitura)
    public int WinPoints { get { return winPoints; } }
    public int PlayerOneScore { get { return playerOneScore; } }
    public int PlayerTwoScore { get { return playerTwoScore; } }

    //eventos
    public delegate void PlayerScoresEventHandler(int playerOneScore, int playerTwoScore);
    public static event PlayerScoresEventHandler PlayerScoresEvent;

    void Awake()
    {
        bars.Add(GameObject.Find("Left Bar").GetComponent<LeftBarModel>() as IMovable);
        bars.Add(GameObject.Find("Right Bar").GetComponent<RightBarModel>() as IMovable);
        balls.Add(GameObject.Find("Ball").GetComponent<BallModel>() as IBall);
    }

    void Start()
    {
        //inicia com as pontuações a 0
        playerOneScore = 0;
        playerTwoScore = 0;
    }

    //le o ficheiro XML
    public void LoadConfigFile()
    {
        int barSpeed;
        int ballSpeed;
        Color objectsColor;

        var xmlDoc = new XmlDocument();

        //lê as configuracoes do ficheiro config.xml
        //verifica se existe o ficheiro...se nao existir, lança excecao!
        if (!File.Exists("config.xml"))
            throw new ConfigFileMissingException("Ficheiro de configuração não encontrado!", "config.xml");

        xmlDoc.Load("config.xml");
        var xmlDocElem = xmlDoc.DocumentElement;

        //lê as configurações do jogo
        var gameConfig = xmlDocElem.SelectSingleNode("/config/game");
        barSpeed = Convert.ToInt32(gameConfig.Attributes["barSpeed"].Value);
        ballSpeed = Convert.ToInt32(gameConfig.Attributes["ballSpeed"].Value);
        this.winPoints = Convert.ToInt32(gameConfig.Attributes["winPoints"].Value);

        //costumizacoes
        var CostumConfig = gameConfig.SelectSingleNode("costumization");
        objectsColor = new Color(Convert.ToInt32(CostumConfig.Attributes["objectsColorRed"].Value), Convert.ToInt32(CostumConfig.Attributes["objectsColorGreen"].Value), Convert.ToInt32(CostumConfig.Attributes["objectsColorBlue"].Value));
        
        //isto é apenas um exemplo que podemos ter uma lista do tipo da interface
        //percorre os vários objetos com a interface para guardar as configurações
        foreach(IMovable b in bars)
        {
            b.Configuration(barSpeed, objectsColor);
        }

        foreach (IBall b in balls)
        {
            b.Configuration(ballSpeed, objectsColor);
        }
    }

    //aumenta a pontuacao do jogador
    public void OnPlayerScores(int player)
    {
        if(player == 1)
            playerOneScore++;
        else if (player == 2)
            playerTwoScore++;

        PlayerScoresEvent(playerOneScore, playerTwoScore);
    }
}
