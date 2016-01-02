using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class TextBoxData
{
    public AudioClip Sound;
    public float ReadSpeed = 0.1f;
    public float SpaceHoldTime = 0f;
    public float FullStopHoldTime = 0.25f;
    public float CommaHoldTime = 0.4f;

    // This multipler will be used to modify how quickly the text is read when a key or button is held down.
    public float SpeedModifier = 1.5f;
}

public class TextBox : MonoBehaviour
{
    public AudioSource AudioSource;
    public TextBoxData TextBoxData;
    public Text Text;

    string currentString = "";
    bool isRevealingText = false;
    bool isSpeedModifierActive = false;
    KeyCode speedModifierKey = KeyCode.LeftShift;
    KeyCode skipKey = KeyCode.Space;
    IEnumerator revealText;

	void Start ()
    {
        ShowText("Legend has it that this was created in only a few minutes. But was it?... Yeah, it was.");
	}
	
	public void ShowText(string text)
    {
        Text.text = "";
        revealText = RevealText(text);
        StartCoroutine(RevealText(text));
    }

    IEnumerator RevealText(string text)
    {
        isRevealingText = true;

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
                    holdTime = TextBoxData.FullStopHoldTime;
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

        isRevealingText = false;

        yield return 0;
    }

    public void Skip()
    {
        isRevealingText = false;
        StopAllCoroutines();
        Text.text = currentString;
    }

    void LateUpdate()
    {
        isSpeedModifierActive = Input.GetKey(speedModifierKey);

        if (isRevealingText && Input.GetKeyDown(skipKey))
            Skip();
    }

    float GetReadSpeedModifier()
    {
        return isSpeedModifierActive ? TextBoxData.SpeedModifier : 1f;
    }
}
