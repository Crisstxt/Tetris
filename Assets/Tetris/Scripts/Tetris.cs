using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tetris : MonoBehaviour
{
    public static Tetris instance;
    public GameObject[][] tablero;
    [Range(4, 20)] public int ancho = 20;
    [Range(10, 22)] public int alto = 22;
    public float velocidad = 1, profundidad = 1;
    private bool pausado;
    public bool locura;

    private int puntuacion; 

    public delegate void CambiarPuntiacion(int puntos);
    public static event CambiarPuntiacion PuntuacionActualizada;

    [SerializeField] private Button btnReiniciar, btnVolverMenu;
    [SerializeField] private TMPro.TextMeshProUGUI txtFinJuego;

    public AudioSource rotar, mover, limpiarFila;
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1;
        LeerDatos();

        CrearTablero();
        CrearPiezas();
        PosicionarCamara();
        puntuacion = 0;
        pausado = false;
        PuntuacionActualizada?.Invoke(puntuacion);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Pausar();
    }

    private void Pausar()
    {
        if (!pausado)
        {
            pausado = true;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void CrearTablero()
    {
        tablero = new GameObject[alto][];

        for (int i = 0; i < tablero.Length; i++)
        {
            tablero[i] = new GameObject[ancho];
        }

        for (int i = 0; i < alto; i++)
        {
            for (int j = 0; j < ancho; j++)
            {
                if (j == 0 || j == ancho - 1)
                {
                    GameObject nuevoCubo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    nuevoCubo.transform.position = new (j, i, 0);
                    tablero[i][j] = nuevoCubo;
                }
                else if (i == 0)
                {
                    GameObject nuevoCubo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    nuevoCubo.transform.position = new (j, i, 0);
                    nuevoCubo.name = "Suelo" + j;
                    tablero[i][j] = nuevoCubo;
                }
            }
        }
    }

    public void CrearPiezas()
    {
        new GameObject().AddComponent<Piezas>();
    }

    public void PosicionarCamara()
    {
        Vector3 posicionCamara = new (ancho/2, alto/2,0);
        float distanciaCamara = alto + (ancho / Camera.main.aspect);
        Camera.main.transform.position = posicionCamara - Vector3.forward * distanciaCamara;
    }

    public void AumentarVelocidad()
    {
        if(velocidad >= 0.32f) velocidad -= 0.02f;
    }

    public void AumentarPuntuacion()
    {
        float multiplicador = 1.0f / (velocidad + 1.0f) * 2f;
        puntuacion += (int)(ancho * multiplicador);

        PuntuacionActualizada?.Invoke(puntuacion);

        if (locura)
            VelocidadRandom(UnityEngine.Random.Range(0.15f, 0.3f));
        else
            AumentarVelocidad();
    }

    public void AumentarPuntuacion(int puntosExtra)
    {
        puntuacion += puntosExtra;
        PuntuacionActualizada?.Invoke(puntuacion);
    }

    public void FinJuego()
    {
        txtFinJuego.gameObject.SetActive(true);
        btnReiniciar.gameObject.SetActive(true);
        btnVolverMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReiniciarPartida()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReproducirRotar()
    {
        rotar.Play();
    }
    public void ReproducirMover()
    {
        mover.Play();
    }

    public void ReproducirLimpiarFila()
    {
        limpiarFila.Play();
    }

    public void VolverMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void EstablecerVelocidad(string dif)
    {
        switch (dif)
        {
            case "Facil":
                velocidad = 1.2f;
                break;
            case "Medio":
                velocidad = 0.8f;
                break;
            case "Dificil":
                velocidad = 0.6f;
                break;
            case "Locura":
                VelocidadRandom(UnityEngine.Random.Range(0.15f, 0.3f));
                locura = true;
                break;
        }
    }

    public void VelocidadRandom(float random)
    {
        velocidad = random;
    }

    public void LeerDatos()
    {
        EstablecerVelocidad(PlayerPrefs.GetString("Dificultad"));
        alto = PlayerPrefs.GetInt("Altura") + 1;
        ancho = PlayerPrefs.GetInt("Ancho") + 2;
        profundidad = PlayerPrefs.GetFloat("Prof");
    }
}