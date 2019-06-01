using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public interface IMovable
{
    int moveSpeed { get; set; }

    Color objectColor { get; set; }

    void Configuration(int speed, Color color);

    void OnMovePosition(Vector3 pos);

    event EventHandler<Color> ObjectsConfiguration;
}

