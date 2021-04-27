using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia a trajetoria do projetil (bolinha)
/// </summary>

public class Arco : MonoBehaviour
{
    // Corrotina que controla a trajetoria do projetil
    public IEnumerator arcoTrajetoria(Vector3 destino,float duracao)
    {
        var posicaoInicial = transform.position;            // Ajusta a posi�ao inicial como sendo a da muni�ao
        var percentualCompleto = 0.0f;                      // Inicializa em zero o percentual da trajetoria completado

        while (percentualCompleto < 1f)                     // Se o percentual da trajetoria completado for menor que 100%...
        {
            percentualCompleto += Time.deltaTime / duracao;     // A cada itera��o, incrementa o percentual da trajetoria que foi completado
            var alturaCorrente = Mathf.Sin(Mathf.PI * percentualCompleto);      // Varia a altura (posi�ao y) de forma senoidal para emular a subida e queda do projetil
            // A posi�ao do projetil recebe o resultado da interpola�ao entre a posi�ao inicial e o destino somado do vetor que adiciona a subida/descida
            transform.position = Vector3.Lerp(posicaoInicial,destino,percentualCompleto) + Vector3.up * alturaCorrente; 
            yield return null;              // Retorna null
        }
        gameObject.SetActive(false);            // Desativa o projetil
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
