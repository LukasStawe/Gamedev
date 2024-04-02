using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI leftPageNumber;
    [SerializeField]
    private TextMeshProUGUI rightPageNumber;

    [SerializeField]
    private Button nextPageButton; 
    [SerializeField]
    private Button previousPageButton;

    [SerializeField]
    private GameObject leftPage;
    [SerializeField]
    private GameObject rightPage;

    private int currentLeftPage = 1;

    [SerializeField]
    private GameObject entryPrefab;


    private JournalManager journalManager;

    public bool isShown = false;

    private int remainingEntries;

    private int entriesLeft;
    private int entriesRight;
    public List<GameObject> createdEntries = new();

    [SerializeField]
    private GameObject container;

    // Start is called before the first frame update
    void Start()
    {
        journalManager = JournalManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        container.SetActive(true);
        isShown = true;
        currentLeftPage = 1;
        entriesLeft = 0;
        entriesRight = 0;
        remainingEntries = journalManager.journalEntries.Count;
        previousPageButton.gameObject.SetActive(false);
        CreateLeftPage();

        if (remainingEntries == 0)
        {
            nextPageButton.gameObject.SetActive(false);
        }
    }

    public void Hide()
    {
        container.SetActive(false);
        isShown = false;

        //Transform[] children = leftPage.gameObject.GetComponentsInChildren<Transform>();
        //foreach (Transform child in children)
        //{
        //    Destroy(child.gameObject);
        //}

        //children = rightPage.gameObject.GetComponentsInChildren<Transform>();
        //foreach (Transform child in children)
        //{
        //    Destroy(child.gameObject);
        //}

        foreach (GameObject createdEntry in createdEntries)
        {
            Destroy(createdEntry);
        }
    }

    private void UpdateUI()
    {
        //Transform[] children = leftPage.gameObject.GetComponentsInChildren<Transform>();
        //foreach (Transform child in children)
        //{
        //    if (child.tag == "JournalEntry")
        //    {
        //        Destroy(child.gameObject);
        //    }
        //}

        //children = rightPage.gameObject.GetComponentsInChildren<Transform>();
        //foreach (Transform child in children)
        //{
        //    if (child.tag == "JournalEntry")
        //    {
        //        Destroy(child.gameObject);
        //    }
        //}

        foreach (GameObject createdEntry in createdEntries)
        {
            Destroy(createdEntry);
        }

        CreateLeftPage();

        if (currentLeftPage != 1 )
        {
            previousPageButton.gameObject.SetActive(true);
        } else
        {
            previousPageButton.gameObject.SetActive(false);
        }

        if (remainingEntries == 0 ) 
        {
            nextPageButton.gameObject.SetActive(false);
        } else
        {
            nextPageButton.gameObject.SetActive(true);
        }

        leftPageNumber.text = currentLeftPage.ToString();
        rightPageNumber.text = (currentLeftPage + 1).ToString();
    }

    private void CreateLeftPage()
    {
        for (int i = (journalManager.journalEntries.Count - 1) - (currentLeftPage - 1) * 3; i >= 0; i--)
        {
            if (remainingEntries == 0 || entriesLeft >= 3)
            {
                break;
            }

            GameObject entryCopy = Instantiate(entryPrefab, leftPage.transform, false);
            //entryCopy.transform.SetParent(leftPage.transform, false);
            entryCopy.GetComponent<TextMeshProUGUI>().text = journalManager.journalEntries[i].entryText;
            remainingEntries--;
            entriesLeft++;

            createdEntries.Add(entryCopy);
        }

        if ( remainingEntries > 0)
        {
            CreateRightPage();
        }
    }

    private void CreateRightPage()
    {
        for (int i = (journalManager.journalEntries.Count - 1) - currentLeftPage * 3; i >= 0; i--)
        {
            if (remainingEntries == 0 || entriesRight >= 3)
            {
                break;
            }

            GameObject entryCopy = Instantiate(entryPrefab);
            entryCopy.transform.SetParent(rightPage.transform, false);
            entryCopy.GetComponent<TextMeshProUGUI>().text = journalManager.journalEntries[i].entryText;
            remainingEntries--;
            entriesRight++;

            createdEntries.Add(entryCopy);
        }
    }

    public void NextPage()
    {
        currentLeftPage = currentLeftPage + 2;
        entriesLeft = 0;
        entriesRight = 0;

        UpdateUI();
    }

    public void PreviousPage()
    {
        currentLeftPage = currentLeftPage - 2;
        remainingEntries = remainingEntries + 6 + entriesRight + entriesLeft;
        entriesLeft = 0;
        entriesRight = 0;

        UpdateUI();
    }
}
