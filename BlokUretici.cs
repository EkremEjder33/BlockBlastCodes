using UnityEngine;
using System.Collections.Generic;

public class BlokUretici : MonoBehaviour
{
    public List<GameObject> sekilPrefableri;
    public float blokAraBoslugu = 2.4f;

    private List<GameObject> uretilenBloklar = new List<GameObject>();

    void Start()
    {
        BloklariUret();
    }

    public void BloklariUret()
    {
        uretilenBloklar.Clear();

        for (int i = 0; i < 3; i++)
        {
            if (sekilPrefableri.Count == 0) return;

            int rastgeleIndex = Random.Range(0, sekilPrefableri.Count);
            
            float xOfseti = (i - 1) * blokAraBoslugu;
            Vector3 dogumPozisyonu = transform.position + new Vector3(xOfseti, 0, 0);

            GameObject yeniBlok = Instantiate(sekilPrefableri[rastgeleIndex], dogumPozisyonu, Quaternion.identity);
            yeniBlok.transform.localScale = Vector3.one * 0.72f; 

            uretilenBloklar.Add(yeniBlok);
        }
    }

    public void BlokYerlestirildi(GameObject yerlesenBlok)
    {
        if (uretilenBloklar.Contains(yerlesenBlok))
        {
            uretilenBloklar.Remove(yerlesenBlok);
        }

        // Eski Unity sürümleri için güvenli temizlik döngüsü
        for (int i = uretilenBloklar.Count - 1; i >= 0; i--)
        {
            if (uretilenBloklar[i] == null)
            {
                uretilenBloklar.RemoveAt(i);
            }
        }

        // EĞER TÜM BLOKLAR BİTTİYSE YENİDEN ÜRET
        if (uretilenBloklar.Count == 0)
        {
            BloklariUret();
        }
    }
}