using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressBar;
    public Image shipIcon;

    private float barHeight;
    // private float barWidth;
    private float deltaY;
    private Vector2 currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        // get dimensions
        // barWidth = progressBar.rectTransform.rect.width;
        barHeight = progressBar.rectTransform.rect.height;
        deltaY = barHeight / 100.0f;

        // place the icon at the bottom
        Vector2 bottom = new Vector2(0,-(barHeight/2.0f));
        shipIcon.rectTransform.anchoredPosition = bottom;
    }

    // Update is called once per frame
    void Update()
    {
        Progress(0.00001f);
    }

    void Progress(float delta) // progress the icon by this amount (float)
    {
        if (delta > 1.0f)
            delta = 1.0f;
        Vector2 newPos = new Vector2(0, (shipIcon.rectTransform.anchoredPosition.y + barHeight * delta));
        shipIcon.rectTransform.anchoredPosition = newPos;
    }
}
