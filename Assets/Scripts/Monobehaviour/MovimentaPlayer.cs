using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentaPlayer : MonoBehaviour
{
    public float VelocidadeMovimento = 3f; // equivale ao momento (impulso)
    Vector2 Movimento = new Vector2(); // detectar movimento pelo teclado

    Rigidbody2D rb2D; // guarda o componente corpo rigido do player
    string estadoAnimacao = "EstadoAnimacao"; // variavel que guarda o nome do parametro de Animacao
    Animator animator;

    enum EstadosCaractere
    {
        andaLeste = 1,
        andaOeste = 2,
        andaNorte = 3,
        andaSul = 4,
        idle = 5,
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>(); // obtem o componete corpo rigido do player
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEstado();
    }

    private void FixedUpdate()
    {
        MoveCaractere();
    }

    void MoveCaractere()
    {
        Movimento.x = Input.GetAxisRaw("Horizontal");
        Movimento.y = Input.GetAxisRaw("Vertical");
        Movimento.Normalize();
        rb2D.velocity = Movimento * VelocidadeMovimento;
    }

    private void UpdateEstado()
    {
        if(Movimento.x > 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaLeste);
        }
        else if(Movimento.x < 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaOeste);
        }
        else if (Movimento.y > 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaNorte);
        }
        else if (Movimento.y < 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaSul);
        }
        else
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.idle);
        }
    }
}
