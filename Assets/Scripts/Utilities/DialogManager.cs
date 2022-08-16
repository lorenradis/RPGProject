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
    private Button button1; //yes
    [SerializeField]
    private Button button2; //no
    [SerializeField]
    private Button button3; //cancel
    [SerializeField]
    private Button button4; //misc

    private Button[] buttons = new Button[4];

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
    }

    private void Start()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
        buttons[0] = button1;
        buttons[1] = button2;
        buttons[2] = button3;
        buttons[3] = button4;
        buttonTexts[0] = button1Text;
        buttonTexts[1] = button2Text;
        buttonTexts[2] = button3Text;
        buttonTexts[3] = button4Text;

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
            Debug.Log("I received a dialog while inactive");
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

    public void ShowDialog(Dialog newDialog, Action action1)
    {
        dialogs.Add(newDialog);
        GameManager.instance.EnterDialogState();
        EventSystem.current.SetSelectedGameObject(button1.gameObject);
        AddCallbacks(action1);


        dialogPanel.SetActive(true);
        if (!isActive)
        {
            StartCoroutine(DisplayDialog());
            isActive = true;
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

    public void ShowDialog(Dialog newDialog, Action action1, Action action2, Action action3)
    {
        dialogs.Add(newDialog);
        GameManager.instance.EnterDialogState();
        EventSystem.current.SetSelectedGameObject(button1.gameObject);
        AddCallbacks(action1, action2, action3);

        dialogPanel.SetActive(true);
        if (!isActive)
        {
            StartCoroutine(DisplayDialog());
            isActive = true;
        }
    }

    public void ShowDialog(Dialog newDialog, Action action1, Action action2, Action action3, Action action4)
    {
        dialogs.Add(newDialog);
        GameManager.instance.EnterDialogState();
        EventSystem.current.SetSelectedGameObject(button1.gameObject);
        AddCallbacks(action1, action2, action3, action4);

        dialogPanel.SetActive(true);
        if (!isActive)
        {
            StartCoroutine(DisplayDialog());
            isActive = true;
        }
    }

    private void AddCallbacks(Action action)
    {
        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(delegate
        {
            CloseDialogWindow();
            action();
        });
    }

    private void AddCallbacks(Action action1, Action action2)
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();

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

    private void AddCallbacks(Action action1, Action action2, Action action3)
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();

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
        button3.onClick.AddListener(delegate
        {
            CloseDialogWindow();
            action3();
        });
    }

    private void AddCallbacks(Action action1, Action action2, Action action3, Action action4)
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button4.onClick.RemoveAllListeners();

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
        button3.onClick.AddListener(delegate
        {
            CloseDialogWindow();
            action3();
        });
        button4.onClick.AddListener(delegate
        {
            CloseDialogWindow();
            action4();
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
            if (dialog.isSpeech)
            {
                //GameManager.instance.audioManager.PlaySoundEffect(voiceClips[UnityEngine.Random.Range(0, voiceClips.Length)]);
            }
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

            yield return null;

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
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }
    }

    private void ClearListeners()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
    }

    private void CloseDialogWindow()
    {
        ClearListeners();
        isActive = false;
        dialogPanel.SetActive(false);
        speakerPortraitImage.enabled = false;
        speakerNameText.text = "";
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
        GameManager.instance.ExitDialogState();
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