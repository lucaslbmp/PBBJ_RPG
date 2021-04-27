using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que gerencia o canvas que mostra a qtde de itens coletados/totais na cena
/// </summary>
/// 
public class ItensParaColetar : MonoBehaviour
{
    public const int numSlots = 5;              //numero fixo de slots

    public Text TxtItensParaColetar;            // Texto que contem a relçao de itens e  aparecerá no canvas

    //Dictionary<string, int> itensNaCena; // dicionario de itens a serem coletados pelo player na cena atual
    //Dictionary<string, int> itensColetados; // dicionario de itens já coletados pelo player na cena atual


    void Start()
    {
        ResetCanvas();                          // Reseta o canvas
    }
    
    public void AtualizarCanvas()
    {
        Dictionary<string, int> itensNaCena = GameManager.dicionarioColetaveis;   // Pega o dicionarios de coletaveis na cena
        Dictionary<string, int> itensColetados = GameManager.itensColetados; // Pega o dicionarios de itens coletados na cena
        int qtdItemColetado;                                                  // armazena quantidade do item coletado
        string textoCanvas = "";                                              // texto que aparecerá no canvas
        textoCanvas += "Itens para coletar:\n";                               // Acrescento titulo "Itens para coletar:"
        foreach (string item in itensNaCena.Keys)                             // Para cada tipo de item na cena...
        {
            if (itensColetados.ContainsKey(item))                    // Se também existe este tipo de item no dicionario de itens coletados
                qtdItemColetado = itensColetados[item];              // Incrementa qtde do item coletado para este tipo de item
            else                                                     // Caso contrario
                qtdItemColetado = 0;                                 // Atribui zero à quantidade de itens coletados deste tipo de item
            // Adiciona ao texto do canvas: [tipo do item] : [qtde do item coletado]/[qtde total do item na cena];
            textoCanvas += item + " : " + qtdItemColetado.ToString() + "/" + itensNaCena[item] + "\n"; 
        }
        TxtItensParaColetar.text = textoCanvas;                     // Associa o texto construido ao texto do canvas  
    }

    // Reseta o canvas de itens coletados/totais
    public void ResetCanvas()
    {
        Dictionary<string, int> itensNaCena = GameManager.dicionarioColetaveis;         // Inicializa itens na cena
        string textoCanvas = "";                                               // Atribui texto vaxio ao canvas 
        textoCanvas += "Itens para coletar:\n";                                // Acrescenta "Itens para coletar:"
        foreach (string item in itensNaCena.Keys)                              // Para cada item na cena...
        {
            textoCanvas += item + " : " + "0/" + itensNaCena[item] + "\n";     // Atribui 0 a qtde de i 
        }
        TxtItensParaColetar.text = textoCanvas;                               // Associa o texto construido ao texto do canvas 
    }
}
