using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int genislik = 8;
    public int yukseklik = 8;
    public GameObject hucrePrefab; 
    private float boslukDegeri = 1.05f;

    public HucreKimligi[,] gridMatrisi; 

    public TextMeshProUGUI puanYazisiKomponenti;
    public GameObject gameOverObjesi; // Sağ taraftaki "OYUN BİTTİ" yazısı
    public GameObject yenidenBaslatButonObjesi; // YENİ: Sol taraftaki "YENİDEN OYNA" butonu
    
    private int mevcutPuan = 0;
    private bool oyunBittiMi = false;

    void Awake() { Instance = this; }
    void Start() { GridiOlustur(); }

    void GridiOlustur()
    {
        gridMatrisi = new HucreKimligi[genislik, yukseklik];
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                GameObject yeniHucre = Instantiate(hucrePrefab, new Vector3(x * boslukDegeri, y * boslukDegeri, 0), Quaternion.identity);
                yeniHucre.transform.parent = transform; 
                yeniHucre.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
                yeniHucre.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.15f);

                HucreKimligi kimlik = yeniHucre.AddComponent<HucreKimligi>();
                kimlik.X = x;
                kimlik.Y = y;
                gridMatrisi[x, y] = kimlik;
            }
        }
        Camera.main.transform.position = new Vector3((genislik - 1) * boslukDegeri / 2f, (yukseklik - 1) * boslukDegeri / 2f, -10);
        
        // Oyun başında panellerin gizli olduğundan emin oluyoruz
        if (gameOverObjesi != null) gameOverObjesi.SetActive(false);
        if (yenidenBaslatButonObjesi != null) yenidenBaslatButonObjesi.SetActive(false); // GÜNCELLEME
    }

    public void HucreyiDoldur(int x, int y, Color blokRengi)
    {
        if (oyunBittiMi) return;

        if (x >= 0 && x < genislik && y >= 0 && y < yukseklik)
        {
            gridMatrisi[x, y].doluMu = true;
            SpriteRenderer sr = gridMatrisi[x, y].GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = blokRengi;
        }
        
        SatirlariKontrolEt(); 
        
        // Oyun bitiş kontrolünü 0.2 saniye gecikmeli çalıştırıyoruz. 
        Invoke("OyunBitisiniKontrolEt", 0.2f); 
    }

    void SatirlariKontrolEt()
    {
        List<int> patlayacakSatirlar = new List<int>();
        List<int> patlayacakSutunlar = new List<int>();

        for (int y = 0; y < yukseklik; y++)
        {
            bool satirDolu = true;
            for (int x = 0; x < genislik; x++)
            {
                if (gridMatrisi[x, y].doluMu == false) satirDolu = false;
            }
            if (satirDolu) patlayacakSatirlar.Add(y);
        }

        for (int x = 0; x < genislik; x++)
        {
            bool sutunDolu = true;
            for (int y = 0; y < yukseklik; y++)
            {
                if (gridMatrisi[x, y].doluMu == false) sutunDolu = false;
            }
            if (sutunDolu) patlayacakSutunlar.Add(x);
        }

        int toplamPatlamaSayisi = patlayacakSatirlar.Count + patlayacakSutunlar.Count;

        foreach (int satirIndex in patlayacakSatirlar) SatiriPatlat(satirIndex);
        foreach (int sutunIndex in patlayacakSutunlar) SutunuPatlat(sutunIndex);

        if (toplamPatlamaSayisi > 1)
        {
            mevcutPuan += 300;
            PuanYazisiniGuncelle();
        }
    }

    void SatiriPatlat(int satirIndex)
    {
        for (int x = 0; x < genislik; x++)
        {
            gridMatrisi[x, satirIndex].doluMu = false;
            gridMatrisi[x, satirIndex].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.15f);
        }
        mevcutPuan += 100;
        PuanYazisiniGuncelle();
    }

    void SutunuPatlat(int sutunIndex)
    {
        for (int y = 0; y < yukseklik; y++)
        {
            gridMatrisi[sutunIndex, y].doluMu = false;
            gridMatrisi[sutunIndex, y].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.15f);
        }
        mevcutPuan += 100;
        PuanYazisiniGuncelle();
    }

    public void OyunBitisiniKontrolEt()
    {
        BlokSurukle[] sahnedekiBloklar = FindObjectsOfType<BlokSurukle>();
        
        if (sahnedekiBloklar.Length == 0) return;

        foreach (BlokSurukle blok in sahnedekiBloklar)
        {
            if (BlokSigiyorMu(blok)) return; 
        }

        OyunBitti();
    }

    bool BlokSigiyorMu(BlokSurukle blok)
    {
        List<Vector2Int> parcaOfsetleri = new List<Vector2Int>();
        foreach (Transform parca in blok.transform)
        {
            int localX = Mathf.RoundToInt(parca.localPosition.x);
            int localY = Mathf.RoundToInt(parca.localPosition.y);
            parcaOfsetleri.Add(new Vector2Int(localX, localY));
        }

        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                bool yerlesebilirMi = true; 

                foreach (Vector2Int ofset in parcaOfsetleri)
                {
                    int hedefX = x + ofset.x;
                    int hedefY = y + ofset.y;

                    if (hedefX < 0 || hedefX >= genislik || hedefY < 0 || hedefY >= yukseklik || gridMatrisi[hedefX, hedefY].doluMu)
                    {
                        yerlesebilirMi = false; 
                        break;
                    }
                }

                if (yerlesebilirMi) return true; 
            }
        }
        return false;
    }

    void OyunBitti()
    {
        oyunBittiMi = true;
        
        // GÜNCELLEME: Hem sağdaki yazıyı hem soldaki butonu aynı anda aktif ediyoruz
        if (gameOverObjesi != null) gameOverObjesi.SetActive(true); 
        if (yenidenBaslatButonObjesi != null) yenidenBaslatButonObjesi.SetActive(true); 

        Debug.Log("OYUN BİTTİ! Hamle kalmadı.");
    }

    void PuanYazisiniGuncelle()
    {
        if (puanYazisiKomponenti != null) puanYazisiKomponenti.text = "PUAN: " + mevcutPuan;
    }

    public bool OyunBittiMiGetir()
    {
        return oyunBittiMi;
    }

    public void OyunuYenidenBaslat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}