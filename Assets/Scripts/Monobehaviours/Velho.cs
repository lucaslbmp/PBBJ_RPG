using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velho : Inimigo
{
    public float duracaoMensagem; // tempo de exibiçao da fala do caractere

    public Item itemFornecido;          // Item que o NPC force ao ser tocado
    Coroutine mensagemCoroutine;        // Armazena corrotina de mensagem
    protected bool tocouPlayer;         // Flag que indica se o NPC tocou o player

    Player player;                      // Armazena o player

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Se o objeto com o qual o NPC colidiu foi o player...
        {
            player = collision.gameObject.GetComponent<Player>();
            if (mensagemCoroutine == null) // Se a corrotina de fala nao se iniciou
            {
                mensagemCoroutine = StartCoroutine(CaractereFala("ANCIÃO: Use a chave para localizar o pergaminho!", duracaoMensagem)); 
                // inicia a corrotina de fala do caractere
            }
        }
    }


    public IEnumerator CaractereFala(string fala, float duracao)
    {
        while (!tocouPlayer)                                    // Enquanto nao tocou om player...
        {
            StartCoroutine(FlickerCaractere());         // Inicia corrotina de "piscar" o NPC
            GameManager.ExibirMensagem(fala);           // Exibe a fala do NPC
            player.inventario.AddItem(itemFornecido);       // Adiciona item fornecido ao inventario do player
            if (duracao > 0f)                               // Se a duraçao da fala for maior quer zero...
                yield return new WaitForSeconds(duracao);       // Aguarde "duraçao"
            GameManager.ExibirMensagem("");                 // Exibe mensagem vazia
            tocouPlayer = true;                             // Muda a flag tocouPlayer para true
        }
        if (mensagemCoroutine != null)                      // Se a corrotina de mensagem está sendo executada...
            StopCoroutine(mensagemCoroutine);               // Pare a corrotina de mensagem
    }

    // Corrotina de dano no NPC
    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        yield return null;
    }
}
