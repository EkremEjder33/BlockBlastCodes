void OnMouseUp()
{
    surukleniyor = false;
    
    if (GridManager.Instance.YerlestirilebilirMi(this))
    {
        GridManager.Instance.SekliYerlestir(this);
        // yeni şekli buradan çağıracağız
        gameObject.SetActive(false); 
    }
    else
    {
  
        transform.position = baslangicPozisyonu;
        // yerleşme olmadığı durumlarda kullanılacak not al
    }
}