using UnityEngine;

public class ShapePart : MonoBehaviour
{
    // parçanın hangi hücrenin üzerinde olduğunu anlamak için derse not al 
    public Vector2Int GetGridPosition()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }
   //round yapısı fatma hocaya sorulacak
}