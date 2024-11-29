using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconAnimation : MonoBehaviour
{
    enum AnimationType
    {
        BLINK,//ì_ñ≈
        UPDOWN,//è„â∫
    }

    [SerializeField]
    AnimationType animationType = AnimationType.BLINK;

    [SerializeField]
    Sprite[] iconSprites;

    Image image;

    float count = 0;
    int currentSprite = 0;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (animationType) 
        {
            case AnimationType.BLINK:

                count += Time.deltaTime;
                if (count > 0.5f)
                {
                    count = 0;
                    if (currentSprite == 0)
                        currentSprite = 1;
                    else
                        currentSprite = 0;

                    image.sprite = iconSprites[currentSprite];
                }

                break;

            case AnimationType.UPDOWN:


                break;
        }

    }
}
