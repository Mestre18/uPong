using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public delegate void MoveLeftbarEventHandler(Vector3 pos);
    public static event MoveLeftbarEventHandler MoveLeftBarEvent;

    public delegate void MoveRightbarEventHandler(Vector3 pos);
    public static event MoveRightbarEventHandler MoveRightBarEvent;

    public delegate void MoveBallEventHandler(Vector3 pos);
    public static event MoveBallEventHandler MoveBallEvent;

    public delegate void PlayerScoresEventHandler(int player);
    public static event PlayerScoresEventHandler PlayerScoresEvent;

    public GameModel model;
    public GameView view;
    public BallView ball;
    //public Camera camera;

    void Awake()
    {
        model = GetComponent<GameModel>();
        view = GetComponent<GameView>();
        ball = GameObject.Find("Ball").GetComponent<BallView>();
        //camera = GameObject.Find("Main Camera").GetComponent<Camera>();


        /***********************SUBSCRICOES AOS EVENTOS*******************/
        //movimentacoes: barras e bola
        MoveLeftBarEvent += view.OnMoveLeftbar;
        MoveRightBarEvent += view.OnMoveRightbar;
        MoveBallEvent += model.OnMoveBall;
        //MoveBallEvent += ball.OnMoveBall;
        GameModel.ChangeBallDirectionEvent += ball.OnMoveBall;

        //colisoes da bola com os limites e barras
        /*BallView.BoundColisionEvent += model.OnBoundCollision;
        BallView.BarColisionEvent += model.OnBarCollision;*/
        BallView.ColisionEvent += model.OnBallCollision;
       
        //pontuacao jogadores
        PlayerScoresEvent += model.OnPlayerScores;
        PlayerScoresEvent += ball.OnPlayerScore;
        GameModel.PlayerScoresEvent += view.OnPlayerScores;
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //inicializa os atributos com as configurações no ficheiro
            model.LoadConfigFile();

            view.leftbar.GetComponent<MeshRenderer>().material.color = model.ObjectsColor;
            view.rightbar.GetComponent<MeshRenderer>().material.color = model.ObjectsColor;
            ball.GetComponent<MeshRenderer>().material.color = model.ObjectsColor;
            //camera.backgroundColor = model.BackgroundColor;
   
            //inicia o movimento da bola
            StartCoroutine(LaunchBall());
        }
        catch (ConfigFileMissingException error)
        {
            Debug.Log("Erro: " + error.Message + " Ficheiro: " + error.FileName);
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

        /*try
        {
            //verifica se recebeu inputs
            GetInput();
        }
        catch(Exception e)
        {
            Debug.Log("Erro: " + e.Message);
        }*/

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
                MoveLeftBarEvent(new Vector3(0, model.BarSpeed, 0));
        }
        //se a tecla carregada for S...move a barra esquerda para baixo
        else if (Input.GetKey(KeyCode.S))
        {
            if (MoveLeftBarEvent != null)
                MoveLeftBarEvent(new Vector3(0, -model.BarSpeed, 0));
        }

        /**************BARRA DIREITA*************/
        //se a tecla carregada for Up...move a barra direita para cima
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (MoveRightBarEvent != null)
                MoveRightBarEvent(new Vector3(0, model.BarSpeed, 0));
        }
        //se a tecla carregada for Down...move a barra direita para baixo
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (MoveRightBarEvent != null)
                MoveRightBarEvent(new Vector3(0, -model.BarSpeed, 0));
        }

        //se carregou em alguma outra tecla gera mensagem de erro
        //if(Input.anyKey && (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)))
            //throw new Exception("Tecla inválida!");
    }

    public void StartMovingBall()
    {
        //determina aleatoriamente a direcao X e Y da bola
        int x = UnityEngine.Random.Range(0, 2);
        int y = UnityEngine.Random.Range(0, 2);

        Vector3 ballDirection = new Vector3();

        //x=0: move a bola para a esquerda (-x)
        if (x == 0)
            ballDirection.x = -model.BallSpeed;
        //x=1: move a bola para a direita (+x)
        else if (x == 1)
            ballDirection.x = model.BallSpeed;

        //y=0: move a bola para baixo (-y)
        if (y == 0)
            ballDirection.y = -model.BallSpeed;
        //y=1: move a bola para cima (+y)
        else if (y == 1)
            ballDirection.y = model.BallSpeed;
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
        if (ball.transform.position.x > 20f)
        {
            
            //Jogador 1 pontua!
            PlayerScoresEvent(1);

            //verifica se já tem os pontos necessarios para ganhar o jogo
            if (model.PlayerOneScore >= model.WinPoints)
            {
                Debug.Log("JOGADOR 1 VENCEU!");
                //gera evento
                //....
            }
            else
            {
                //se ainda nao venceu o jogo, volta a lançar a bola
                StartCoroutine(LaunchBall());
            }

        }
        //se ultrapassou a barra esquerda, o jogador 2 pontua
        else if (ball.transform.position.x < -20f)
        {
            //Jogador 2 pontua!
            PlayerScoresEvent(2);

            //verifica se já tem os pontos necessarios para ganhar o jogo
            if (model.PlayerTwoScore >= model.WinPoints)
            {
                Debug.Log("JOGADOR 2 VENCEU!");
                //gera evento
                //....
            }
            else
            {
                //se ainda nao venceu o jogo, volta a lançar a bola
                StartCoroutine(LaunchBall());
            }
        }
    }
}
