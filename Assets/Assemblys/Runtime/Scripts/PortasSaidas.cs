using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PortasSaidas : MonoBehaviour
{
    public string EntrandoNaSala;
    public List<Transform> PortasDaCena;
    public static PortasSaidas instance;
    public float TempoMovimentoForcado = .5f;
    public float Subida_forca = 8f;
    public float Subida_tempo = .2f;
    public bool E_Instance;
    public string SaindoDaSala;

    [Header("Direção de chegada")]
    public Tipo TipoDePorta;
    public enum Tipo { Direita, Esquerda, SubirD, SubirE, Cair, Entrar }


    private void Awake()
    {        
        SaindoDaSala = SceneManager.GetActiveScene().name;
        if (instance == null) 
        {  
            instance = this; E_Instance = true;
            if (!PortasDaCena.Contains(transform))
            {
                PortasDaCena.Add(transform);
            }
        } 
        else
        { 
            E_Instance = false;
            if (!PortasSaidas.instance.PortasDaCena.Contains(transform))
            {
                PortasSaidas.instance.PortasDaCena.Add(transform);
            }
        }
    }
}
