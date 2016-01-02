using UnityEngine;
using System.Collections.Generic;

public class Dialog
{
    public string Speaker { get; private set; }
    public string Contents { get; private set; }

    public Dialog(string contents, string speaker = "")
    {
        Contents = contents;
        Speaker = speaker;
    }
}
