using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Caractere : MonoBehaviour
{
    //public int PontosDano; //Versão anterior do valor de "dano"
    
    // public int MaxPontosDano; //Versão anterior do valor máximo de "dano"
    public float inicioPontosDano; // valor mínimo inicial de "saúde" do player
    public float MaxPontosDano; // novo tipo que tem o valor máximo do objeto script

    public abstract void ResetCaractere();

    public abstract IEnumerator DanoCaractere(int dano, float intervalo);

    public virtual void KillCaractere()
    {
        Destroy(gameObject);
    }
}
