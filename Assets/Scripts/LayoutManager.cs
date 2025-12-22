using UnityEngine;
using System.Collections.Generic;

public class LayoutManager : MonoBehaviour
{
    [Header("Referanslar")]
    [Tooltip("Hiyerarþideki 'Flags' ana objesini buraya sürükleyin.")]
    public GameObject flagsParent;

    [Tooltip("Hiyerarþideki 'Landmarks' ana objesini buraya sürükleyin.")]
    public GameObject landmarksParent;

    [Header("Konum ve Mesafe Ayarlarý")]
    public float startX = -1.5f;       // Ýlk objenin baþlayacaðý X pozisyonu
    public float flagY = 2.0f;         // Bayraklarýn ekranýn üstündeki Y yüksekliði
    public float landmarkY = -1.0f;    // Modellerin ekranýn altýndaki Y yüksekliði
    public float spacing = 0.8f;       // Objeler arasýndaki yatay boþluk

    [Header("Boyut (Scale) Ayarlarý")]
    public Vector3 flagScale = new Vector3(0.5f, 0.3f, 0.02f); // Bayraklarýn boyutu
    public Vector3 landmarkScale = new Vector3(1.0f, 1.0f, 1.0f); // Modellerin boyutu

    void Start()
    {
        // Oyun baþladýðýnda otomatik olarak düzenle
        ArrangeLayout();
    }

    /// <summary>
    /// Bayraklarý ve modelleri rastgele karýþtýrýr ve belirlenen düzende dizer.
    /// </summary>
    public void ArrangeLayout()
    {
        if (flagsParent == null || landmarksParent == null)
        {
            Debug.LogError("Lütfen LayoutManager üzerindeki Parent objelerini atayýn!");
            return;
        }

        // 1. Çocuk objeleri (Flag_TR, Landmark_TR vb.) listelere al
        List<Transform> flags = GetChildren(flagsParent);
        List<Transform> landmarks = GetChildren(landmarksParent);

        // 2. Listeleri rastgele karýþtýr (Her oyun baþýnda farklý dizilim için)
        ShuffleList(flags);
        ShuffleList(landmarks);

        // 3. Bayraklarý üst sýraya diz ve boyutlandýr
        for (int i = 0; i < flags.Count; i++)
        {
            flags[i].localPosition = new Vector3(startX + (i * spacing), flagY, 0);
            flags[i].localScale = flagScale;
        }

        // 4. Modelleri alt sýraya diz ve boyutlandýr
        for (int i = 0; i < landmarks.Count; i++)
        {
            landmarks[i].localPosition = new Vector3(startX + (i * spacing), landmarkY, 0);
            landmarks[i].localScale = landmarkScale;
        }

        Debug.Log("Layout baþarýyla oluþturuldu ve objeler karýþtýrýldý.");
    }

    // Yardýmcý Fonksiyon: Parent objesinin altýndaki tüm çocuklarý Transform listesi olarak döner
    private List<Transform> GetChildren(GameObject parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child);
        }
        return children;
    }

    // Yardýmcý Fonksiyon: Listeyi Fisher-Yates algoritmasýyla karýþtýrýr
    private void ShuffleList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}