using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IBall : IMovable
{
    Vector3 ballDirection { get; set; }

    void OnPlayerScore(int player);

    bool CheckLeftTranspass();

    bool CheckRightTranspass();
}

