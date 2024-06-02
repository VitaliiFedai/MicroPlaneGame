using TMPro;
using UnityEngine;

public class FPSHandler : MonoBehaviour
{
    [SerializeField] private float _refreshDelaySec = 0.5f;
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

    private void Reset()
    {
        if (_valueText == null && TryGetComponent(out TMP_Text text))
        {
            _valueText = text;
        }
    }
}
