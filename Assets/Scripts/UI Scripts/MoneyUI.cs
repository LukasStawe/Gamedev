using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{

    [SerializeField]
    private GameObject moneyUI;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private GameObject changeParent;
    [SerializeField]
    private GameObject moneyChangePrefab; 
    [SerializeField]
    private RectTransform poolParent;

    private Coroutine moneyDisplay;

    private List<GameObject> displayedChanges = new List<GameObject>();


    private void Start()
    {
        Inventory.instance.onMoneyChangedCallback += DisplayMoney;
    }

    private void DisplayMoneyChange(int value)
    {
        GameObject changeDisplay;
        if (displayedChanges.Count < 3)
        {
            changeDisplay = Instantiate(moneyChangePrefab, changeParent.transform);
            changeDisplay.GetComponent<MoneyChangeDisplay>().poolParent = poolParent;
        } else
        {
            changeDisplay = displayedChanges[0];
            displayedChanges.RemoveAt(0);
            changeDisplay.GetComponent<MoneyChangeDisplay>().StopDisplay();
            changeDisplay.transform.SetParent(changeParent.transform);
        }
        displayedChanges.Add(changeDisplay);
        changeDisplay.GetComponent<MoneyChangeDisplay>().StartDisplay(value);

    }

    public void DisplayMoney(int value, int change)
    {
        Debug.Log("Money changed displaying");
        if (moneyDisplay != null)
        {
            StopCoroutine(moneyDisplay);
        }
        moneyText.alpha = 1f;
        moneyDisplay = StartCoroutine(DisplayMoneyCoroutine(value));
        DisplayMoneyChange(change);
    }

    private IEnumerator DisplayMoneyCoroutine(int value)
    {
        moneyText.text = value.ToString() + " Gold";
        moneyUI.SetActive(true);
        yield return new WaitForSeconds(3f);

        while (moneyText.alpha > 0f)
        {
            moneyText.alpha -= 0.01f;
            yield return null;
        }

        moneyUI.SetActive(false);
        yield break;
    }
}
