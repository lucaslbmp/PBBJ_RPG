using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que gerencia a mensagem exibida na parte inferior da tela
/// </summary>

public class MensagemCanvas : MonoBehaviour
{
    public Text mensagemTxt;

    //private void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}

    void Start()
    {
        ResetCanvas();                  // resta Canvas de mensagem 
    }

    // Metodo que atualiza mensagem do Canvas de mensagem
    public void AtualizarCanvas(string mensagem)
    {
        mensagemTxt.text = mensagem;
    }
    
    // Metodo que reseta o Canvas de mensagem
    public void ResetCanvas()
    {
        mensagemTxt.text = "";
    }
}
