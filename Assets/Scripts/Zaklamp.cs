using UnityEngine;

public class Zaklamp : MonoBehaviour
{
    private Light zaklampLight;

    private void Start()
    {
        zaklampLight = GetComponent<Light>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            zaklampLight.enabled = !zaklampLight.enabled;
        }
    }
}
