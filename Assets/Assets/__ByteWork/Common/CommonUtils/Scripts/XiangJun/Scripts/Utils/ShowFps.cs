using UnityEngine;
using UnityEngine.UI;

public class ShowFps : MonoBehaviour
{
    private const float UpdateInterval = 0.5F;
    private float _lastUpdateTime;
    private int _frameCount;
    private float _fps;

    private Text _label;

    void Start()
    {
        _label = transform.GetComponent<Text>();
        _label.raycastTarget = false;
    }

    void Update()
    {
        ++_frameCount;
        if (Time.realtimeSinceStartup > _lastUpdateTime + UpdateInterval)
        {
            _fps = _frameCount / (Time.realtimeSinceStartup - _lastUpdateTime);
            _frameCount = 0;
            _lastUpdateTime = Time.realtimeSinceStartup;
            DrawFps();
        }
    }

    private void DrawFps()
    {
        Color color;
        if (_fps > 50)
        {
            color = new Color(0, 1, 0);
        }
        else if (_fps > 40)
        {
            color = new Color(1, 1, 0);
        }
        else
        {
            color = new Color(1.0f, 0, 0);
        }

        _label.color = color;
        _label.text = "Fps: " + Mathf.RoundToInt(_fps);
    }
}