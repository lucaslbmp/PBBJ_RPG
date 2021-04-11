using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItensParaColetar : MonoBehaviour
{
    public const int numSlots = 5; //numero fixo de slots

    public Text TxtItensParaColetar;

    //Dictionary<string, int> itensNaCena; // dicionario de itens a serem coletados pelo player na cena atual
    //Dictionary<string, int> itensColetados; // dicionario de itens já coletados pelo player na cena atual


    void Start()
    {
        ResetCanvas();
    }
    
    public void AtualizarCanvas()
    {
        Dictionary<string, int> itensNaCena = GameManager.dicionarioColetaveis;
        Dictionary<string, int> itensColetados = GameManager.itensColetados;
        int qtdItemColetado;
        string textoCanvas = "";
        textoCanvas += "Itens para coletar:\n";
        foreach (string item in itensNaCena.Keys)
        {
            if (itensColetados.ContainsKey(item))
                qtdItemColetado = itensColetados[item];
            else
                qtdItemColetado = 0;
            textoCanvas += item + " : " + qtdItemColetado.ToString() + "/" + itensNaCena[item] + "\n";
        }
        TxtItensParaColetar.text = textoCanvas;
    }

    public void ResetCanvas()
    {
        Dictionary<string, int> itensNaCena = GameManager.dicionarioColetaveis;
        string textoCanvas = "";
        textoCanvas += "Itens para coletar:\n";
        foreach (string item in itensNaCena.Keys)
        {
            textoCanvas += item + " : " + "0/" + itensNaCena[item] + "\n";
        }
        TxtItensParaColetar.text = textoCanvas;
    }
}
