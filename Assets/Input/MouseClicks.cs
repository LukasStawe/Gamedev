using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseClicks : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private GameObject target;

    private bool wasPressed = false;
    private float clickTime = 0;
    private readonly float clickDelay = 0.5f;

    private void Update()
    {
        if (wasPressed)
        {
            if (Time.unscaledTime - clickTime > clickDelay)
            {
                wasPressed = false;
                clickTime = 0;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            target.SendMessage("OnRightClick");
        } else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (wasPressed)
            {
                wasPressed = false;
                target.SendMessage("OnDoubleClick");
                clickTime = 0;
            } else
            {
                wasPressed = true;
                clickTime = Time.unscaledTime;
            }
        }
    }
}

