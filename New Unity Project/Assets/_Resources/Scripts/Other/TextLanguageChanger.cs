using UnityEngine;

public class TextLanguageChanger : MonoBehaviour
{
    [SerializeField] int lineId;

    private void OnEnable()
    {
        change();
    }

    public void change()
    {
        GetComponent<UnityEngine.UI.Text>().text = LanguageLines.getLine(lineId);
    }
}
