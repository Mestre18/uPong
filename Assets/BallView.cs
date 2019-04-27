using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{

    public delegate void BoundColisionEventHandler();
    public static event BoundColisionEventHandler BoundColisionEvent;

    public delegate void BarColisionEventHandler();
    public static event BarColisionEventHandler BarColisionEvent;

    //sempre que a bola colidir com algum objeto
    private void OnCollisionEnter(Collision collision)
    {
        //verifica se colidiu com os limites (superior/inferior)
        if (collision.gameObject.tag == "Boundaries")
        {
            BoundColisionEvent();
        }
        //verifica se colidiu com as barras (esquerda/direita)
        else if (collision.gameObject.tag == "Bars")
        {
            BarColisionEvent();
        }

    }

    //move a bola para nova posição
    public void OnMoveBall(Vector3 pos)
    {
        this.GetComponent<Rigidbody>().velocity = pos;
    }

    public void OnPlayerScore(int player)
    {
        this.GetComponent<Rigidbody>().position = Vector3.zero;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
