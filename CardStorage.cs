using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine;


public class CardStorage : MonoBehaviour
{    
    [SerializeField] private Transform testIQDeckTransform;
    [SerializeField] private Transform testCommonDeckTransform;
    [SerializeField] private Transform testManagementDeckTransform;
    [SerializeField] private Transform testCompetenceDeckTransform;
    [SerializeField] private Transform socialLiftDeckTransform;

    public List<Card> testIQCardsList;
    public List<Card> testCommonCardsList;
    public List<Card> testManagementCardsList;
    public List<Card> testCompetenceCardsList;
    public List<Card> socialLiftCardsList;

    //debug
    public List<Card> allCardsList;

    public List<Card> SetActiveDeck(DeckType deckType)
    {
        switch (deckType)
        {
            case DeckType.TestIQ:
                return testIQCardsList;
            case DeckType.TestCommon:
                return testCommonCardsList;
            case DeckType.TestManagement:
                return testManagementCardsList;
            case DeckType.TestCompetence:
                return testCompetenceCardsList;
            default:
                return socialLiftCardsList;
        }        
    }

    private void Awake()
    {        
        LoadDecks();
    }

    private void LoadDecks()
    {
        for (int i = 0; i < testIQDeckTransform.childCount; i++)
        {
            testIQCardsList.Add(testIQDeckTransform.GetChild(i).GetComponent<Card>());            

            //debug
            allCardsList.Add(testIQCardsList[i]);
        }
        testIQCardsList = GetRandomDeck(testIQCardsList);

        for (int i = 0; i < testCommonDeckTransform.childCount; i++)
        {
            testCommonCardsList.Add(testCommonDeckTransform.GetChild(i).GetComponent<Card>());            

            //debug
            allCardsList.Add(testCommonCardsList[i]);
        }
        testCommonCardsList = GetRandomDeck(testCommonCardsList);

        for (int i = 0; i < testManagementDeckTransform.childCount; i++)
        {
            testManagementCardsList.Add(testManagementDeckTransform.GetChild(i).GetComponent<Card>());
            
            //debug
            allCardsList.Add(testManagementCardsList[i]);
        }
        testManagementCardsList = GetRandomDeck(testManagementCardsList);

        for (int i = 0; i < testCompetenceDeckTransform.childCount; i++)
        {
            testCompetenceCardsList.Add(testCompetenceDeckTransform.GetChild(i).GetComponent<Card>());            

            //debug
            allCardsList.Add(testCompetenceCardsList[i]);
        }
        testCompetenceCardsList = GetRandomDeck(testCompetenceCardsList);

        for (int i = 0; i < socialLiftDeckTransform.childCount; i++)
        {
            socialLiftCardsList.Add(socialLiftDeckTransform.GetChild(i).GetComponent<Card>());            

            //debug
            allCardsList.Add(socialLiftCardsList[i]);
        }
        socialLiftCardsList = GetRandomDeck(socialLiftCardsList);
    }    

    private List<Card> GetRandomDeck(List<Card> deck)
    {
        Random random = new Random();
        return deck.OrderBy(card => random.Next()).ToList();
    }    
}