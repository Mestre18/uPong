using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public delegate void MoveLeftbarEventHandler(Vector3 pos);
    public static event MoveLeftbarEventHandler MoveLeftbarEvent;

    public delegate void MoveRightbarEventHandler(Vector3 pos);
    public static event MoveRightbarEventHandler MoveRightbarEvent;

    public delegate void MoveBallEventHandler(Vector3 pos);
    public static event MoveBallEventHandler MoveBallEvent;

    public delegate void PlayerScoresEventHandler(int player);
    public static event PlayerScoresEventHandler PlayerScoresEvent;

    public GameModel model;
    public GameView view;
    public BallView ball;

    void Awake()
    {
        model = GetComponent<GameModel>();
        view = GetComponent<GameView>();
        ball = GameObject.Find("Ball").GetComponent<BallView>();

        /***********************SUBSCRICOES AOS EVENTOS*******************/
        //movimentacoes: barras e bola
        MoveLeftbarEvent += view.OnMoveLeftbar;
        MoveRightbarEvent += view.OnMoveRightbar;
        MoveBallEvent += model.OnMoveBall;
        //MoveBallEvent += ball.OnMoveBall;

        //colisoes da bola com os limites e barras
        BallView.BoundColisionEvent += model.OnBoundCollision;
        BallView.BarColisionEvent += model.OnBarCollision;
        GameModel.ChangeBallDirectionEvent += ball.OnMoveBall;

        //pontuacao jogadores
        PlayerScoresEvent += model.OnPlayerScores;
        PlayerScoresEvent += ball.OnPlayerScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        //inicia o movimento da bola
        StartMovingBall();
    }

    // Update is called once per frame
    void Update()
    {
        //verifica se algum jogador pontuou
        //CheckScore();

        //enquanto não receber um input as barras não se movem
        NoBarMove();

        //verifica se recebeu input
        GetInput();
    }
   

    //as barras não se movem
    public void NoBarMove()
    {
        if (MoveLeftbarEvent != null)
            MoveLeftbarEvent(new Vector3(0, 0, 0));

        if (MoveRightbarEvent != null)
            MoveRightbarEvent(new Vector3(0, 0, 0));
    }

    public void GetInput()
    {
        /**************BARRA ESQUERDA*************/
        //se a tecla carregada for W...move a barra esquerda para cima
        if (Input.GetKey(KeyCode.W))
        {
            if (MoveLeftbarEvent != null)
                MoveLeftbarEvent(new Vector3(0, model.BarSpeed, 0));
        }
        //se a tecla carregada for S...move a barra esquerda para baixo
        else if (Input.GetKey(KeyCode.S))
        {
            if (MoveLeftbarEvent != null)
                MoveLeftbarEvent(new Vector3(0, -model.BarSpeed, 0));
        }

        /**************BARRA DIREITA*************/
        //se a tecla carregada for Up...move a barra direita para cima
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (MoveRightbarEvent != null)
                MoveRightbarEvent(new Vector3(0, model.BarSpeed, 0));
        }
        //se a tecla carregada for Down...move a barra direita para baixo
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (MoveRightbarEvent != null)
                MoveRightbarEvent(new Vector3(0, -model.BarSpeed, 0));
        }
    }

    public void StartMovingBall()
    {
        //determina aleatoriamente a direcao X e Y da bola
        int x = Random.Range(0, 2);
        int y = Random.Range(0, 3);

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
        else if (y == 2)
            ballDirection.y = 0;

        //chama o evento
        if (MoveBallEvent != null)
            MoveBallEvent(ballDirection);
    }

    //verifica se algum jogador pontuou
    public void CheckScore()
    {
        

        if (ball.transform.position.x > 20f)
        {
            //StartCoroutine(Pause());

            ball.GetComponent<Rigidbody>().position = new Vector3(0, 0, 0);

            //Jogador 1 pontua!
            PlayerScoresEvent(1);

            //verifica se já tem os pontos necessarios para ganhar o jogo
            if (model.PlayerOneScore >= model.WinPoints)
            {
                Debug.Log("JOGADOR 1 VENCEU!");
            }
            else
            {
                StartMovingBall();
            }

        }    
        if (ball.transform.position.x < -20f)
        {

            ball.GetComponent<Rigidbody>().position = new Vector3(0, 0, 0);
            //Jogador 2 pontua!
            PlayerScoresEvent(2);

            //verifica se já tem os pontos necessarios para ganhar o jogo
            if (model.PlayerTwoScore >= model.WinPoints)
            {
                Debug.Log("JOGADOR 2 VENCEU!");
            }
            else
            {
                StartMovingBall();
            }
        }
    }

    /*IEnumerator Pause()
    {
        yield return new WaitForSeconds(2f);
    }*/
}
