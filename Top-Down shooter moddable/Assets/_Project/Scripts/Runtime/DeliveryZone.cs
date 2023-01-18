using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public Vector2 deliveryZoneSize;
    List<ISellable> soldSellables = new List<ISellable>();
    
    

    public void SellVegetablesBoxes()
    {
        soldSellables.Clear();
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, deliveryZoneSize, 0, Vector3.forward);
        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            if (hit.collider.gameObject == this.gameObject) continue;
            if (!hit.collider.TryGetComponent<ISellable>(out ISellable sellable)) continue;

            sellable.Sell(sellable.vegetableCount, sellable.value);
            soldSellables.Add(sellable);
            sellable.EmptyBox();
            Destroy(hit.collider.gameObject, 2f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, deliveryZoneSize);
    }
}
