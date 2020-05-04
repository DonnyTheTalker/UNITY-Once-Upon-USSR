using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public Sprite DamageSprite;
    public int HP = 3;

    public AudioClip ChopSound1;
    public AudioClip ChopSound2;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int loss)
    { 
        SoundManager.Instance.RandomizeSfx(ChopSound1, ChopSound2);

        _spriteRenderer.sprite = DamageSprite;
        HP -= loss;
        if (HP <= 0)
            gameObject.SetActive(false); 
    }


}
