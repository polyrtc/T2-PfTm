using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        this.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, this.transform.position.z);
    }
}
