using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;
    [SerializeField] bool lockZ;
    Vector3 ogRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ogRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);

        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX)
        {
            rotation.x = ogRotation.x;
        }
        if (lockY)
        {
            rotation.y = ogRotation.y;
        }
        if (lockZ)
        {
            rotation.z = ogRotation.z;
        }
        transform.rotation = Quaternion.Euler(rotation);
    }
}
