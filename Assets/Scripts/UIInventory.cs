using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIInventory : MonoBehaviour
{
    public static UIInventory Instance;
    // Singleton para manejar la UI del inventario.

    [Header("Asignar en Inspector (escena 1)")]
    public GameObject itemTMPPrefab;      // Prefab del texto (TextMeshPro) para mostrar los items.
    public Transform contentParent;       // Contenedor donde se instancian los textos.

    // Guarda referencias a cada entrada generada.
    private Dictionary<string, TextMeshProUGUI> itemEntries = new Dictionary<string, TextMeshProUGUI>();

    private void Awake()
    {
        // Configurar Singleton, evitando duplicados entre escenas.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Que no destruya al cambiar de escena.
        DontDestroyOnLoad(transform.root.gameObject);

        // Suscribir evento de carga de escena.
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnEnable()
    {
        // Asegurar la suscripción del evento.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Desuscribirse cuando se desactive.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void Start()
    {
        // Si aún no tiene parent asignado, intentar encontrarlo.
        if (contentParent == null)
            TryFindContentInScene();
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"UIInventory: escena cargada: {scene.name} -> intentando reconectar Content...");

        // En los menús no se muestra el inventario.
        if (scene.name == "MenuInicial" || scene.name == "MainMenu" || scene.name.Contains("Menu"))
        {
            gameObject.SetActive(false);
            return;
        }

        // En escenas de juego sí se muestra.
        gameObject.SetActive(true);

        TryFindContentInScene();
    }

    private void TryFindContentInScene()
    {
        // 1) Buscar objeto llamado exactamente "Content".
        GameObject found = GameObject.Find("Content");
        if (found != null)
        {
            contentParent = found.transform;
            Debug.Log("UIInventory: Encontrado Content por nombre 'Content'.");
            RefreshUIEntriesToNewParent();
            return;
        }

        // 2) Buscar dentro de los Canvas un layout adecuado.
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (var c in canvases)
        {
            Transform candidate = FindChildWithLayout(c.transform);
            if (candidate != null)
            {
                contentParent = candidate;
                Debug.Log($"UIInventory: Encontrado Content dentro del Canvas '{c.name}' (path: {GetHierarchyPath(candidate)}).");
                RefreshUIEntriesToNewParent();
                return;
            }
        }

        // 3) No encontrado → crear UI de emergencia.
        Debug.LogWarning("UIInventory: No se encontró 'Content' en la escena. Creando un panel temporal para el inventario.");
        CreateFallbackUI();
    }

    private Transform FindChildWithLayout(Transform root)
    {
        // Busca recursivamente un child con layout.
        foreach (Transform child in root)
        {
            if (child.GetComponent<VerticalLayoutGroup>() != null || child.GetComponent<ContentSizeFitter>() != null)
                return child;

            Transform rec = FindChildWithLayout(child);
            if (rec != null) return rec;
        }
        return null;
    }

    private void CreateFallbackUI()
    {
        // Crea Canvas de emergencia si la escena no tiene UI compatible.
        GameObject canvasGO = new GameObject("UIInventory_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Crear panel contenedor.
        GameObject panel = new GameObject("InventoryPanel");
        panel.transform.SetParent(canvasGO.transform, false);
        RectTransform panelRT = panel.AddComponent<RectTransform>();
        Image img = panel.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.25f); // Fondo semitransparente.
        panelRT.anchorMin = new Vector2(0.5f, 1f);
        panelRT.anchorMax = new Vector2(0.5f, 1f);
        panelRT.pivot = new Vector2(0.5f, 1f);
        panelRT.sizeDelta = new Vector2(300, 80);
        panelRT.anchoredPosition = new Vector2(0, -10);

        // Crear contenedor Content interno.
        GameObject content = new GameObject("Content");
        content.transform.SetParent(panel.transform, false);
        RectTransform contentRT = content.AddComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0, 0);
        contentRT.anchorMax = new Vector2(1, 1);
        contentRT.sizeDelta = Vector2.zero;
        VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;
        ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        contentParent = content.transform;
        Debug.Log("UIInventory: Fallback UI creado y asignado como Content.");
        RefreshUIEntriesToNewParent();
    }

    private void RefreshUIEntriesToNewParent()
    {
        // Crear nuevamente toda la UI del inventario según el snapshot del InventoryManager.
        if (itemTMPPrefab == null)
        {
            Debug.LogError("UIInventory: itemTMPPrefab NO está asignado en el Inspector.");
            return;
        }

        // Borrar viejos elementos.
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Recrear entradas según el inventario actual.
        if (InventoryManager.Instance != null)
        {
            var currentItems = InventoryManager.Instance.GetAllItemsSnapshot();
            if (currentItems != null)
            {
                foreach (var kv in currentItems)
                {
                    GameObject go = Instantiate(itemTMPPrefab, contentParent);
                    TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
                    if (tmp != null)
                        tmp.text = $"{kv.Key} x{kv.Value.ToString("D2")}";
                }
            }
        }
    }

    public void UpdateItem(string itemName, int count)
    {
        // Actualiza un solo item, recreando su entrada en UI.
        if (itemTMPPrefab == null)
        {
            Debug.LogError("UIInventory: itemTMPPrefab NO está asignado.");
            return;
        }

        // Si el contentParent aún no existe, buscarlo.
        if (contentParent == null)
        {
            Debug.LogWarning("UIInventory: contentParent aún no encontrado. Intentando otra búsqueda rápida...");
            TryFindContentInScene();
            if (contentParent == null)
            {
                Debug.LogError("UIInventory: NO hay contentParent asignado. No puedo mostrar el item.");
                return;
            }
        }

        // Si ya existe una entrada del item, eliminarla.
        foreach (Transform child in contentParent)
        {
            var tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null && tmp.text.StartsWith(itemName))
            {
                Destroy(child.gameObject);
                break;
            }
        }

        // Crear nueva entrada actualizada.
        GameObject newItem = Instantiate(itemTMPPrefab, contentParent);
        TextMeshProUGUI textComp = newItem.GetComponent<TextMeshProUGUI>();
        if (textComp != null)
            textComp.text = $"{itemName} x{count.ToString("D2")}";
    }

    // Limpia la UI del inventario (sin borrar datos del InventoryManager)
    public void ClearUI()
    {
        if (contentParent == null) return;
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
    }

    // Función auxiliar para debug: muestra el path completo del objeto en la jerarquía.
    private string GetHierarchyPath(Transform t)
    {
        if (t == null) return "";
        string path = t.name;
        Transform cur = t.parent;
        while (cur != null)
        {
            path = cur.name + "/" + path;
            cur = cur.parent;
        }
        return path;
    }
}
