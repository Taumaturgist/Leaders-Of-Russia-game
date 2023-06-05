using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileNum;
    public Transform AnchorTransform;
    public string TileText;
    public string RuTileText;
    public string RuDescriptionText;
    public string RuEffectText;
    public Effect Effect;
    public MoveDirection MoveDirection;
    public bool IsTeleport;
    public int TeleportDestination;
    public bool IsDeckTile;
    public DeckType DeckType;

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
        switch(Effect)
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

public enum MoveDirection
{
    Right,
    Left
}

public enum Effect
{
    None,
    Start,
    PlusOne,
    MinusOne,
    SkipTurn,
    RowUp,
    RowDown,
    Reset,
    DrawCard,
    Finish
}
