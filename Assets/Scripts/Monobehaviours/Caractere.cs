using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe abstrata que gerencia aspectos dos personagens, como pontos de dano, reset, dano e morte
/// </summary>
public abstract class Caractere : MonoBehaviour
{
    //public int PontosDano; //Vers�o anterior do valor de "dano"
    
    // public int MaxPontosDano; //Vers�o anterior do valor m�ximo de "dano"
    public float inicioPontosDano; // valor m�nimo inicial de "sa�de" do player
    public float MaxPontosDano; // novo tipo que tem o valor m�ximo do objeto script

    public abstract void ResetCaractere();          // Fun�ao que reseta o caractere

    // Corrotina que faz o "piscar" do caractere quandoele sofre dano
    public virtual IEnumerator FlickerCaractere()
    {
        GetComponent<SpriteRenderer>().color = Color.red;           // Muda a cor do sprite do carctere para vermelho
        yield return new WaitForSeconds(0.1f);                      // Aguarda 0.1 seg
        GetComponent<SpriteRenderer>().color = Color.white;         // Volta a cor do sprite do caractere para branco
    }

    public abstract IEnumerator DanoCaractere(int dano, float intervalo);       // Corrotina que gerencia o dano ao caractere com este script

    // Fun�ao que gerencia a morte do caractere
    public virtual void KillCaractere()
    {
        Destroy(gameObject);
    }
}
