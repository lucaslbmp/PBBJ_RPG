using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public GameObject slotPrefab; // objeto que recebe o prefab slot
    public const int numSlots = 5; //numero fixo de slots
    Image[] itemImagens = new Image[numSlots]; // array de imagens
    [HideInInspector] public Item[] itens = new Item[numSlots]; // array de itens
    GameObject[] slots = new GameObject[numSlots]; // array de Slots

    public Dictionary<string, int> itensColetados = new Dictionary<string, int>(); // dicionario de itens coletados pelo player na cena atual

    void Start()
    {
        CriaSlots();
        ResetInventario();
    }
    public void CriaSlots()
    {
        if(slotPrefab != null)
        {
            for(int i = 0; i < numSlots; i++)
            {
                GameObject novoSlot = Instantiate(slotPrefab);
                novoSlot.name = "ItemSlot_" + i;
                novoSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = novoSlot;
                itemImagens[i] = novoSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }


    public bool AddItem(Item itemToAdd)
    {
        for(int i=0; i<itens.Length; i++)
        {
            if(itens[i]!=null && itens[i].tipoItem == itemToAdd.tipoItem && itemToAdd.empilhavel == true)
            {
                itens[i].quantidade = itens[i].quantidade + 1;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantidadeTexto = slotScript.qtdTexto;
                quantidadeTexto.enabled = true;
                quantidadeTexto.text = itens[i].quantidade.ToString("00");
                AtualizarListaColetaveis(itemToAdd.tipoItem.ToString(), itens[i].quantidade);
                GameManager.ImprimeListaColetaveis(itensColetados);
                return true;
            }
            else if(itens[i] == null)
            {
                itens[i] = Instantiate(itemToAdd);
                itens[i].quantidade = 1;
                itemImagens[i].sprite = itemToAdd.sprite;
                itemImagens[i].enabled = true;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantidadeTexto = slotScript.qtdTexto;
                quantidadeTexto.enabled = true;
                quantidadeTexto.text = itens[i].quantidade.ToString("00");
                AtualizarListaColetaveis(itemToAdd.tipoItem.ToString());
                GameManager.ImprimeListaColetaveis(itensColetados);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < itens.Length; i++)
        {
            if(itens[i] != null)
            {
                if (itens[i].tipoItem == itemToRemove.tipoItem)
                {
                    itens[i] = null;
                    itemImagens[i].enabled = false;
                    Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                    Text quantidadeTexto = slotScript.qtdTexto;
                    quantidadeTexto.enabled = false;
                }
            }
        }
    }

    public void AtualizarListaColetaveis(string tipoColetavel)
    {
        if (!itensColetados.ContainsKey(tipoColetavel))
            itensColetados.Add(tipoColetavel, 1);
        else
            itensColetados.Remove(tipoColetavel);
    }

    public void AtualizarListaColetaveis(string tipoColetavel, int quantidade)
    {
        if (tipoColetavel != "HEALTH" && itensColetados.ContainsKey(tipoColetavel))
        {
            itensColetados[tipoColetavel] = quantidade;
        }
    }

    public void ResetInventario()
    {
        for (int i = 0; i < itens.Length; i++)
        {
            itens[i] = null;
        }
        itensColetados = new Dictionary<string, int>();
    }
}
