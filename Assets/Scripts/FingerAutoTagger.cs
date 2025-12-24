using UnityEngine;

public class FingerAutoTagger : MonoBehaviour
{
    [Header("Assignments")]
    public GameObject fingerProxy;
    public Camera mainCamera; // Kamerayi elle sececegiz
    public float targetDepth = 10.0f; // Bayraklarin Z pozisyonu (Genelde 10)

    private GameObject uiPoint;

    void Start()
    {
        // Eger editor'den atamayi unutursan otomatik bulsun
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // 1. MediaPipe noktasini bul
        if (uiPoint == null)
        {
            GameObject pointList = GameObject.Find("Point List Annotation");
            if (pointList != null && pointList.transform.childCount >= 9)
            {
                uiPoint = pointList.transform.GetChild(8).gameObject;
            }
        }

        // 2. Takibi Gerceklestir (PROJECTION YONTEMI)
        if (uiPoint != null && fingerProxy != null && mainCamera != null)
        {
            // A. Elinin 3D pozisyonunu "Ekran Noktasina" (Piksel) cevir
            Vector3 screenPixel = mainCamera.WorldToScreenPoint(uiPoint.transform.position);

            // B. Kameradan hedefe olan mesafeyi hesapla
            // (Ornek: Bayrak Z=10, Kamera Z=-20 ise Mesafe = 30)
            float distanceFromCamera = targetDepth - mainCamera.transform.position.z;

            // C. O pikseli, hesaplanan mesafe kadar ileriye "Isinla"
            Vector3 finalWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPixel.x, screenPixel.y, distanceFromCamera));

            fingerProxy.transform.position = finalWorldPos;
        }
    }
}