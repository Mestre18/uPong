using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{

    public GameObject leftbar;
    public GameObject rightbar;
    public GameObject playerOneScoreboard;
    public GameObject playerTwoScoreboard;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnMoveLeftbar(Vector3 pos)
    {
        leftbar.GetComponent<Rigidbody>().velocity = pos;
    }

    public void OnMoveRightbar(Vector3 pos)
    {
        rightbar.GetComponent<Rigidbody>().velocity = pos;
    }

    public void OnPlayerScores(int playerOneScore, int playerTwoScore)
    {
        playerOneScoreboard.GetComponent<Text>().text = playerOneScore.ToString();
        playerTwoScoreboard.GetComponent<Text>().text = playerTwoScore.ToString();
    }
}
