using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interfaz : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPuntuacion;

    private void OnEnable()
    {
        Tetris.PuntuacionActualizada += PuntuacionActualizada;
    }

    private void OnDisable()
    {
        Tetris.PuntuacionActualizada -= PuntuacionActualizada;
    }

    private void PuntuacionActualizada(int puntos)
    {
        txtPuntuacion.text = $"Puntos: {puntos}";
    }
}
