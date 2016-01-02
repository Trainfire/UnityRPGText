using UnityEngine;
using System.Collections.Generic;

public class Example : MonoBehaviour
{
    public TextBox TextBox;

    void Start()
    {
        List<Dialog> dialogs = new List<Dialog>();

        dialogs.Add(new Dialog("Why hi there!"));
        dialogs.Add(new Dialog("It's awfully cold today, isn't it? I'm bloody freezing!"));
        dialogs.Add(new Dialog("I hate the winter. It's the worst..."));

        foreach (var dialog in dialogs)
        {
            TextBox.AddDialog(dialog);
        }
    }
}
