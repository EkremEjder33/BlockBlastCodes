using UnityEngine;

public class DraggableShape : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 baslangicPozisyonu;
    private bool surukleniyor = false;

    void Start() => baslangicPozisyonu = transform.position;

    void OnMouseDown()
    {
        offset = transform.position - KameraPozisyonu();
        surukleniyor = true;
    }

    void OnMouseDrag()
    {
        transform.position = KameraPozisyonu() + offset;
    }

    void OnMouseUp()
    {
        surukleniyor = false;
       
        transform.position = baslangicPozisyonu; 
    }

    Vector3 KameraPozisyonu()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}

 // 18,04,2026 burada kaldın blok yapıları