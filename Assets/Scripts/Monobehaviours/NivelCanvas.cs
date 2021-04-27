using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que gerencia o canvas que exibe o nome da fase no in�cio  da mesma
/// </summary>

public class NivelCanvas : MonoBehaviour
{
    public Text textoNivel;                             // Texto que conter� o nome da fase
    Coroutine MensagemCorrotina;                        // Armazena corrotina de exibi�ao da mensagem

    private void Start()
    {
        StartCoroutine(MostrarNivel());                 // Inicia corrotina que exibe o nivel da fase por um intervalo
    }

    // Corrotina que exibe o nivel da fase por um intervalo 
    public IEnumerator MostrarNivel()
    {
        MostrarMensagemNivel("Nivel " + (int)GameManager.nivelAtual);    // Atribui a mensagem "N�vel [nivelAtual]" ao Texto de NivelCanvas
        yield return new WaitForSeconds(5f);                             // Aguarda 5 seg
        MostrarMensagemNivel("");                                        // Esvazia o Texto de NivelCanvas
    }

    // Metodo que atribui uma mensagem ao texto do n�vel
    public void MostrarMensagemNivel(string mensagem)
    {
        textoNivel.enabled = true;
        textoNivel.text = mensagem;
    }
}
