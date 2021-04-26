using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public Item chave;
    Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            //print(player.inventario.itens);
            if (BuscaChave())
            {
                player.inventario.RemoveItem(chave);
                player.inventario.AtualizarListaColetaveis(chave.tipoItem.ToString());
                GameManager.itensColetados = player.inventario.itensColetados;
                Destroy(gameObject);
            }
        }
    }

    public bool BuscaChave()
    {
        Item[] itensPlayer = player.inventario.itens;
        foreach (Item item in itensPlayer)
        {
            if(item != null)
            {
                if (item.tipoItem == chave.tipoItem)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
