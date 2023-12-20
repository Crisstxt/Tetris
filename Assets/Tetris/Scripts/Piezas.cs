using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piezas : MonoBehaviour
{
    private Tetris tetris;
    private GameObject cubo1, cubo2, cubo3, cubo4;
    private int random, PiezaActual;
    private bool bloqueada;


    void Start()
    {
        bloqueada = false;
        random = Random.Range(1, 6);
        tetris = Tetris.instance;
        InicializarCubos();
        PosPiezas(random);
        PiezaActual = random;
    }

    void Update()
    {

        if (!bloqueada)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) Mover(Vector3.right);

            if (Input.GetKeyDown(KeyCode.LeftArrow)) Mover(Vector3.left);

            if (Input.GetKey(KeyCode.UpArrow)) Mover(Vector3.down);

            if (Input.GetKeyDown(KeyCode.Space)) Rotar();
        }
        LimpiarFilas();
    }
    


    public void PosPiezas(int pieza)
    {
        switch (pieza)
        {
            case 1:
                PosicionarCubos(new(tetris.ancho / 2 - 1, tetris.alto - 1, 0), new(tetris.ancho / 2, tetris.alto - 1, 0),
                    new(tetris.ancho / 2 - 1, tetris.alto - 2, 0), new(tetris.ancho / 2, tetris.alto - 2, 0));
                break;
            case 2:
                PosicionarCubos(new(tetris.ancho / 2 - 1, tetris.alto - 1, 0), new(tetris.ancho / 2, tetris.alto - 1, 0),
                    new(tetris.ancho / 2 + 1, tetris.alto - 1, 0), new(tetris.ancho / 2, tetris.alto - 2, 0));
                break;
            case 3:
                PosicionarCubos(new(tetris.ancho / 2, tetris.alto - 1, 0), new(tetris.ancho / 2, tetris.alto - 2, 0),
                    new(tetris.ancho / 2, tetris.alto - 3, 0), new(tetris.ancho / 2 - 1, tetris.alto - 3, 0));
                break;
            case 4:
                PosicionarCubos(new(tetris.ancho / 2 + 1, tetris.alto - 1, 0), new(tetris.ancho / 2, tetris.alto - 1, 0),
                    new(tetris.ancho / 2, tetris.alto - 2, 0), new(tetris.ancho / 2 - 1, tetris.alto - 2, 0));
                break;
            case 5:
                PosicionarCubos(new(tetris.ancho / 2, tetris.alto - 1, 0), new(tetris.ancho / 2, tetris.alto - 2, 0),
                    new(tetris.ancho / 2, tetris.alto - 3, 0), new(tetris.ancho / 2, tetris.alto - 4, 0));
                break;
        }

        random = Random.Range(1, 6);

        AgregarColor(cubo1, random);
        AgregarColor(cubo2, random);
        AgregarColor(cubo3, random);
        AgregarColor(cubo4, random);

        StartCoroutine(Bajar());    
    }

    IEnumerator Bajar()
    {
        while (true)
        {
            if (!PuedeMover(Vector3.down))
            {
                BloquearPieza();
                yield break;
            }

            yield return new WaitForSeconds(tetris.velocidad);
            Mover(Vector3.down);
        }
    }

    public void InicializarCubos()
    {
        cubo1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo4 = GameObject.CreatePrimitive(PrimitiveType.Cube);

       cubo1.transform.localScale = new(1, 1, tetris.profundidad);
       cubo2.transform.localScale = new(1, 1, tetris.profundidad);
       cubo3.transform.localScale = new(1, 1, tetris.profundidad);
       cubo4.transform.localScale = new(1, 1, tetris.profundidad);
    }

    public void PosicionarCubos(Vector3 posCubo1, Vector3 posCubo2, Vector3 posCubo3, Vector3 posCubo4)
    {
        cubo1.transform.position = posCubo1;
        cubo2.transform.position = posCubo2;
        cubo3.transform.position = posCubo3;
        cubo4.transform.position = posCubo4;
    }

    public void Mover(Vector3 direccion)
    {
        if (PuedeMover(direccion))
        {
            cubo1.transform.position += direccion;
            cubo2.transform.position += direccion;
            cubo3.transform.position += direccion;
            cubo4.transform.position += direccion;
            tetris.ReproducirMover();
        } 
    }

    public bool PuedeMover(Vector3 direccion)
    {
        int cubo1_x = (int)cubo1.transform.position.x;
        int cubo1_y = (int)cubo1.transform.position.y;

        int cubo2_x = (int)cubo2.transform.position.x;
        int cubo2_y = (int)cubo2.transform.position.y;

        int cubo3_x = (int)cubo3.transform.position.x;
        int cubo3_y = (int)cubo3.transform.position.y;

        int cubo4_x = (int)cubo4.transform.position.x;
        int cubo4_y = (int)cubo4.transform.position.y;

        if (tetris.tablero[cubo1_y + (int)direccion.y][cubo1_x] != null || tetris.tablero[cubo2_y + (int)direccion.y][cubo2_x] != null ||
        tetris.tablero[cubo3_y + (int)direccion.y][cubo3_x] != null || tetris.tablero[cubo4_y + (int)direccion.y][cubo4_x] != null)
        {
            return false;
        }

        if (tetris.tablero[cubo1_y][cubo1_x + (int)direccion.x] != null || tetris.tablero[cubo2_y][cubo2_x + (int)direccion.x] != null ||
                tetris.tablero[cubo3_y][cubo3_x + (int)direccion.x] != null || tetris.tablero[cubo4_y][cubo4_x + (int)direccion.x] != null)
        {
            return false;
        }

        return true;

    }

    public void Rotar()
    {
        Vector3 pivot = cubo2.transform.position;

        if (PuedeRotar(pivot, cubo1) && PuedeRotar(pivot, cubo3) && PuedeRotar(pivot, cubo4))
        {
            RotarCubosPivot(cubo1, pivot);
            RotarCubosPivot(cubo3, pivot);
            RotarCubosPivot(cubo4, pivot);
            tetris.ReproducirRotar();
        }
    }

    public void RotarCubosPivot(GameObject cubo, Vector3 pivot)
    {
        Vector3 direccion = cubo.transform.position - pivot;
        Vector3 rotar = new(-direccion.y, direccion.x, 0);

        cubo.transform.position = pivot + rotar;
    }

    public bool PuedeRotar(Vector3 pivot, GameObject cubo)
    {

        Vector3 direccion = cubo.transform.position - pivot;
        Vector3 rotar = new(-direccion.y, direccion.x, 0);

        Vector3 nuevaPos = pivot + rotar;

        if ((int)nuevaPos.x < 0 || (int)nuevaPos.x >= tetris.ancho
            || (int)nuevaPos.y < 0 || (int)nuevaPos.y >= tetris.alto) return false;

        if (tetris.tablero[(int)nuevaPos.y][(int)nuevaPos.x] != null) return false;

        return true;
    }

    public void BloquearPieza()
    {
        bloqueada = true;
        AgregarAlTableroCubos();
        LimpiarFilas();

        if (!FinDelJuego())tetris.CrearPiezas();

           
        Destroy(gameObject.GetComponent<Piezas>());
    }

    public void AgregarAlTableroCubos()
    {
        tetris.tablero[(int)cubo1.transform.position.y][(int)cubo1.transform.position.x] = cubo1;
        tetris.tablero[(int)cubo2.transform.position.y][(int)cubo2.transform.position.x] = cubo2;
        tetris.tablero[(int)cubo3.transform.position.y][(int)cubo3.transform.position.x] = cubo3;
        tetris.tablero[(int)cubo4.transform.position.y][(int)cubo4.transform.position.x] = cubo4;
    }

    public void LimpiarFilas()
    {
        int cantidaFilas = 0;

        for(int i = 1; i < tetris.alto; i++)
        {
            for(int j = 1; j < tetris.ancho - 1; j++)
            {
                if (tetris.tablero[i][j] != null) cantidaFilas++;

                if (cantidaFilas == tetris.ancho - 2)
                {
                    DestruirFilas(i);
                    BajarPiezas();
                }
            }
            cantidaFilas = 0;
        }

    }

    public void DestruirFilas(int totalFilas)
    {
        Color similar = Color.blue; 
        int contador = 0;

        for (int j = 1; j < tetris.ancho - 2; j++)
        {
            if(j == 1) similar = tetris.tablero[totalFilas][j].GetComponent<Renderer>().material.color;

            if (j > 2 && similar.Equals(tetris.tablero[totalFilas][j].GetComponent<Renderer>().material.color)) contador++;

           Destroy(tetris.tablero[totalFilas][j]);
           tetris.tablero[totalFilas][j] = null;
        }

        if(contador == tetris.ancho - 2)
        {
            tetris.AumentarPuntuacion(tetris.ancho * 200);
        }

        tetris.AumentarPuntuacion();
        tetris.ReproducirLimpiarFila();
    }

    public void BajarPiezas()
    {
        for (int i = 1; i < tetris.alto; i++)
        {
            for (int j = 1; j < tetris.ancho - 1; j++)
            {
                if (tetris.tablero[i][j] != null)
                {
                    MoverPiezaHaciaAbajo(tetris.tablero[i][j]);
                }
            }
        }
    }

    public void MoverPiezaHaciaAbajo(GameObject cubo)
    {
        int cubo_x = (int)cubo.transform.position.x;
        int cubo_y = (int)cubo.transform.position.y;

        while (cubo_y > 0 && tetris.tablero[cubo_y - 1][cubo_x] == null)
        {
            cubo.transform.position += Vector3.down;
            tetris.tablero[cubo_y][cubo_x] = null;
            tetris.tablero[cubo_y - 1][cubo_x] = cubo;
            cubo_y--;
        }
    }

    public bool FinDelJuego()
    {
        if (bloqueada)
        {
            if (ComprobarLimite(cubo1) || ComprobarLimite(cubo2) ||
                ComprobarLimite(cubo3) || ComprobarLimite(cubo4))
            {
                tetris.FinJuego();
                return true;
            }
        }

        return false;
    }

    public bool ComprobarLimite(GameObject cubo)
    {
        if (cubo.transform.position.y >= tetris.alto - 1) return true;

        return false;
    }

    public void AgregarColor(GameObject cubo, int color)
    {
        switch (color)
        {
            case 1:
                cubo.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 2:
                cubo.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 3:
                cubo.GetComponent<Renderer>().material.color = Color.green;
                break;
            case 4:
                cubo.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case 5:
                cubo.GetComponent<Renderer>().material.color = Color.cyan;
                break;
        }
    }
}