using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{

    public delegate void ColisionEventHandler(Collision collision);
    public static event ColisionEventHandler ColisionEvent;

    //sempre que a bola colidir com algum objeto
    private void OnCollisionEnter(Collision collision)
    {
        //gera o envento colisao
        ColisionEvent(collision);
    }

    //move a bola para nova posição
    public void OnMoveBall(Vector3 pos)
    {
        this.GetComponent<Rigidbody>().velocity = pos;
    }

    public void OnPlayerScore(int player)
    {
        this.transform.position = Vector3.zero;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
