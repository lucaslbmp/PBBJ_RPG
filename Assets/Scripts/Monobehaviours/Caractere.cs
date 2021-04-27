using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe abstrata que gerencia aspectos dos personagens, como pontos de dano, reset, dano e morte
/// </summary>
public abstract class Caractere : MonoBehaviour
{
    //public int PontosDano; //Versão anterior do valor de "dano"
    
    // public int MaxPontosDano; //Versão anterior do valor máximo de "dano"
    public float inicioPontosDano; // valor mínimo inicial de "saúde" do player
    public float MaxPontosDano; // novo tipo que tem o valor máximo do objeto script

    public abstract void ResetCaractere();          // Funçao que reseta o caractere

    // Corrotina que faz o "piscar" do caractere quandoele sofre dano
    public virtual IEnumerator FlickerCaractere()
    {
        GetComponent<SpriteRenderer>().color = Color.red;           // Muda a cor do sprite do carctere para vermelho
        yield return new WaitForSeconds(0.1f);                      // Aguarda 0.1 seg
        GetComponent<SpriteRenderer>().color = Color.white;         // Volta a cor do sprite do caractere para branco
    }

    public abstract IEnumerator DanoCaractere(int dano, float intervalo);       // Corrotina que gerencia o dano ao caractere com este script

    // Funçao que gerencia a morte do caractere
    public virtual void KillCaractere()
    {
        Destroy(gameObject);
    }
}
