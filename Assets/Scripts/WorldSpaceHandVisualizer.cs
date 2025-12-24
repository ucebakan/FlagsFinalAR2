using UnityEngine;

public class WorldSpaceHandVisualizer : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject jointPrefab; // Eklemler icin kullanacagimiz kucuk top
    public float targetDepth = 10.0f; // Bayraklarin oldugu derinlik (Canvas Z)
    public Camera mainCamera;

    // 21 adet eklem noktasi (Parmaklar)
    private GameObject[] handJoints = new GameObject[21];
    private GameObject uiPointList;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // 21 tane eklem (top) olustur ve sahneye koy
        for (int i = 0; i < 21; i++)
        {
            if (jointPrefab != null)
            {
                handJoints[i] = Instantiate(jointPrefab, transform);
                handJoints[i].name = "Joint_" + i;
                handJoints[i].SetActive(false); // Basta gizle
            }
        }
    }

    void LateUpdate()
    {
        // 1. MediaPipe'in cizdigi 2D noktalari bul
        if (uiPointList == null)
        {
            GameObject foundList = GameObject.Find("Point List Annotation");
            if (foundList != null && foundList.transform.childCount >= 21)
            {
                uiPointList = foundList;
            }
        }

        // 2. Noktalar bulunduysa 3D toplari oraya isinla
        if (uiPointList != null)
        {
            for (int i = 0; i < 21; i++)
            {
                // UI'daki 2D noktayi al
                Transform uiJoint = uiPointList.transform.GetChild(i);

                if (uiJoint.gameObject.activeInHierarchy)
                {
                    handJoints[i].SetActive(true);

                    // Ayni "FingerAutoTagger" mantigiyla 3D'ye cevir
                    Vector3 screenPixel = mainCamera.WorldToScreenPoint(uiJoint.position);

                    // Derinlik hesabi (Projection)
                    float distanceFromCamera = targetDepth - mainCamera.transform.position.z;
                    Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPixel.x, screenPixel.y, distanceFromCamera));

                    handJoints[i].transform.position = worldPos;
                }
                else
                {
                    handJoints[i].SetActive(false);
                }
            }
        }
    }
}