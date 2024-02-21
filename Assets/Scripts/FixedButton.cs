using UnityEngine;
using UnityEngine.EventSystems;

//A Custom script to handle touch buttons
public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public bool Pressed;
    [HideInInspector]
    public bool ButtonDown;
    [HideInInspector]
    public bool ButtonUp;
    private bool isHeld;

    void Update()
    {
        if (Pressed && !isHeld)
        {
            ButtonDown = true;
            isHeld = true;
        }
        else
        {
            ButtonDown = false;
        }

        if (!Pressed && isHeld)
        {
            ButtonUp = true;
            isHeld = false;
        }
        else
        {
            ButtonUp = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
