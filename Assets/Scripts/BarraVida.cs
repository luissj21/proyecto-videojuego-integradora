using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{

    public Image rellenoBarraVida;
    private NewMonoBehaviourScript playerController;
    private float vidaMaxima;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     playerController = GameObject.Find("Rambo").GetComponent<NewMonoBehaviourScript>(); 
        vidaMaxima = playerController.Health;
    }

    // Update is called once per frame
    void Update()
    {
        rellenoBarraVida.fillAmount = playerController.Health / vidaMaxima;
    }



}
