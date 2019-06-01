using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Eventos
    public delegate void MoveLeftbarEventHandler(Vector3 pos);
    public static event MoveLeftbarEventHandler MoveLeftBarEvent;

    public delegate void MoveRightbarEventHandler(Vector3 pos);
    public static event MoveRightbarEventHandler MoveRightBarEvent;

    public delegate void MoveBallEventHandler(Vector3 pos);
    public static event MoveBallEventHandler MoveBallEvent;

    public delegate void PlayerScoresEventHandler(int player);
    public static event PlayerScoresEventHandler PlayerScoresEvent;

    public delegate void ErrorEventHandler(string error);
    public static event ErrorEventHandler ErrorEvent;

    public MenuView menuView;
    public GameView gameView;
    public GameModel model;
    public IBall ball;
    public IMovable leftBar;
    public IMovable rightBar;


    void Awake()
    {
        model = GetComponent<GameModel>();
        gameView = GetComponent<GameView>();
        ball = GameObject.Find("Ball").GetComponent<BallModel>();
        leftBar = GameObject.Find("Left Bar").GetComponent<LeftBarModel>();
        rightBar = GameObject.Find("Right Bar").GetComponent<RightBarModel>();

        /***********************SUBSCRICOES AOS EVENTOS*******************/
        //movimentacoes: barras e bola
        MoveLeftBarEvent += leftBar.OnMovePosition;
        MoveRightBarEvent += rightBar.OnMovePosition;
        MoveBallEvent += ball.OnMovePosition;

        //pontuacao jogadores
        PlayerScoresEvent += model.OnPlayerScores;
        PlayerScoresEvent += ball.OnPlayerScore;
        GameModel.PlayerScoresEvent += gameView.OnPlayerScores;

        //configuracoes
        leftBar.ObjectsConfiguration += gameView.OnObjectsConfiguration;
        rightBar.ObjectsConfiguration += gameView.OnObjectsConfiguration;
        ball.ObjectsConfiguration += gameView.OnObjectsConfiguration;

        //erros
        ErrorEvent += gameView.OnError;
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //inicializa os atributos com as configurações no ficheiro
            model.LoadConfigFile();

            //inicia o movimento da bola
            StartCoroutine(LaunchBall());
        }
        catch (ConfigFileMissingException error)
        {
            ErrorEvent("Erro: " + error.Message + " Ficheiro: " + error.FileName);
            Time.timeScale = 0;
            return;
        }
        catch (FormatException)
        {
            ErrorEvent("Ficheiro de configuração contém erros!");
            Time.timeScale = 0;
            return;
        }
        catch (InvalidCastException)
        {
            ErrorEvent("Ficheiro de configuração contém erros!");
            Time.timeScale = 0;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //verifica se algum jogador pontuou
        CheckScore();

        //enquanto não receber um input as barras não se movem
        NoBarMove();

        //verifica se recebeu inputs
        GetInput();

    }

    //faz uma pausa de 2 segundos e inicia o movimento da bola
    IEnumerator LaunchBall()
    {
        yield return new WaitForSeconds(2f);

        StartMovingBall();
    }

    //as barras não se movem
    public void NoBarMove()
    {
        if (MoveLeftBarEvent != null)
            MoveLeftBarEvent(new Vector3(0, 0, 0));

        if (MoveRightBarEvent != null)
            MoveRightBarEvent(new Vector3(0, 0, 0));
    }

    public void GetInput()
    {
        /**************BARRA ESQUERDA*************/
        //se a tecla carregada for W...move a barra esquerda para cima
        if (Input.GetKey(KeyCode.W))
        {
            if (MoveLeftBarEvent != null)
                MoveLeftBarEvent(new Vector3(0, leftBar.moveSpeed, 0));
        }
        //se a tecla carregada for S...move a barra esquerda para baixo
        else if (Input.GetKey(KeyCode.S))
        {
            if (MoveLeftBarEvent != null)
                MoveLeftBarEvent(new Vector3(0, -leftBar.moveSpeed, 0));
        }

        /**************BARRA DIREITA*************/
        //se a tecla carregada for Up...move a barra direita para cima
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (MoveRightBarEvent != null)
                MoveRightBarEvent(new Vector3(0, rightBar.moveSpeed, 0));
        }
        //se a tecla carregada for Down...move a barra direita para baixo
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (MoveRightBarEvent != null)
                MoveRightBarEvent(new Vector3(0, -rightBar.moveSpeed, 0));
        }
    }

    public void StartMovingBall()
    {
        //determina aleatoriamente a direcao X e Y da bola
        int x = UnityEngine.Random.Range(0, 2);
        int y = UnityEngine.Random.Range(0, 2);

        Vector3 ballDirection = new Vector3();

        //x=0: move a bola para a esquerda (-x)
        if (x == 0)
            ballDirection.x = -ball.moveSpeed;
        //x=1: move a bola para a direita (+x)
        else if (x == 1)
            ballDirection.x = ball.moveSpeed;

        //y=0: move a bola para baixo (-y)
        if (y == 0)
            ballDirection.y = -ball.moveSpeed;
        //y=1: move a bola para cima (+y)
        else if (y == 1)
            ballDirection.y = ball.moveSpeed;
        //y=2: não move a bola no eixo do y
        /*else if (y == 2)
            ballDirection.y = 0;*/

        //chama o evento
        if (MoveBallEvent != null)
            MoveBallEvent(ballDirection);
    }

    //verifica se algum jogador pontuou
    public void CheckScore()
    {
        //se ultrapassou a barra direita, o jogador 1 pontua
        if (ball.CheckRightTranspass())
        {
            //Jogador 1 pontua!
            PlayerScoresEvent(1);

            //verifica se já tem os pontos necessarios para ganhar o jogo
            if (model.PlayerOneScore >= model.WinPoints)
            {
                //o jogador 1 venceu o jogo
                SceneManager.LoadScene(2);

            }
            else
            {
                //se ainda nao venceu o jogo, volta a lançar a bola
                StartCoroutine(LaunchBall());
            }

        }
        //se ultrapassou a barra esquerda, o jogador 2 pontua
        else if (ball.CheckLeftTranspass())
        {
            //Jogador 2 pontua!
            PlayerScoresEvent(2);

            //verifica se já tem os pontos necessarios para ganhar o jogo
            if (model.PlayerTwoScore >= model.WinPoints)
            {
                //o jogador 2 venceu o jogo
                SceneManager.LoadScene(3);
            }
            else
            {
                //se ainda nao venceu o jogo, volta a lançar a bola
                StartCoroutine(LaunchBall());
            }
        }
    }
}
