using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ChangeLanguage : MonoBehaviour
{
    bool isChanging = false; // Evita cambiar idioma múltiples veces al mismo tiempo

    public void SetLanguage(string localeIdentifier)
    {
        if (isChanging) return; // Si ya está cambiando, ignorar
        StartCoroutine(Change(localeIdentifier)); // Arrancar corrutina
    }

    private System.Collections.IEnumerator Change(string localeIdentifier)
    {
        isChanging = true;

        // Espera a que Unity cargue bien el sistema de localización
        yield return LocalizationSettings.InitializationOperation;

        // Buscar el idioma dentro de la lista de idiomas configurados
        Locale selectedLocale = null;
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == localeIdentifier)
            {
                selectedLocale = locale;
                break;
            }
        }

        // Si lo encuentra, cambiar idioma
        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
        else
        {
            Debug.LogError("Idioma no encontrado: " + localeIdentifier);
        }

        isChanging = false;
    }
}
