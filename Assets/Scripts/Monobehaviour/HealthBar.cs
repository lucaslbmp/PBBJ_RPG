using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PontosDano pontosDano; // Objeto que fara a leitura de quanros pontos tem o Player
    public Player caractere; // recebera o objeto player
    public Image medidorImagem; // recebe a barra de mediçao
    public Text pdtexto; // recebe os dados de PD
    float maxPontosDano; // armazena a quantidade limite de "saude" do player

    private void Start()
    {
        maxPontosDano = caractere.MaxPontosDano;
    }

    private void Update()
    {
        if(caractere != null)
        {
            medidorImagem.fillAmount = pontosDano.valor / maxPontosDano;
            pdtexto.text = "PD:" + (medidorImagem.fillAmount * 100);
        }
    }
}
