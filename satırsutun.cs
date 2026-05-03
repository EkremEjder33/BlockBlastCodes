public bool YerlestirilebilirMi(DraggableShape sekil)
{
    foreach (ShapePart parca in sekil.GetComponentsInChildren<ShapePart>())
    {
        Vector2Int pos = parca.GetGridPosition();

        // dışarı veya dolu durumları için geçerli metod not al
        if (pos.x < 0 || pos.x >= genislik || pos.y < 0 || pos.y >= yukseklik) return false;
        if (gridMantigi[pos.x, pos.y] == 1) return false;
    }
    return true;
}

public void SekliYerlestir(DraggableShape sekil)
{
    foreach (ShapePart parca in sekil.GetComponentsInChildren<ShapePart>())
    {
        Vector2Int pos = parca.GetGridPosition();
        gridMantigi[pos.x, pos.y] = 1; // dolu işaretle işlemi onaylama
        
        // kare oluşturma ve renk paleti değiştirme
        GameObject yeniKare = Instantiate(hucreDoluGorseli, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        yeniKare.transform.parent = this.transform;
    }
    SatirlariKontrolEt();
}