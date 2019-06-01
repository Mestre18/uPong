using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftBarModel : MonoBehaviour, IMovable
{
    public int moveSpeed { get; set; }

    public Color objectColor { get; set; }

    public event EventHandler<Color> ObjectsConfiguration;

    //guarda as configurações bola (carregadas pelo ficheiro XML)
    public void Configuration(int speed, Color color)
    {
        moveSpeed = speed;
        objectColor = color;

        //informa a view para mudar a cor do objeto
        ObjectsConfiguration(this.GetComponent<Rigidbody>(), color);
    }

    //move a barra para nova posição
    public void OnMovePosition(Vector3 pos)
    {
        if (this != null)
            this.GetComponent<Rigidbody>().velocity = pos;
    }
}
