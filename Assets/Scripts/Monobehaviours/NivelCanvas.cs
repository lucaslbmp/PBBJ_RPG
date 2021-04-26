using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NivelCanvas : MonoBehaviour
{
    public Text textoNivel;
    Coroutine MensagemCorrotina;

    private void Start()
    {
        StartCoroutine(MostrarNivel());
    }

    public IEnumerator MostrarNivel()
    {
        MostrarMensagemNivel("Nivel " + (int)GameManager.nivelAtual);
        yield return new WaitForSeconds(5f);
        MostrarMensagemNivel("");
    }

    public void MostrarMensagemNivel(string mensagem)
    {
        textoNivel.enabled = true;
        textoNivel.text = mensagem;
    }
}
