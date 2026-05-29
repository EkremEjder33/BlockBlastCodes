using UnityEngine;

public class BlokSurukle : MonoBehaviour
{
    private bool surukleniyor = false;
    private Vector3 baslangicPozisyonu;
    private float boslukDegeri = 1.05f; 

    void Start()
    {
        baslangicPozisyonu = transform.position;
    }

    void Update()
    {
        // YENİ: Eğer GridManager'da oyun bittiyse, bu bloğun sürüklenmesine asla izin verme
        if (GridManager.Instance != null && GridManager.Instance.OyunBittiMiGetir())
        {
            surukleniyor = false;
            return; // Kodun aşağıya inmesini engeller ve bloğu kilitler
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // Dar tıklama mesafesi (Sadece tam üzerine tıklanan tek blok tutmak için)
            if (Vector2.Distance(transform.position, mousePos) < 1f)
            {
                surukleniyor = true;
            }
        }

        if (surukleniyor)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0) && surukleniyor)
        {
            surukleniyor = false;
            YerlesimIzniniKontrolEt();
        }
    }

    void YerlesimIzniniKontrolEt()
    {
        Transform[] cocukKareler = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cocukKareler[i] = transform.GetChild(i);
        }

        bool yerlesebilirMi = true;

        foreach (Transform kare in cocukKareler)
        {
            // YENİ/GÜNCEL: L bloğunun üst parçasının taşmasını engelleyen hassas ofset hesaplaması.
            // + 0.01f, yuvarlamayı bir üst hücreye kaydırabilecek milimetrik sapmaları filtreler.
            float hassasX = (kare.position.x / boslukDegeri) + 0.01f;
            float hassasY = (kare.position.y / boslukDegeri) + 0.01f;
            int gx = Mathf.RoundToInt((float)System.Math.Round(hassasX, 1));
            int gy = Mathf.RoundToInt((float)System.Math.Round(hassasY, 1));

            // Sınır kontrollerini GridManager'daki dinamik genişlik/yükseklik ile yapıyoruz
            if (gx < 0 || gx >= GridManager.Instance.genislik || gy < 0 || gy >= GridManager.Instance.yukseklik || GridManager.Instance.gridMatrisi[gx, gy].doluMu)
            {
                yerlesebilirMi = false;
                break;
            }
        }

        if (yerlesebilirMi)
        {
            foreach (Transform kare in cocukKareler)
            {
                // YENİ/GÜNCEL: Hücre doldurulurken de aynı hassas koordinatlar kullanılıyor
                float hassasX = (kare.position.x / boslukDegeri) + 0.01f;
                float hassasY = (kare.position.y / boslukDegeri) + 0.01f;
                int gx = Mathf.RoundToInt((float)System.Math.Round(hassasX, 1));
                int gy = Mathf.RoundToInt((float)System.Math.Round(hassasY, 1));

                // GÜNCELLEME: Küçük karenin kendi rengini SpriteRenderer'dan çekiyoruz
                Color kareninKendiRengi = kare.GetComponent<SpriteRenderer>().color;

                // GridManager'a rengi ve koordinatları eksikosiz gönderiyoruz
                GridManager.Instance.HucreyiDoldur(gx, gy, kareninKendiRengi);
            }

            BlokUretici uretici = FindObjectOfType<BlokUretici>();
            if (uretici != null)
            {
                uretici.BlokYerlestirildi(gameObject);
            }

            Destroy(gameObject);
        }
        else
        {
            transform.position = baslangicPozisyonu;
        }
    }
}