using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceAnim : MonoBehaviour
{
    [SerializeField] private Sprite[] diceSprites;
    [SerializeField] private Transform diceTransform;
    [SerializeField] private Image diceImage;
    [SerializeField] private Transform centerDiceAnchor;
    [SerializeField] private Transform bottomDiceAnchor;
    [SerializeField] private float imageChangeDelay;
    [SerializeField] private int minTicksAmount;
    [SerializeField] private int maxTicksAmount;

    [SerializeField] private Transform centerHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform handTransform;
    [SerializeField] private Image rightHandImage;
    [SerializeField] private Sprite handWithDiceSprite;
    [SerializeField] private Sprite handEmptySprite;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] diceRollSounds;

    [SerializeField] private Game game;

    [SerializeField] private float diceMoveSpeed;
    [SerializeField] private float handMoveSpeed;

    private Vector3 _centerScale = new Vector3(0.9f, 0.9f, 1);
    private Vector3 _bottomScale = new Vector3(0.5f, 0.5f, 1);

    

    private int _currentValue = -1;
    private WaitForSeconds _delay;    
    private WaitForSeconds _longDelay;    

    private bool _isMovingAndScaling;
    private bool _isRightMoving = true;

    private void Awake()
    {
        _delay = new WaitForSeconds(imageChangeDelay);        
        _longDelay = new WaitForSeconds(imageChangeDelay * 10f);        

        diceTransform.position = bottomDiceAnchor.position;
        handTransform.position = rightHandTransform.position;
    }   

    public void SetDiceImage(int rollResult)
    {
        _isMovingAndScaling = false;
        StartCoroutine(GetRandomImage(rollResult));
    }

    private IEnumerator GetRandomImage(int rollResult)
    {
        _isRightMoving = false;

        while (handTransform.position != centerHandTransform.position)
        {
            yield return null;
        }

        yield return _delay;

        rightHandImage.sprite = handEmptySprite;
        diceTransform.position = centerDiceAnchor.position;
        diceTransform.localScale = _centerScale;

        PlayRandomSound();
        var randomTicksAmount = Random.Range(minTicksAmount, maxTicksAmount);

        for (int i = 0; i <= randomTicksAmount; i++)
        {
            yield return _delay;

            var randomInt = Random.Range(0, diceSprites.Length);

            while (randomInt == _currentValue)
            {
                randomInt = Random.Range(0, diceSprites.Length);
            }

            diceImage.sprite = diceSprites[randomInt];
            _currentValue = randomInt;
        }

        diceImage.sprite = diceSprites[rollResult - 1];    
        
        yield return _longDelay;
        _isMovingAndScaling = true;
        _isRightMoving = true;

        yield return _longDelay;
        game.WalkTo(rollResult);        
    }

    public void PlayRandomSound()
    {        
        var randomInt = Random.Range(0, diceRollSounds.Length);
        audioSource.PlayOneShot(diceRollSounds[randomInt]);   
    }

    private void FixedUpdate()
    {
        if (_isMovingAndScaling)
        {
            MoveAndScaleDice();
        }

        MoveHand();
    }

    private void MoveAndScaleDice()
    {
        diceTransform.position = Vector3.Lerp(
            diceTransform.position,
            bottomDiceAnchor.position,                       
            diceMoveSpeed * Time.deltaTime);

        diceTransform.localScale = Vector3.Lerp(
            diceTransform.localScale,
            _bottomScale,
            diceMoveSpeed * Time.deltaTime);
    }

    private void MoveHand()
    {
        if (_isRightMoving)
        {
            handTransform.position = Vector3.MoveTowards(
                handTransform.position,
                rightHandTransform.position,
                diceMoveSpeed * 200 * Time.deltaTime);
        }
        else
        {
            handTransform.position = Vector3.MoveTowards(
                handTransform.position,
                centerHandTransform.position,
                handMoveSpeed * Time.deltaTime);
        }

        if (handTransform.position == rightHandTransform.position)
        {
            rightHandImage.sprite = handWithDiceSprite;
        }
    }
}
