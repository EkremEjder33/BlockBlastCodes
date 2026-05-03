using UnityEngine;
using System.Collections.Generic;

public class ShapeSpawner : MonoBehaviour
{
    public List<GameObject> sekilPrefablar; // şekil çeşitleri kütüphanesi gibi düşün
    public Transform[] spawnNoktalari; 
    private GameObject[] mevcutSekiller = new GameObject[3];

    void Start() => SekilleriYenile();

    public void SekilleriYenile()
    {
        for (int i = 0; i < spawnNoktalari.Length; i++)
        {
            // bir şekil kullanıldıysa yeni şekle geçmek için koşul
            if (spawnNoktalari[i].childCount == 0)
            {
                int rastgeleIndex = Random.Range(0, sekilPrefablar.Count);
                GameObject yeniSekil = Instantiate(sekilPrefablar[rastgeleIndex], spawnNoktalari[i].position, Quaternion.identity);
                yeniSekil.transform.parent = spawnNoktalari[i];
            }
        }
    }
}