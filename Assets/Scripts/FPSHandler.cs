using TMPro;
using UnityEngine;

public class FPSHandler : MonoBehaviour
{
    [SerializeField] private float _refreshDelaySec;
    [SerializeField] private TMP_Text _valueText;

    float _elapsedTime;
    int _frames;

    private void Update()
    {
        _frames++;
        _elapsedTime += Time.unscaledDeltaTime;
        if (_elapsedTime >= _refreshDelaySec)
        {
            int fps = (int)(_frames / _elapsedTime);
            _elapsedTime = 0;
            _frames = 0;

            _valueText.text = fps.ToString();
        }
    }
}
