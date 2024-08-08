using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPanel : MonoBehaviour
{
    public GameObject descriptionPanel;
    public TMP_Text descriptionText;// Используйте TMP_Text, если используете TextMeshPro
    public TMP_Text itemNameText; // Поле для отображения имени предмета


    private void Start()
    {
        descriptionPanel.SetActive(false); // Скрываем панель описания по умолчанию
    }

    public void ShowDescription(string name, string description)
    {
        descriptionPanel.SetActive(true);
        itemNameText.text = name; // Устанавливаем имя предмета
        descriptionText.text = description; // Устанавливаем описание предмета

        // Перемещаем панель на передний план
        descriptionPanel.transform.SetAsLastSibling();
    }

    public void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}
