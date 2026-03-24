using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    public CinemachineCamera PlayerCam;
    CinemachineThirdPersonFollow ThirdPersonCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThirdPersonCam = PlayerCam.GetComponent<CinemachineThirdPersonFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = ThirdPersonCam.ShoulderOffset;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            ThirdPersonCam.ShoulderOffset = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        ThirdPersonCam.ShoulderOffset = originalPos;
    }
}
