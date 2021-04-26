using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velho : Inimigo
{
    public float duracaoMensagem; // tempo de exibiçao da fala do caractere

    public Item itemFornecido;
    Coroutine mensagemCoroutine;
    protected bool tocouPlayer;

    Player player;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Se o objeto com o qual o caractere colidiu foi o player...
        {
            player = collision.gameObject.GetComponent<Player>();
            if (mensagemCoroutine == null) // Se a corrotina de fala nao se iniciou
            {
                mensagemCoroutine = StartCoroutine(CaractereFala("ANCIÃO: Use a chave para localizar o pergaminho!", duracaoMensagem)); // inicie a corrotina de fala do caractere
            }
        }
        //else
        //{
        //    if (!collision.gameObject.CompareTag("Inimigo"))
        //        gameObject.GetComponent<Perambular>().Retornar(true); // Se o caractere colidir com um objeto, retornar (i.e., dar meia-volta)
        //}
    }

    //public override void OnCollisionExit2D(Collision2D collision)
    //{
    //    gameObject.GetComponent<Perambular>().Retornar(false);
    //}

    public IEnumerator CaractereFala(string fala, float duracao)
    {
        while (!tocouPlayer)
        {
            StartCoroutine(FlickerCaractere());
            GameManager.ExibirMensagem(fala);
            player.inventario.AddItem(itemFornecido);
            if (duracao > 0f)
                yield return new WaitForSeconds(duracao);
            GameManager.ExibirMensagem("");
            tocouPlayer = true;
        }
        if (mensagemCoroutine != null)
            StopCoroutine(mensagemCoroutine);
    }

    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        yield return null;
    }
}
