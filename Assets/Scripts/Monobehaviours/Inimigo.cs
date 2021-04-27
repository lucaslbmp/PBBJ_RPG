using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia aspectos dos inimigos, como pontos de vida, formas de aplicar e receber dano e reset
/// </summary>

public class Inimigo : Caractere
{

    float pontosVida; //equivalente à saude do inimigo
    public int forcaDano; // poder de dano

    Coroutine danoCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        ResetCaractere();                           // Reseta o caractere
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))                                          // Se o inimigo colide com o player...
        {
            Player player = collision.gameObject.GetComponent<Player>();                    // Pga componente Player do player
            if (danoCoroutine == null)                                                      // Se a corrotina de dano nao esta sendo executada...
            {
                danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));      // Inicia a corrotina de dano, com intervalo de 1 seg
            }
        }
    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))                            // Se o inimigo deixa de colidir com o player...
        {
            if(danoCoroutine != null)                                           // Se a corrotina de dano está sendo executada
            {
                StopCoroutine(danoCoroutine);                                   // Para a corrotina de dano
                danoCoroutine = null;                                           // Atribui null à corrotina de dano
            }
        }
    }


    // Corrotina que recebe dano e intervalo e aplica dano ao inimigo a cada intervalo
    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        while (true)
        {
            StartCoroutine(FlickerCaractere());                     // Faz inimigo "piscar"
            pontosVida = pontosVida - dano;                         // Decrementa pontos vida do inimigo

            if(pontosVida <= float.Epsilon)                         // Se pontos vida é menor ou igual a zero...
            {
                KillCaractere();                                    // Mata caractere
                break;
            }
            
            if(intervalo > float.Epsilon)                          // Se intervalo > 0...
            {
                yield return new WaitForSeconds(intervalo);         // Aguarda pelo tempo de intervalo até aplicar dano de novo
            }

            else
            {
                break;
            }
        }
    }

    // Metodo que reseta o caractere
    public override void ResetCaractere()
    {
        pontosVida = inicioPontosDano;              // Inicializa o pontos vida do inimigo com "inicioPontosDano"
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
