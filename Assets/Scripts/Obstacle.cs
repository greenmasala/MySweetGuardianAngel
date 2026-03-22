using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool SlowMotion = true;
    public bool SmallFixedDeltaTime = false;

    int fixedUpdateCountInFrame;

    void Update()
    {
        Time.timeScale = SlowMotion ? 0.1f : 1f;
        Time.fixedDeltaTime = SmallFixedDeltaTime ? 0.001f : 0.02f;

        Debug.Log("Fixed update calls in this frame: " + fixedUpdateCountInFrame);
        fixedUpdateCountInFrame = 0;
    }

    private void FixedUpdate()
    {
        fixedUpdateCountInFrame++;
    }
}
