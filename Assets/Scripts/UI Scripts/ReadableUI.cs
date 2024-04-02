using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

/**
 * The class for the UI for Readable Items like Books and Journals.
 * 
 * Has a reference to the UI Container with all the UI Elements as well as the actual Text-Element.
 */
public class ReadableUI : MonoBehaviour
{

    [SerializeField] private GameObject container;


    [SerializeField] private TextMeshProUGUI ReadableUIText;

    public bool isShown = false;

    /**
     * When the UI is shown it activates the OnRead action that other classes are listening to.
     */
    private void OnEnable()
    {
        Actions.OnRead += Show;
    }

    /**
     * When the UI is hidden it removes the OnRead action that other classes are listening to.
     */
    private void OnDisable()
    {
        Actions.OnRead -= Show;
    }

    /**
     * De-activates the UI Container.
     */
    void Start()
    {
        container.SetActive(false);
    }

    /**
     * @param readable: The Object that is read.
     * 
     * Activates the UI-Container and changes the Text-Element to the text of the readable object.
     */
    void Show(Readables readable)
    {
        ReadableUIText.text = readable.book.text;
        container.SetActive(true);
        isShown = true;
    }

    /**
     * Hides the UI-Container again when closing it after reading.
     */
    public void Hide()
    {
        ReadableUIText.text = null;
        container.SetActive(false);
        isShown = false;
    }

}
