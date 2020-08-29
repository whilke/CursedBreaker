using System;
using System.Collections;
using System.Collections.Generic;
using enBask;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image Icon;
    public Button Button;
    public TMP_Text Text;
    public Slider slider;
    public float cooldown = 5f;

    private ItemData item;
    private float accumulator = 0f;
    public bool isCoolingDown = false;

    private void Awake()
    {
        this.slider.value = 0f;
    }

    public void SetItem(ItemData item)
    {
        this.item = item;
        this.Icon.sprite = item.Icon;
        this.Text.text = item.Name;
    }

    public ItemData GetItem()
    {
        return this.item;
    }

    private void Update()
    {
        if (!this.isCoolingDown) return;

        this.accumulator += Time.deltaTime;

        var p = 1f - this.accumulator / this.cooldown;
        this.slider.value = p;

        if (this.accumulator >= this.cooldown)
        {

            this.isCoolingDown = false;
            this.Button.interactable = true;
        }
    }

    public void OnClick()
    {
        GameObject.FindObjectOfType<ItemManager>().StartPlaceItem(this.item, this);
    }

    public void OnClickEnd()
    {
        GameObject.FindObjectOfType<ItemManager>().StopPlaceItem();
    }

    public void EnableCooldown()
    {
        this.Button.interactable = false;
        this.isCoolingDown = true;
        this.accumulator = 0f;
    }
}
