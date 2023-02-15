using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public float speed = 5f;
    public float zoomSpeed = 1f;
    public GameObject grid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // mvt
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 mvt = new Vector3(horizontal, vertical);
        mvt = mvt.normalized * speed;
        grid.transform.position += mvt * Time.deltaTime;

    }
    public void ResetGridPos()
    {
        grid.transform.position = new Vector3(0, 0, -10);
    }

}
