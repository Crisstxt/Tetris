using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfazMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoDificultad, textoAltura, textoAncho, textoProf;

    private void OnEnable()
    {
        Menu.DificultadCambiada += DificultadCambiada;
        Menu.AlturaCambiada += AlturaCambiada;
        Menu.AnchoCambiado += AnchoCambiado;
        Menu.ProfCambiada += ProfCambiada;
    }

    private void OnDisable()
    {
        Menu.DificultadCambiada -= DificultadCambiada;
        Menu.AlturaCambiada -= AlturaCambiada;
        Menu.AnchoCambiado -= AnchoCambiado;
        Menu.ProfCambiada -= ProfCambiada;
    }

    private void ProfCambiada(float prof)
    {
        textoProf.text = $"{prof}";
    }

    private void DificultadCambiada(string dif)
    {
        textoDificultad.text = dif;
    }

    private void AnchoCambiado(int ancho)
    {
        textoAncho.text = $"{ancho}";
    }
    private void AlturaCambiada(int altura)
    {
        textoAltura.text = $"{altura}";
    }
}
