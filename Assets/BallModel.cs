using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallModel : MonoBehaviour, IBall
{

    public int moveSpeed { get; set; }
    public Color objectColor { get; set; }
    public Vector3 ballDirection { get; set; }

    public event EventHandler<Color> ObjectsConfiguration;

    //guarda as configurações bola (carregadas pelo ficheiro XML)
    public void Configuration(int speed, Color color)
    {
        moveSpeed = speed;
        objectColor = color;

        ObjectsConfiguration(this.GetComponent<Rigidbody>(), color);
    }

    //move a bola para nova posição
    public void OnMovePosition(Vector3 pos)
    {
        ballDirection = pos;

        if(this != null)
            this.GetComponent<Rigidbody>().velocity = pos;
    }

    //sempre que a bola colidir com algum objeto
    public void OnCollisionEnter(Collision collision)
    {
        Vector3 dir = ballDirection;

        //se colidiu com os limites
        if (collision.gameObject.tag == "Boundaries")
            //inverte o eixo y da trajetoria da bola
            dir.y *= -1;

        //se colidiu com as barras (esquerda/direita)
        else if (collision.gameObject.tag == "Bars")
            //inverte o eixo x (direcao da bola) da trajetoria da bola
            dir.x *= -1;

            
        OnMovePosition(dir);
    }

    //quando um jogador pontua a bola volta para a posição inicial
    public void OnPlayerScore(int player)
    {
        if (this != null) { 
            this.transform.position = Vector3.zero;
            OnMovePosition(Vector3.zero);
        }
    }

    //verifica se a bola transpassou a barra esquerda
    public bool CheckLeftTranspass()
    {
        if (this.transform.position.x < -20f)
            return true;

        return false;
    }

    //verifica se a bola transpassou a barra direita
    public bool CheckRightTranspass()
    {
        if (this.transform.position.x > 20f)
            return true;

        return false;
    }
}
