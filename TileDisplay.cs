using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileDisplay : MonoBehaviour
{
    [SerializeField] private GameObject tileDisplay;
    [SerializeField] private Transform tileFaceTransform;
    [SerializeField] private Transform tileAnchorTransform;
    [SerializeField] private float tileMoveSpeed;

    [SerializeField] private Image tileFaceImage;
    [SerializeField] private TextMeshProUGUI tileLoreText;
    [SerializeField] private TextMeshProUGUI tileDescriptionText;
    [SerializeField] private TextMeshProUGUI tileEffectText;
    [SerializeField] private Sprite colorRed;
    [SerializeField] private Sprite colorGreen;

    [SerializeField] private Game game;

    [SerializeField] private AudioClip[] flipSounds;

    private bool _isAnimable;
    private Vector3 _faceStartPos;

    private Tile _currentTile;

    private AudioSource _audioSource;

    //Debug    
    private int _debugIndex;

    public void ShowTile(Tile tile)
    {
        _currentTile = tile;
        var effect = tile.Effect;

        tileLoreText.text = tile.RuTileText;
        tileDescriptionText.text = tile.RuDescriptionText;
        tileEffectText.text = tile.RuEffectText;

        switch(effect)
        {
            case Effect.PlusOne:
            case Effect.RowUp:
                tileFaceImage.sprite = colorGreen;
                break;
            case Effect.MinusOne:
            case Effect.RowDown:
            case Effect.SkipTurn:
            case Effect.Reset:
                tileFaceImage.sprite = colorRed;
                break;
        }

        _faceStartPos = tileFaceTransform.position;
        _isAnimable = true;

        PlayRandomSound();
    }

    public void HideTile()
    {
        _isAnimable = false;
        tileFaceTransform.position = _faceStartPos;
        game.TeleportTo(_currentTile.TeleportDestination);
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (_isAnimable)
        {
            MoveTileFace();
        }
    }    

    private void MoveTileFace()
    {
        tileFaceTransform.position = Vector3.Lerp(
            tileFaceTransform.position,
            tileAnchorTransform.position,
           tileMoveSpeed * Time.deltaTime);
    }

    private void PlayRandomSound()
    {
        var randomInt = Random.Range(0, flipSounds.Length);
        _audioSource.PlayOneShot(flipSounds[randomInt]);
    }

    //debug
    private void Update()
    {
        DebugTileText();
    }

    //debug
    private void DebugTileText()
    {
        if (!game.IsDevModeOn)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            tileLoreText.text = game.EffectTilesList[_debugIndex].RuTileText;
            tileDescriptionText.text = game.EffectTilesList[_debugIndex].RuDescriptionText;
            tileEffectText.text = game.EffectTilesList[_debugIndex].RuEffectText;

            _debugIndex++;
            if (_debugIndex >= game.EffectTilesList.Count)
            {
                _debugIndex = 0;
            }
        }
    }

}
