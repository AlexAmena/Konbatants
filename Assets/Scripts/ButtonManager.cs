using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button button;
    private ColorBlock colors;
    private bool isClicked;
    private float transparency = 0.5f;

    void Start()
    {
        colors = button.colors;
        SetButtonColor(transparency);
        isClicked = false;
    }

    public void OnButtonClick()
    {
        int currentAmount = PlayerPrefs.GetInt("amountPlayers");
        if (!isClicked)
        {
            SetButtonColor(1f);
            isClicked = true;
            PlayerPrefs.SetInt("amountPlayers", currentAmount+1);
        }
        else
        {
            SetButtonColor(transparency);
            isClicked = false;
            PlayerPrefs.SetInt("amountPlayers", currentAmount-1);
        }
        Debug.Log("amountPlayers: " + PlayerPrefs.GetInt("amountPlayers"));
    }

    private void SetButtonColor(float alpha)
    {
        colors.normalColor = new Color(colors.normalColor.r, colors.normalColor.g, colors.normalColor.b, alpha);
        colors.highlightedColor = new Color(colors.highlightedColor.r, colors.highlightedColor.g, colors.highlightedColor.b, alpha);
        colors.pressedColor = new Color(colors.pressedColor.r, colors.pressedColor.g, colors.pressedColor.b, alpha);
        colors.selectedColor = new Color(colors.selectedColor.r, colors.selectedColor.g, colors.selectedColor.b, alpha);
        button.colors = colors;
    }
}
