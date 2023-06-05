using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Game : MonoBehaviour
{
    //debug
    public List<Tile> EffectTilesList => _effectTilesList;
    public bool IsDevModeOn => isDevModeOn;

    [SerializeField] private Transform tileStorageTransfrom;
    [SerializeField] private List<Tile> tilesList;
    [SerializeField] private Transform playerChipTransform;
    [SerializeField] private float playerChipSpeed;
    [SerializeField] private SpriteRenderer playerChipSpriteRenderer;

    [SerializeField] private CamFollow cameraMover;
    [SerializeField] private int camDeltaX;

    [SerializeField] private CardDisplay cardDisplay;
    [SerializeField] private TileDisplay tileDisplay;

    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject fadePanel;

    [SerializeField] private DiceAnim diceAnim;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] playerChipTaps;
    [SerializeField] private AudioClip playerTeleportSound;

    [SerializeField] private bool isDevModeOn;

    private int _currentTileNum;    
    private Tween _tweenCardDisplay;
    private bool _isDiceRollAllowed = true;

    private Tile _targetTile;
    private Tile _walkingStarttile;
    private bool _isPlayerChipMoving;
    private bool _isPlayerTeleporting;

    private int _turnsCount;

    private List<Tile> _effectTilesList = new List<Tile>();

    private void Awake()
    {       
        for (int i = 0; i < tileStorageTransfrom.childCount; i++)
        {
            tilesList.Add(tileStorageTransfrom.GetChild(i).GetComponent<Tile>());
        } 
        
        //debug
        for (int i = 0; i < tilesList.Count; i++)
        {
            if (tilesList[i].Effect != Effect.None &&
                tilesList[i].Effect != Effect.DrawCard &&
                tilesList[i].Effect != Effect.Start &&
                tilesList[i].Effect != Effect.Finish)
            {
                _effectTilesList.Add(tilesList[i]);
            }
        }

        playerChipTransform.position = tilesList[0].transform.position;
        SetCameraOffsetX(tilesList[0].MoveDirection);        
    }
    public void RollD6()
    {
        if (!_isDiceRollAllowed)
        {
            return;
        }

        _turnsCount++;
        Debug.Log($"===TURNS COUNT: {_turnsCount}===");

        AllowDiceRoll(false);

        var randomInt = Random.Range(1, 7);
        Debug.Log($"Roll result: {randomInt}");

        diceAnim.SetDiceImage(randomInt);        

        _tweenCardDisplay?.Kill();
    }

    public void WalkTo(int distance)
    {
        _walkingStarttile = tilesList[_currentTileNum];
        _currentTileNum += distance;
        if (_currentTileNum > tilesList.Count - 1)
        {
            _currentTileNum = tilesList.Count - 1;
        }

        var currentTile = tilesList[_currentTileNum];
        
        _targetTile = currentTile;
        _isPlayerChipMoving = true;
        
        Debug.Log($"Walked to {currentTile.name}");

        SetCameraOffsetX(currentTile.MoveDirection);
    }

    public void TeleportTo(int destination)
    {
        audioSource.PlayOneShot(playerTeleportSound);

        _currentTileNum = destination;
        var currentTile = tilesList[_currentTileNum];        

        _targetTile = currentTile;
        _isPlayerTeleporting = true;

        SetCameraOffsetX(currentTile.MoveDirection);

        Debug.Log($"Teleported to Tile ({destination})");
    }

    public bool AllowDiceRoll(bool value)
    {
        return _isDiceRollAllowed = value;
    }

    public bool GetDiceRollStatus()
    {
        return _isDiceRollAllowed;
    }

    private void ProcessTileEffect(Tile tile)
    {
        switch (tile.Effect)
        {           
            case Effect.PlusOne:                
            case Effect.MinusOne:               
            case Effect.Reset:            
            case Effect.RowUp:                
            case Effect.RowDown:                
            case Effect.SkipTurn: 
                if (tile.IsTeleport)
                {
                    if (tile.TileText != null)
                    {
                        Debug.Log($"{tile.TileText} ({tile.Effect})");
                    }
                    fadePanel.SetActive(true);
                    DrawTile(tile);                    
                }                
                break;
            case Effect.Finish:
                Debug.Log("VICTORY!!!");
                fadePanel.SetActive(true);
                DeclareVictory();
                break;
            case Effect.DrawCard:
                if (tile.IsDeckTile)
                {
                    if (tile.TileText != null)
                    {
                        Debug.Log($"{tile.TileText} ({tile.Effect})");
                    }
                    Debug.Log($"Card Is Drawn from {tile.DeckType}");
                }
                fadePanel.SetActive(true);
                DrawCard(tile);
                break;
            default:
                AllowDiceRoll(true);
                break;
        }
    }
    
    private void SetCameraOffsetX(MoveDirection moveDirection)
    {
        if (moveDirection == MoveDirection.Right)
        {
            cameraMover.SetDeltaX(camDeltaX);
            playerChipSpriteRenderer.flipX = false;
        }
        else
        {
            cameraMover.SetDeltaX(-camDeltaX);
            playerChipSpriteRenderer.flipX = true;
        }
    }

    private void DrawCard(Tile tile)
    {        
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.ShowCard(tile);
    }

    private void DrawTile(Tile tile)
    {
        tileDisplay.gameObject.SetActive(true);
        tileDisplay.ShowTile(tile);
    }

    private void DeclareVictory()
    {
        victoryScreen.SetActive(true);
    }

    private void TransferChip()
    {        
        var targetPos = _targetTile.transform.position;

        if (_isPlayerChipMoving)
        {
            playerChipTransform.position = Vector3.MoveTowards(
                playerChipTransform.transform.position,
                tilesList[tilesList.IndexOf(_walkingStarttile) + 1].transform.position,
                playerChipSpeed * Time.deltaTime);

            if (playerChipTransform.position == tilesList[tilesList.IndexOf(_walkingStarttile) + 1].transform.position)
            {
                _walkingStarttile = tilesList[tilesList.IndexOf(_walkingStarttile) + 1];

                PlayRandomSound();
            }
        }

        if (_isPlayerTeleporting)
        {
            playerChipTransform.position = Vector3.MoveTowards(
            playerChipTransform.position,
            targetPos,
            playerChipSpeed * Time.deltaTime);
        }        

        if (playerChipTransform.position == targetPos)
        {
            if (_isPlayerChipMoving)
            {                
                ProcessTileEffect(_targetTile);
                _isPlayerChipMoving = false;
            }
            else if (_isPlayerTeleporting)
            {       
                AllowDiceRoll(true);
                _isPlayerTeleporting = false;

                PlayRandomSound();
            }            
        }
    }    

    private void FixedUpdate()
    {
        if (_isPlayerChipMoving || _isPlayerTeleporting)
        {
            TransferChip();
        }
    }

    private void PlayRandomSound()
    {
        var randomInt = Random.Range(0, playerChipTaps.Length);
        audioSource.PlayOneShot(playerChipTaps[randomInt]);
    }
}
