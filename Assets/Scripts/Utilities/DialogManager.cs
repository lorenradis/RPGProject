using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

using System;
using System.Collections;
using System.Collections.Generic;


public class DialogManager : MonoBehaviour
{

    public static DialogManager instance = null;

    public AudioClip[] voiceClips;

    [SerializeField]
    private Button button1; //confirm
    [SerializeField]
    private Button button2; //deny
    [SerializeField]
    private Button button3; //misc
    [SerializeField]
    private Button button4; //misc

    [SerializeField]
    private GameObject dialogPanel;
    [SerializeField]
    private GameObject continueArrow;

    [SerializeField]
    private TextMeshProUGUI dialogText;
    [SerializeField]
    private TextMeshProUGUI speakerNameText;
    [SerializeField]
    private Image speakerPortraitImage;
    [SerializeField]
    private TextMeshProUGUI button1Text;
    [SerializeField]
    private TextMeshProUGUI button2Text;
    [SerializeField]
    private TextMeshProUGUI button3Text;
    [SerializeField]
    private TextMeshProUGUI button4Text;

    private Button[] buttons = new Button[2];
    private TextMeshProUGUI[] buttonTexts = new TextMeshProUGUI[4];

    private bool isActive = false;
    private bool anyKey = false;

    private float waitTime = .025f;

    private List<Dialog> dialogs = new List<Dialog>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        buttons[0] = button1;
        buttons[1] = button2;
        //buttons[2] = button3;
        //buttons[3] = button4;
        buttonTexts[0] = button1Text;
        buttonTexts[1] = button2Text;
        //buttonTexts[2] = button3Text;
        //buttonTexts[3] = button4Text;
    }

    private void Start()
    {
        dialogPanel.SetActive(false);
    }

    public void ShowSimpleDialog(string message)
    {
        Dialog newDialog = new Dialog();
        newDialog.message = message;
        dialogs.Add(newDialog);
        if (!isActive)
        {
            GameManager.instance.EnterDialogState();
            dialogPanel.SetActive(true);
            isActive = true;
            StartCoroutine(DisplayDialog());
        }
        else
        {
            Debug.Log("I received a new dialog while active");
        }
    }

    public void ShowDialog(Dialog newDialog)
    {
        dialogs.Add(newDialog);
        if (!isActive)
        {
            GameManager.instance.EnterDialogState();
            dialogPanel.SetActive(true);
            isActive = true;
            StartCoroutine(DisplayDialog());
        }
        else
        {
            Debug.Log("I received a new dialog while active");
        }
    }

    public void ShowDialog(Dialog newDialog, Action action1, Action action2)
    {
        dialogs.Add(newDialog);
        GameManager.instance.EnterDialogState();
        EventSystem.current.SetSelectedGameObject(button1.gameObject);
        AddCallbacks(action1, action2);

        dialogPanel.SetActive(true);
        if (!isActive)
        {
            StartCoroutine(DisplayDialog());
            isActive = true;
        }
    }

    private void AddCallbacks(Action action1, Action action2)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }

        button1.onClick.AddListener(delegate
        {
            CloseDialogWindow();
            action1();
        });
        button2.onClick.AddListener(delegate
        {
            CloseDialogWindow();
            action2();
        });
    }

    private IEnumerator DisplayDialog()
    {
        Dialog dialog = dialogs[0];
        dialogs.RemoveAt(0);

        float speedUp = 1f;

        dialogText.text = dialog.message;

        if (dialog.speakerPortrait != null)
        {
            speakerPortraitImage.enabled = true;
            speakerPortraitImage.sprite = dialog.speakerPortrait;
        }
        else
        {
            speakerPortraitImage.enabled = false;
        }

        speakerNameText.text = dialog.speakerName;

        for (int i = 0; i < dialog.message.Length + 1; i++)
        {
            dialogText.maxVisibleCharacters = i;
            speedUp = Input.anyKey ? .5f : 1f;
            if(dialog.isSpeech)
                GameManager.instance.audioManager.PlaySoundEffect(voiceClips[UnityEngine.Random.Range(0, voiceClips.Length)]);
            yield return new WaitForSeconds(waitTime * speedUp);
        }

        bool waitingOnInput = true;

        if (dialog.responses.Length < 1)
        {
            continueArrow.SetActive(true);

            while (waitingOnInput)
            {
                waitingOnInput = !Input.anyKeyDown;
                yield return null;
            }

            if (dialogs.Count > 0)
            {
                continueArrow.SetActive(false);
                StartCoroutine(DisplayDialog());
            }
            else
            {
                CloseDialogWindow();
            }
        }
        else
        {
            for (int i = 0; i < dialog.responses.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
                buttonTexts[i].text = dialog.responses[i];
            }
        }
    }

    private void CloseDialogWindow()
    {
        isActive = false;
        dialogPanel.SetActive(false);
        speakerPortraitImage.enabled = false;
        speakerNameText.text = "";
        GameManager.instance.ExitDialogState();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false); 
        }
    }
}

[Serializable]
public class Dialog
{
    public string message;
    public string speakerName;
    public Sprite speakerPortrait;
    public string[] responses = new string[0];

    public bool isSpeech;

    public Dialog()
    {

    }
}