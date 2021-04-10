using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Caractere : MonoBehaviour
{
    //public int PontosDano; //Vers�o anterior do valor de "dano"
    
    // public int MaxPontosDano; //Vers�o anterior do valor m�ximo de "dano"
    public float inicioPontosDano; // valor m�nimo inicial de "sa�de" do player
    public float MaxPontosDano; // novo tipo que tem o valor m�ximo do objeto script

    public abstract void ResetCaractere();

    public virtual IEnumerator FlickerCaractere()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public abstract IEnumerator DanoCaractere(int dano, float intervalo);

    //public virtual IEnumerator CaractereFala(string mensagem, float duracao)
    //{
    //    GameManager.ExibirMensagem(mensagem);
    //    if(duracao > 0f)
    //        yield return new WaitForSeconds(duracao);
    //}

    public virtual void KillCaractere()
    {
        Destroy(gameObject);
    }
}
