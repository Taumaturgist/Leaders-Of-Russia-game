using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{       
    public string text;
    public DeckType type;
    public Effect effect;
    public int teleportTile;
    public Button button;    
    public string RuEffectText;

    [Header("DeckCompetence")]
    public int CardTileNum;

    [Header("DeckSocialLift")]
    public int RowUpTileNum;
    public int RowDownTileNum;
    public int SkipTurnTileNum;

    private string[] _ruEffectStrings =
    {
        "������� �� ������ ������", //0
        "������� �� ������ �����", //1
        "������� �� ������ �����", //2
        "������� �� ������� �����", //3
        "������� �� ������� ����", //4
        "������� � ������ ����", //5
        "",
    };

    private void Awake()
    {
        switch (effect)
        {
            case Effect.PlusOne:
                RuEffectText = _ruEffectStrings[0];
                break;
            case Effect.MinusOne:
                RuEffectText = _ruEffectStrings[1];
                break;
            case Effect.SkipTurn:
                RuEffectText = _ruEffectStrings[2];
                break;
            case Effect.RowUp:
                RuEffectText = _ruEffectStrings[3];
                break;
            case Effect.RowDown:
                RuEffectText = _ruEffectStrings[4];
                break;
            case Effect.Reset:
                RuEffectText = _ruEffectStrings[5];
                break;
            default:
                RuEffectText = "";
                break;
        }
    }
}

public enum DeckType
{
    TestIQ,
    TestCommon,
    TestManagement,
    TestCompetence,
    SocialLift
}
