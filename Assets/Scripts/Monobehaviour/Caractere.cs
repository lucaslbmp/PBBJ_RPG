using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Caractere : MonoBehaviour
{
    //public int PontosDano; // versao anterior do valor de "dano"
    public PontosDano pontosDano; // novo tipo que tem o valor do objeto script
    //public int MaxPontosDano; // versao anterior do valor da variavel de maximo de pontos de dano
    public float inicioPontosDano; // valor minimo inicial de "saude" do player
    public float MaxPontosDano;  // valor maximo de saude permitido do player
}
