using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int genislik = 8;
    public int yukseklik = 8;
    public GameObject hucrePrefab; 
    private GameObject[,] gridDizisi;

    void Start()
    {
        GridiOlustur();
    }

    void GridiOlustur()
    {
        gridDizisi = new GameObject[genislik, yukseklik];
        for (int y = 0; y < yukseklik; y++)
        {
            for (int x = 0; x < genislik; x++)
            {
                GameObject yeniHucre = Instantiate(hucrePrefab, new Vector3(x, y, 0), Quaternion.identity);
                yeniHucre.transform.parent = transform;
                gridDizisi[x, y] = yeniHucre;
               
            }
        }
    }
}