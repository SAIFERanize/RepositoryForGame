using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPanel : MonoBehaviour
{
    public GameObject descriptionPanel;
    public TMP_Text descriptionText;// ����������� TMP_Text, ���� ����������� TextMeshPro
    public TMP_Text itemNameText; // ���� ��� ����������� ����� ��������


    private void Start()
    {
        descriptionPanel.SetActive(false); // �������� ������ �������� �� ���������
    }

    public void ShowDescription(string name, string description)
    {
        descriptionPanel.SetActive(true);
        itemNameText.text = name; // ������������� ��� ��������
        descriptionText.text = description; // ������������� �������� ��������

        // ���������� ������ �� �������� ����
        descriptionPanel.transform.SetAsLastSibling();
    }

    public void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}
