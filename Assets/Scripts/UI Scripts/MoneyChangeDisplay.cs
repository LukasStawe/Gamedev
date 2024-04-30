using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyChangeDisplay : MonoBehaviour
{
    public TextMeshProUGUI changeValue;

    private Coroutine coroutine;

    [SerializeField]
    public RectTransform poolParent;

    public void StartDisplay(int value)
    {
        gameObject.SetActive(true);
        changeValue.text = "+ " + value.ToString();
        changeValue.alpha = 1f;
        coroutine = StartCoroutine(Displaying());
    }

    public void StopDisplay()
    {
        StopCoroutine(coroutine);
        transform.SetParent(poolParent);
        gameObject.SetActive(false);
    }

    private IEnumerator Displaying()
    {
        yield return new WaitForSeconds(3f);

        while (changeValue.alpha > 0)
        {
            changeValue.alpha -= 0.01f;
            yield return null;
        }

        StopDisplay();
    }
}
