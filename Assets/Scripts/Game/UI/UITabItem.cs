using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabItem : MonoBehaviour
{
    public TabViewStatus status = TabViewStatus.None;

    [SerializeField] bool isActive = false;

    [SerializeField] GameObject icon = null;
    [SerializeField] TextMeshProUGUI txtName = null;
    [SerializeField] GameObject mask = null;

    private UIFixMenu fixMenu;
    private LayoutElement layoutElement;

    protected virtual void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
    }

    public void Set(UIFixMenu fixMenu)
    {
        this.fixMenu = fixMenu;
    }

    public void OnClick()
    {
        fixMenu.ChangeStatus(status);
    }

    public void Active(TabViewStatus status)
    {
        bool isActive = this.status == status;
        if (this.isActive == isActive) return;

        this.isActive = isActive;
        layoutElement.flexibleWidth = isActive ? 1.677f : 1;

        icon.gameObject.transform.localScale = new Vector3(isActive ? 1.35f : 1, isActive ? 1.35f : 1, 1);
        Vector2 posIcon = icon.GetComponent<RectTransform>().anchoredPosition;
        posIcon.y = isActive ? 73f : 10f;
        icon.GetComponent<RectTransform>().anchoredPosition = posIcon;

        mask.gameObject.SetActive(isActive);
        txtName.gameObject.SetActive(isActive);
    }
}
