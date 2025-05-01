using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    public UpgradeScriptable UpgradeObject;

    [HideInInspector] public List<UpgradeClass> CurrentUpgrades = new List<UpgradeClass>();

    public GameObject G_UpgradePanel;
    public UpgradeButton[] UpgradeButtons;
    public Image I_UPDarkenator;
    public RectTransform RT_UPArrow;
    public RectTransform[] RT_UPPaintSplatter;

    public AnimCurve_Scriptable A_flyIn;

    public TextMeshProUGUI TM_currency;
    public Image I_currencySlider;

    public numberClass Cost;
    public int I_curCurrency;
    public int I_level = 0;

    private void OnEnable()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cost.Update(I_level);
        CurrentUpgrades = UpgradeObject.CloneList();
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Upgrade()
    {
        I_curCurrency -= Cost.curNumber;
        I_level++;
        Cost.Update(I_level);
        UpdateDisplay();
        UpgradeDisplay();
    }

    void UpdateDisplay()
    {
        TM_currency.text = "Cash".ToSpriteString() + I_curCurrency.AbbreviatedString() + " / " + Cost.curNumber.AbbreviatedString();
        I_currencySlider.fillAmount = (float)I_curCurrency / (float)Cost.curNumber;
    }

    public void Currency_Add(int _amt)
    {
        I_curCurrency += _amt;
        UpdateDisplay();
        if (I_curCurrency >= Cost.curNumber) Upgrade();
    }

    void UpgradeDisplay()
    {
        UpgradeClass[] _upgrades = GetRandomUpgrades(3);
        for (int i = 0; i < _upgrades.Length; i++)
            UpgradeButtons[i].OnCreate(_upgrades[i]);
        StartCoroutine(UpgradePanel_Show());
    }
    void UpgradeClose()
    {
        StartCoroutine(UpgradePanel_Hide());
    }

    public void LevelUp_Upgrade(UpgradeClass _upgrade)
    {
        _upgrade.level++;
        UpgradeClose();
    }

    public UpgradeClass[] GetRandomUpgrades(int _amt)
    {
        UpgradeClass[] _temp = new UpgradeClass[_amt];
        for (int i = 0; i < _amt; i++)
        {
            int _ran = Random.Range(0, CurrentUpgrades.Count);
            _temp[i] = CurrentUpgrades[_ran];
        }
        return _temp;
    }

    void UpgradePanel_Offscreen()
    {
        I_UPDarkenator.color = new Color(0, 0, 0, 0);
        foreach (var item in RT_UPPaintSplatter)
            item.anchoredPosition = Vector3.down * 1500;
        UpgradeButtons[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1500, 0);
        UpgradeButtons[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1500, 0);
        UpgradeButtons[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1500, 0);
        RT_UPArrow.anchoredPosition = Vector3.down * 1500;
    }

    IEnumerator UpgradePanel_Show()
    {
        G_UpgradePanel.SetActive(true);
        UpgradePanel_Offscreen();

        StartCoroutine(ShowDarkenator());
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(MovePosition(RT_UPArrow, new Vector3(0,120,0)));
        StartCoroutine(MovePosition(UpgradeButtons[0].GetComponent<RectTransform>(), new Vector3(0, 50, 0)));
        foreach (var item in RT_UPPaintSplatter)
            StartCoroutine(MovePosition(item, Vector3.zero));
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 0.01f;
        StartCoroutine(MovePosition(UpgradeButtons[1].GetComponent<RectTransform>(), new Vector3(0, -250, 0)));
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(MovePosition(UpgradeButtons[2].GetComponent<RectTransform>(), new Vector3(0, -550, 0)));
        //Nested Coroutines
        IEnumerator ShowDarkenator()
        {
            float timer = 0;
            while (timer < 1)
            {
                I_UPDarkenator.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.5f), A_flyIn.Evaluate(timer));
                timer += Time.unscaledDeltaTime / 0.5f;
                yield return new WaitForEndOfFrame();
            }
        }
    }
    IEnumerator UpgradePanel_Hide()
    {
        StartCoroutine(MovePosition(RT_UPArrow, new Vector3(0, 1500, 0)));
        StartCoroutine(MovePosition(UpgradeButtons[0].GetComponent<RectTransform>(), new Vector3(0, 1500, 0)));
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(MovePosition(UpgradeButtons[1].GetComponent<RectTransform>(), new Vector3(0, 1500, 0)));
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(MovePosition(UpgradeButtons[2].GetComponent<RectTransform>(), new Vector3(0, 1500, 0)));
        foreach (var item in RT_UPPaintSplatter)
            StartCoroutine(MovePosition(item, new Vector3(0, -1500, 0)));

        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(HideDarkenator());
        yield return new WaitForSecondsRealtime(0.5f);
        G_UpgradePanel.SetActive(false);
        //Nested Coroutines
        IEnumerator HideDarkenator()
        {
            float timer = 0;
            while (timer < 1)
            {
                I_UPDarkenator.color = Color.Lerp(new Color(0, 0, 0, 0.5f), new Color(0, 0, 0, 0f), A_flyIn.Evaluate(timer));
                timer += Time.unscaledDeltaTime / 0.5f;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator MovePosition(RectTransform _trans, Vector3 tarAnchorPos, float duration = 0.5f)
    {
        float timer = 0;
        Vector3 startPos = _trans.anchoredPosition;
        while (timer < 1)
        {
            _trans.anchoredPosition = Vector3.Lerp(startPos, tarAnchorPos, A_flyIn.Evaluate(timer));
            timer += Time.unscaledDeltaTime / duration;
            yield return new WaitForEndOfFrame();
        }
    }
}

[System.Serializable]
public class UpgradeClass
{
    public string name;
    public string id;
    [Space (10)]
    public string description;

    public int level;
    public numberClass value;

    public UpgradeClass Clone()
    {
        UpgradeClass _temp = new UpgradeClass();
        _temp.name = name;
        _temp.description = description;
        _temp.id = id;
        _temp.level = level;
        _temp.value = value;
        return _temp;
    }
}