using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anciao : Caractere 
{ 
    float pontosVida; //equivalente à saude do inimigo
    public int forcaDano; // poder de dano

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
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (mensagemCoroutine == null)
            {
                mensagemCoroutine = StartCoroutine(CaractereFala("Use a chave para localizar o pergaminho!", 1.0f));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (mensagemCoroutine != null)
            {
                StopCoroutine(mensagemCoroutine);
                mensagemCoroutine = null;
            }
        }
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