using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemigo//Una clase interface para que cuando llame a hacer un ataque dependiendo el enemigo haga un ataque o otro, para que sea mas escalable y menos codigo
{
    void Golpeado();
    float PoderGolpeao();
    int DanoEnemigo();
    void Ataques();
    void GolpeEspada();
}

