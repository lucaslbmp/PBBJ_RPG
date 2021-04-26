using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MensagemCanvas : MonoBehaviour
{
    public Text mensagemTxt;

    //private void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}

    void Start()
    {
        ResetCanvas();
    }

    public void AtualizarCanvas(string mensagem)
    {
        mensagemTxt.text = mensagem;
    }

    public void ResetCanvas()
    {
        mensagemTxt.text = "";
    }
}
