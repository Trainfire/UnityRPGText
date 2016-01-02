using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TextBoxData
{
    public AudioClip Sound;
    public float ReadSpeed = 0.1f;
    public float SpaceHoldTime = 0f;
    public float EndHoldTime = 0.25f;
    public float CommaHoldTime = 0.4f;

    // This multipler will be used to modify how quickly the text is read when a key or button is held down.
    public float SpeedModifier = 1.5f;
}

public class TextBox : MonoBehaviour
{
    public AudioSource AudioSource;
    public TextBoxData TextBoxData;
    public Image Speaker;
    public Text Text;

    string currentString = "";
    bool isRevealingText = false;
    bool isSpeedModifierActive = false;
    KeyCode speedModifierKey = KeyCode.LeftShift;
    KeyCode skipKey = KeyCode.Space;
    IEnumerator revealText;

    Queue<Dialog> Dialogs = new Queue<Dialog>();

    /// <summary>
    /// Adds an instance of Dialog and then shows it. If a dialog is being shown, this dialog will be queued.
    /// </summary>
    /// <param name="dialog"></param>
	public void AddDialog(Dialog dialog)
    {
        Dialogs.Enqueue(dialog);
        if (!isRevealingText)
            UpdateQueue();
    }

    void UpdateQueue()
    {
        if (Dialogs.Count == 0)
        {
            Close();
            return;
        }

        Open();

        var dialog = Dialogs.Dequeue();
        Speaker.gameObject.SetActive(!string.IsNullOrEmpty(dialog.Speaker));

        StartCoroutine(RevealText(dialog.Contents));
    }

    IEnumerator RevealText(string text)
    {
        isRevealingText = true;
        Text.text = "";
        currentString = text;

        var chars = text.ToCharArray();

        string str = "";
        for (int i = 0; i < chars.Length; i++)
        {
            str += chars[i];
            Text.text = str;

            float holdTime = TextBoxData.ReadSpeed;

            switch (chars[i])
            {
                case ' ':
                    holdTime = TextBoxData.SpaceHoldTime;
                    break;
                case '.':
                    holdTime = TextBoxData.EndHoldTime;
                    break;
                case '!':
                    holdTime = TextBoxData.EndHoldTime;
                    break;
                case '?':
                    holdTime = TextBoxData.EndHoldTime;
                    break;
                case ',':
                    holdTime = TextBoxData.CommaHoldTime;
                    break;
                default:
                    break;
            }

            AudioSource.clip = TextBoxData.Sound;
            AudioSource.Play();

            yield return new WaitForSeconds(holdTime / GetReadSpeedModifier());
        }

        Finish();

        yield return 0;
    }

    void Finish()
    {
        isRevealingText = false;
        StopAllCoroutines();
        Text.text = currentString;
    }

    void Open()
    {
        gameObject.SetActive(true);
    }

    void Close()
    {
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        isSpeedModifierActive = Input.GetKey(speedModifierKey);

        if (Input.GetKeyDown(skipKey))
        {
            if (isRevealingText)
            {
                Finish();
            }
            else
            {
                // Show next prompt...
                UpdateQueue();
            }
        }
    }

    float GetReadSpeedModifier()
    {
        return isSpeedModifierActive ? TextBoxData.SpeedModifier : 1f;
    }
}
