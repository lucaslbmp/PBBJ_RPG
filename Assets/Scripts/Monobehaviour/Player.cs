using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Caractere
{
    public Inventario inventarioPrefab; // referencia ao objeto prefab criado do Inventario
    Inventario inventario;
    public HealthBar healthBarPrefab; // referência ao objeto prefab criado para a healthbar
    HealthBar healthBar;
    bool DeveDesaparecer;

    private void Start()
    {
        inventario = Instantiate(inventarioPrefab);
        pontosDano.valor = inicioPontosDano;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.caractere = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Coletavel"))
        {
            Item DanoObjeto = collision.gameObject.GetComponent<Consumable>().item;
            if(DanoObjeto != null)
            {
                print("Acertou " + DanoObjeto.NomeObjeto);
                switch (DanoObjeto.tipoItem)
                {
                    case Item.TipoItem.MOEDA:
                        //DeveDesaparecer = true;
                        DeveDesaparecer = inventario.AddItem(DanoObjeto);
                        break;
                    case Item.TipoItem.HEALTH:
                        DeveDesaparecer = AjustePontosDano(DanoObjeto.quantidade);
                        break;
                    default:
                        break;
                }
                if (DeveDesaparecer)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    public bool AjustePontosDano(int quantidade)
    {
        if (pontosDano.valor < MaxPontosDano)
        {
            pontosDano.valor += quantidade;
            print("Ajustando pontos de dano por" + quantidade + " - Novo valor: " + pontosDano.valor);
            return true;
        }
        else return false;
    }
}
