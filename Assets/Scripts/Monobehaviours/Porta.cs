using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que controla a porta
/// </summary>

public class Porta : MonoBehaviour
{
    public Item chave;                          // Item que abre a porta
    Player player;                              // Armazena o player

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))                          // Se colidiu com o player...
        {
            player = collision.gameObject.GetComponent<Player>();         // Obtem o componente Player com o player
            //print(player.inventario.itens);
            if (BuscaChave())                                      // Checa se existe a chave no inventario do player
            {
                player.inventario.RemoveItem(chave);                                // Remove acheve do inventario
                player.inventario.AtualizarListaColetaveis(chave.tipoItem.ToString());  // Atualizar lista de itens coletados do player
                GameManager.itensColetados = player.inventario.itensColetados;      // Atualiza lista de itens coletados do Game Manager
                Destroy(gameObject);            // Destroi o gameobject da porta
            }
        }
    }

    // Metodo que busca a chave no inventario e retorna true se achou
    public bool BuscaChave()
    {
        Item[] itensPlayer = player.inventario.itens;           // Obtem o array de itens do player
        foreach (Item item in itensPlayer)                      // Para cada item no array de itens do player...
        {
            if(item != null)                                    // Se o item nao é nulo...
            {
                if (item.tipoItem == chave.tipoItem)            // Se o tipo de item do array é igual ao tipo de item da chave
                {
                    return true;        // Retorna atrue
                }
            }
        }
        return false;           // Retorna false
    }
}
