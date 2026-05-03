using TMPro;

public TextMeshProUGUI scoreText;
private int mevcutPuan = 0;

public void PuanEkle(int miktar)
{
    mevcutPuan += miktar;
    scoreText.text = mevcutPuan.ToString();
}

// patlayan değere göre puan hesaplama aracı 
//03,05,2026 githuba yüklenecek game over 
//koşulundan devam edilecek 