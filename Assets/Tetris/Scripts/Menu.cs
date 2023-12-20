using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject fondo;
    [SerializeField] private Button btnJuego, btnOpciones, btnVolver, btnPuntuacion;
    private Color[] colores = {Color.blue, Color.green, Color.red, Color.gray, Color.cyan};

    [SerializeField] private Slider sldDificultad, sldAncho, sldAltura, sldProfundidad;

    private string dificultad;
    private int altura, ancho;
    private float prof;

    public delegate void CambiarAltura(int altura);
    public static event CambiarAltura AlturaCambiada;

    public delegate void CambiarAncho(int ancho);
    public static event CambiarAncho AnchoCambiado;

    public delegate void CambiarDificultad(string dif);
    public static event CambiarDificultad DificultadCambiada;

    public delegate void CambiarProfundidad(float prof);
    public static event CambiarProfundidad ProfCambiada;


    void Start()
    {
        dificultad = "";
        altura = 10;
        ancho = 4;
        prof = 1;
        CambiarColorFondo();

        sldDificultad?.onValueChanged.AddListener(Dificultad);
        sldAncho?.onValueChanged.AddListener(Ancho);
        sldAltura?.onValueChanged.AddListener(Altura);
        sldProfundidad?.onValueChanged.AddListener(Profunidad);
    }


    public void Jugar()
    {
        PlayerPrefs.SetString("Dificultad", dificultad);
        PlayerPrefs.SetInt("Altura", altura);
        PlayerPrefs.SetInt("Ancho", ancho);
        PlayerPrefs.SetFloat("Prof", prof);

        SceneManager.LoadScene("JuegoPrincipal");
    }

    private void Dificultad(float value)
    {
        int dif = (int)value;

        switch (dif)
        {
            case 1:
                dificultad = "Facil";
                break;
            case 2:
                dificultad = "Medio";
                break;
            case 3:
                dificultad = "Dificil";
                break;
            case 4:
                dificultad = "Locura";
                break;
        }

        DificultadCambiada?.Invoke(dificultad);
    }
    
    private void Altura(float value)
    {
        altura = (int)value;
        AlturaCambiada?.Invoke(altura);
    }

    private void Ancho(float value)
    {
        ancho = (int)value;
        AnchoCambiado?.Invoke(ancho);
    }

    private void Profunidad(float value)
    {
        prof = value;
        ProfCambiada?.Invoke(prof);
    }
    public void Opciones()
    {
        btnJuego.gameObject.SetActive(false);
        btnOpciones.gameObject.SetActive(false);
        //btnPuntuacion.gameObject.SetActive(false);
        sldDificultad.gameObject.SetActive(true);
        sldAncho.gameObject.SetActive(true);
        sldAltura.gameObject.SetActive(true);
        sldProfundidad.gameObject.SetActive(true);
        btnVolver.gameObject.SetActive(true);
    }

    public void Volver()
    {
        btnJuego.gameObject.SetActive(true) ;
        btnOpciones.gameObject.SetActive(true);
        //btnPuntuacion.gameObject.SetActive(true);
        sldDificultad.gameObject.SetActive(false);
        sldAncho.gameObject.SetActive(false);
        sldAltura.gameObject.SetActive(false);
        sldProfundidad.gameObject.SetActive(false);
        btnVolver.gameObject.SetActive(false);
    }

    private void CambiarColorFondo()
    {
        fondo.GetComponent<Renderer>().material.color = Color.gray;
        StartCoroutine(CambiarColor());

    }

    IEnumerator CambiarColor()
    {
        yield return new WaitForSeconds(7.3f);

        int cont = 0;
        while (true)
        {
            fondo.GetComponent<Renderer>().material.color = colores[cont];

            cont = (cont + 1) % colores.Length;
              
            yield return new WaitForSeconds(1.3f);
        }
    }
}
