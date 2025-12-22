using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [Header("Dönüþ Ayarlarý")]
    public float rotationSpeed = 30f; // Saniyedeki dönüþ derecesi
    public Vector3 rotationAxis = Vector3.up; // Hangi eksende döneceði (Y ekseni varsayýlan)

    void Update()
    {
        // Her karede (frame) objeyi belirlediðimiz eksende döndürür
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}