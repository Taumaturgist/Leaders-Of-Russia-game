using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private GameObject cardDisplay;
    [SerializeField] private Transform anchorBackTransform;
    [SerializeField] private Transform anchorFaceTransform;
    [SerializeField] private Transform cardBackTransform;
    [SerializeField] private Transform cardFaceTransform;
    [SerializeField] private float cardMoveSpeed;

    [SerializeField] private CardStorage cardStorage;

    [SerializeField] private Image backImage;
    [SerializeField] private Sprite[] deckFaces;
    [SerializeField] private TextMeshProUGUI cardText;
    [SerializeField] private TextMeshProUGUI cardEffectText;

    [SerializeField] private Game game;

    [SerializeField] private AudioClip[] flipSounds;

    private bool _isAnimable;
    private Tween _tweenAnim;

    private Vector3 _faceStartPos;
    private Vector3 _backStartPos;

    private List<Card> _activeDeck;
    private Card _activeCard;
    private int[] _indexStorage = { 0, 0, 0, 0, 0 };

    private Tile _currentTile;

    private AudioSource _audioSource;

    //Debug    
    private int _debugIndex;    

    public void ShowCard(Tile tile)
    {
        //debug
        _debugIndex = 0;

        _currentTile = tile;
        var deckType = tile.DeckType;

        _activeDeck = cardStorage.SetActiveDeck(deckType);
        var activeDeckIndex = GetActiveDeckIndex(deckType);

        _activeCard = _activeDeck[_indexStorage[activeDeckIndex]];

        switch (_activeCard.type)
        {
            case DeckType.TestIQ:
                backImage.sprite = deckFaces[0];
                break;
            case DeckType.TestCommon:
                backImage.sprite = deckFaces[1];
                break;
            case DeckType.TestManagement:
                backImage.sprite = deckFaces[2];
                break;
            case DeckType.TestCompetence:
                backImage.sprite = deckFaces[3];
                break;
            case DeckType.SocialLift:
                backImage.sprite = deckFaces[4];
                break;
        }

        cardText.text = _activeCard.text;
        cardEffectText.text = _activeCard.RuEffectText;
        Debug.Log($"{cardText.text} ({_activeCard.effect})");

        if (_indexStorage[activeDeckIndex] < _activeDeck.Count)
        {
            _indexStorage[activeDeckIndex]++;
            Debug.Log($"card number is incremented to {_indexStorage[activeDeckIndex]}");
        }
        else
        {
            _indexStorage[activeDeckIndex] = 0;
        }

        _faceStartPos = cardFaceTransform.position;
        _backStartPos = cardBackTransform.position;
        _isAnimable = true;

        PlayRandomSound();
    }

    public void HideCard()
    {
        _tweenAnim?.Kill();
        _isAnimable = false;
        cardFaceTransform.position = _faceStartPos;
        cardBackTransform.position = _backStartPos;
        ProcessCardEffect(_activeCard, _currentTile);
        gameObject.SetActive(false);
    }

    public int GetActiveDeckIndex(DeckType deckType)
    {
        switch (deckType)
        {
            case DeckType.TestIQ:
                return 0;
            case DeckType.TestCommon:
                return 1;
            case DeckType.TestManagement:
                return 2;
            case DeckType.TestCompetence:
                return 3;
            default:
                return 4;
        }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void ProcessCardEffect(Card card, Tile tile)
    {
        switch (card.type)
        {
            default:
                game.TeleportTo(_activeCard.teleportTile);
                break;
            case DeckType.TestCompetence:
                switch (card.effect)
                {
                    case Effect.PlusOne:
                        game.TeleportTo(tile.tileNum + 1);
                        break;
                    case Effect.MinusOne:
                        game.TeleportTo(tile.tileNum - 1);
                        break;
                }
                break;
            case DeckType.SocialLift:
                ProcessSocialLiftEffect(card.effect, tile.tileNum);
                break;
        }
    }

    private void ProcessSocialLiftEffect(Effect effect, int tileNum)
    {
        switch(tileNum)
        {
            case 58:
                switch(effect)
                {
                    case Effect.RowDown:
                        game.TeleportTo(41);
                        break;
                    case Effect.RowUp:
                        game.TeleportTo(61);
                        break;
                    case Effect.SkipTurn:
                        game.TeleportTo(55);
                        break;
                }
                break;
            case 61:
                switch (effect)
                {
                    case Effect.RowDown:
                        game.TeleportTo(58);
                        break;
                    case Effect.RowUp:
                        game.TeleportTo(78);
                        break;
                    case Effect.SkipTurn:
                        game.TeleportTo(55);
                        break;
                }
                break;
            case 78:
                switch (effect)
                {
                    case Effect.RowDown:
                        game.TeleportTo(61);
                        break;
                    case Effect.RowUp:
                        game.TeleportTo(81);
                        break;
                    case Effect.SkipTurn:
                        game.TeleportTo(72);
                        break;
                }
                break;
            case 81:
                switch (effect)
                {
                    case Effect.RowDown:
                        game.TeleportTo(78);
                        break;
                    case Effect.RowUp:
                        game.TeleportTo(98);
                        break;
                    case Effect.SkipTurn:
                        game.TeleportTo(72);
                        break;
                }                
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_isAnimable)
        {
            MoveCardSides();
        }
    }    

    private void MoveCardSides()
    {
        cardBackTransform.position = Vector3.Lerp(
            cardBackTransform.position,
            anchorBackTransform.position,
            cardMoveSpeed * Time.deltaTime);

        cardFaceTransform.position = Vector3.Lerp(
            cardFaceTransform.position,
            anchorFaceTransform.position,
            cardMoveSpeed * Time.deltaTime);
    }    

    private void PlayRandomSound()
    {
        var randomInt = UnityEngine.Random.Range(0, flipSounds.Length);
        _audioSource.PlayOneShot(flipSounds[randomInt]);
    }

    //debug
    private void Update()
    {
        DebugCardText();
    }

    //debug
    private void DebugCardText()
    {
        if (!game.IsDevModeOn)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            cardText.text = cardStorage.allCardsList[_debugIndex].text;
            cardEffectText.text = cardStorage.allCardsList[_debugIndex].RuEffectText;

            _debugIndex++;
            if (_debugIndex >= cardStorage.allCardsList.Count)
            {
                _debugIndex = 0;
            }
        }
    }
}
