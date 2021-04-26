using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anciao : Caractere 
{ 
    float pontosVida; //  equivalente à saude do inimigo
    public float duracaoMensagem; // tempo de exibiçao da fala do caractere

    Coroutine mensagemCoroutine;
    protected bool tocouPlayer; 

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        ResetCaractere();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Se o objeto com o qual o caractere colidiu foi o player...
        {
            if (mensagemCoroutine == null) // Se a corrotina de fala nao se iniciou
            {
                mensagemCoroutine = StartCoroutine(CaractereFala("ANCIÃO: Use a chave para localizar o pergaminho!", duracaoMensagem)); // inicie a corrotina de fala do caractere
            }
        }
        //else
        //{
        //    if(!collision.gameObject.CompareTag("Inimigo"))
        //        gameObject.GetComponent<Perambular>().Retornar(true); // Se o caractere colidir com um objeto, retornar (i.e., dar meia-volta)
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //gameObject.GetComponent<Perambular>().Retornar(false); // Se o caractere deixar de colidir com um objeto, parar de retornar
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    if (mensagemCoroutine != null)
        //    {
        //        StopCoroutine(mensagemCoroutine);
        //        mensagemCoroutine = null;
        //    }
        //}
    }

    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        while (true)
        {
            StartCoroutine(FlickerCaractere());
            pontosVida = pontosVida - dano;

            if (pontosVida <= float.Epsilon)
            {
                KillCaractere();
                break;
            }

            if (intervalo > float.Epsilon)
            {
                yield return new WaitForSeconds(intervalo);
            }

            else
            {
                break;
            }
        }
    }

    public IEnumerator CaractereFala(string fala, float duracao)
    {
        while (!tocouPlayer)
        {
            StartCoroutine(FlickerCaractere());
            GameManager.ExibirMensagem(fala);
            if (duracao > 0f)
                yield return new WaitForSeconds(duracao);
            GameManager.ExibirMensagem("");
            tocouPlayer = true;
        }
        if (mensagemCoroutine != null)
            StopCoroutine(mensagemCoroutine);
    }

    public override void ResetCaractere()
    {
        pontosVida = inicioPontosDano;
    }

    // Update is called once per frame
    void Update()
    {

    }
}