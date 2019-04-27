using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    private int barSpeed;
    private int ballSpeed;
    private Vector3 ballDirection;

    private const int winPoints = 10;
    private int playerOneScore;
    private int playerTwoScore;

    /*private Vector3 leftbarPosition;
    private Vector3 rightbarPosition;
    private Vector3 ballPosition;*/

    //para o controller aceder (ler) aos atributos privados
    public int WinPoints { get { return winPoints; } }
    public int BarSpeed { get { return barSpeed; } }
    public int BallSpeed { get { return ballSpeed; } }
    public int PlayerOneScore { get { return playerOneScore; } }
    public int PlayerTwoScore { get { return playerTwoScore; } }


    /*public delegate void MoveLeftbarEvent(Vector3 pos);
    public static event MoveLeftbarEvent onMoveLeftbar;*/

    public delegate void ChangeBallDirectionEventHandler(Vector3 pos);
    public static event ChangeBallDirectionEventHandler ChangeBallDirectionEvent;

    //inicializa os atributos
    void Awake()
    {
        barSpeed = 15;
        ballSpeed = 13;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
    }

    //inverte o eixo y da trajetoria da bola
    public void OnBoundCollision()
    {
        ballDirection.y *= -1;
        ChangeBallDirectionEvent(ballDirection);
    }

    //inverte o eixo x (direcao da bola) da trajetoria da bola
    public void OnBarCollision()
    {
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

        Debug.Log("Pontuacao: " + playerOneScore + "  -  " + playerTwoScore);
    }
}
