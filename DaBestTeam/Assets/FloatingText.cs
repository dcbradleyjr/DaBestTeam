using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 1f;
    public Vector3 offset = new Vector3 (0, 2, 0);
    public Vector3 RandomizeIntensity = new Vector3(0.5f, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,destroyTime);
        //offset.x = Random.Range(-1f, 1f);
        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
