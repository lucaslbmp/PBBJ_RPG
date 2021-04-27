using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que gerencia o canvas que exibe o nome da fase no início  da mesma
/// </summary>

public class NivelCanvas : MonoBehaviour
{
    public Text textoNivel;                             // Texto que conterá o nome da fase
    Coroutine MensagemCorrotina;                        // Armazena corrotina de exibiçao da mensagem

    private void Start()
    {
        StartCoroutine(MostrarNivel());                 // Inicia corrotina que exibe o nivel da fase por um intervalo
    }

    // Corrotina que exibe o nivel da fase por um intervalo 
    public IEnumerator MostrarNivel()
    {
        MostrarMensagemNivel("Nivel " + (int)GameManager.nivelAtual);    // Atribui a mensagem "Nível [nivelAtual]" ao Texto de NivelCanvas
        yield return new WaitForSeconds(5f);                             // Aguarda 5 seg
        MostrarMensagemNivel("");                                        // Esvazia o Texto de NivelCanvas
    }

    // Metodo que atribui uma mensagem ao texto do nível
    public void MostrarMensagemNivel(string mensagem)
    {
        textoNivel.enabled = true;
        textoNivel.text = mensagem;
    }
}
