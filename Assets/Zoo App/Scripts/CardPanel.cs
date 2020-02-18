using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPanel : MonoBehaviour
{
    [SerializeField]
    Text titleText;
    [SerializeField]
    Text bodyText;
    [SerializeField]
    Text footerText;
    [SerializeField]
    Image animalPhoto;
    [SerializeField]
    CardModel[] cards;

    public void openCard(int cardNumber)
    {      
        CardModel myCard = cards[cardNumber];

        titleText.text = myCard.title;
        bodyText.text = myCard.description;
        footerText.text = myCard.footer;
        animalPhoto.sprite = myCard.image;

        this.gameObject.SetActive(true);
    }

    public void closeCard()
    {
        this.gameObject.SetActive(false);
    }

}
