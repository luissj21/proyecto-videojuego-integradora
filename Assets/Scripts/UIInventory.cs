using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIInventory : MonoBehaviour
{
    public static UIInventory Instance;

    [Header("Asignar en Inspector (escena 1)")]
    public GameObject itemTMPPrefab;      // prefab con TextMeshProUGUI
    public Transform contentParent;       // opcional: asignado en escena 1

    // internal
    private Dictionary<string, TextMeshProUGUI> itemEntries = new Dictionary<string, TextMeshProUGUI>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(transform.root.gameObject);

        // Re-suscribir siempre el evento
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void Start()
    {
        if (contentParent == null)
            TryFindContentInScene();
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"UIInventory: escena cargada: {scene.name} -> intentando reconectar Content...");

        // --- OCULTAR INVENTARIO EN MENÚS ---
        if (scene.name == "MenuInicial" || scene.name == "MainMenu" || scene.name.Contains("Menu"))
        {
            gameObject.SetActive(false);
            return;
        }

        // --- MOSTRAR INVENTARIO EN ESCENAS DE JUEGO ---
        gameObject.SetActive(true);


        TryFindContentInScene();
    }

    private void TryFindContentInScene()
    {
        // 1) Busca por nombre exacto "Content"
        GameObject found = GameObject.Find("Content");
        if (found != null)
        {
            contentParent = found.transform;
            Debug.Log("UIInventory: Encontrado Content por nombre 'Content'.");
            RefreshUIEntriesToNewParent();
            return;
        }

        // 2) Busca dentro de todos los Canvas por un hijo que tenga VerticalLayoutGroup o ContentSizeFitter
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

        // 3) No encontrado: creamos un Canvas/Panel/Content mínimo para que el inventario no desaparezca
        Debug.LogWarning("UIInventory: No se encontró 'Content' en la escena. Creando un panel temporal para el inventario.");
        CreateFallbackUI();
    }

    private Transform FindChildWithLayout(Transform root)
    {
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
        // Crear Canvas raíz
        GameObject canvasGO = new GameObject("UIInventory_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Crear Panel
        GameObject panel = new GameObject("InventoryPanel");
        panel.transform.SetParent(canvasGO.transform, false);
        RectTransform panelRT = panel.AddComponent<RectTransform>();
        Image img = panel.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.25f);
        panelRT.anchorMin = new Vector2(0.5f, 1f);
        panelRT.anchorMax = new Vector2(0.5f, 1f);
        panelRT.pivot = new Vector2(0.5f, 1f);
        panelRT.sizeDelta = new Vector2(300, 80);
        panelRT.anchoredPosition = new Vector2(0, -10);

        // Crear Content (dentro del panel)
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
        if (itemTMPPrefab == null)
        {
            Debug.LogError("UIInventory: itemTMPPrefab NO está asignado en el Inspector. Asigna tu prefab de TextMeshPro.");
            return;
        }

        // limpiar hijos antiguos en el nuevo parent
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // recrear todas las entradas guardadas en InventoryManager
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
        if (itemTMPPrefab == null)
        {
            Debug.LogError("UIInventory: itemTMPPrefab NO está asignado en el Inspector. Asigna tu prefab de TextMeshPro.");
            return;
        }

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

        // Si ya existe una entrada para ese item, la borramos (reemplazamos)
        foreach (Transform child in contentParent)
        {
            var tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null && tmp.text.StartsWith(itemName))
            {
                Destroy(child.gameObject);
                break;
            }
        }

        GameObject newItem = Instantiate(itemTMPPrefab, contentParent);
        TextMeshProUGUI textComp = newItem.GetComponent<TextMeshProUGUI>();
        if (textComp != null)
            textComp.text = $"{itemName} x{count.ToString("D2")}";
    }

    // Limpia la UI (opcional)
    public void ClearUI()
    {
        if (contentParent == null) return;
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
    }

    // --- Implementación de GetHierarchyPath (para logging) ---
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


//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class UIInventory : MonoBehaviour
//{
//    public static UIInventory Instance;

//    public Transform contentParent;
//    public GameObject itemTMPPrefab;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);

//            SceneManager.sceneLoaded += OnSceneLoaded; // Detecta cuando cambia de escena
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        // Buscar el nuevo contentParent en cada escena
//        GameObject newContent = GameObject.Find("Content");

//        if (newContent != null)
//        {
//            contentParent = newContent.transform;
//        }
//        else
//        {
//            Debug.LogWarning("UIInventory: No se encontró el objeto 'Content' en esta escena.");
//        }
//    }

//    public void UpdateItem(string itemName, int count)
//    {
//        if (contentParent == null)
//        {
//            Debug.LogError("UIInventory: NO hay contentParent asignado en la escena actual.");
//            return;
//        }

//        // Limpiar el panel
//        foreach (Transform child in contentParent)
//            Destroy(child.gameObject);

//        // Crear texto
//        var newItem = Instantiate(itemTMPPrefab, contentParent);
//        newItem.GetComponent<TextMeshProUGUI>().text = $"{itemName} x{count}";
//    }
//}
