using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionScreen : AUIScreen
{
    public Button nextButton;
    public Image characterImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI text;

    private string[] _lines;
    private int _progress = 0;

    private void Awake()
    {
        nextButton.onClick.AddListener(Next);
    }

    public void DisplayDialogue(Character character)
    {
        _lines = character.lines;
        text.text = _lines[_progress];
        SetSprite(character.image);
        nameText.text = character.name;
        gameObject.SetActive(true);
    }

    private void Next()
    {
        _progress++;
        if (_progress >= _lines.Length)
        {
            Hide();
            return;
        }
        text.text = _lines[_progress];
    }

    public void Hide()
    {
        _progress = 0;
        gameObject.SetActive(false);
    }

    private void SetSprite(Sprite sprite)
    {
        characterImage.sprite = sprite;
        //sw/sh = iw/ih  ->  iw = (sw/wh) * ih
        float height = characterImage.rectTransform.rect.height;
        float newWidth = (sprite.rect.width / sprite.rect.height) * height;
        //characterImage.rectTransform.rect.size = new Vector2(newWidth, height);
        characterImage.rectTransform.transform.localScale = new Vector3(newWidth, height, 1)*0.01f;
    }
}
