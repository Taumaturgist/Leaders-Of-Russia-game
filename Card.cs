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
        "Переход на клетку вперед", //0
        "Возврат на клетку назад", //1
        "Возврат на начало этапа", //2
        "Переход на уровень вверх", //3
        "Переход на уровень вниз", //4
        "Возврат в начало пути", //5
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
