void SatirlariKontrolEt()
{
    for (int y = 0; y < yukseklik; y++)
    {
        bool satirDolu = true;
        for (int x = 0; x < genislik; x++)
        {
            if (gridMantigi[x, y] == 0) satirDolu = false;
        }

        if (satirDolu) SatiriPatlat(y);
    }
    // sutunu eklemeye çalıştığımda hem satır hem sutun algılıyor ayıramadım fatma hocaya sorulacak
}

void SatiriPatlat(int satirIndex)
{
    // geçerli satırdaki blokları sıfırla ve index = 0 olacak
    Debug.Log(satirIndex + ". satir patladi! Puan kazanildi.");
}