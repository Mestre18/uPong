using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{

    public GameObject leftbar;
    public GameObject rightbar;

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
}
