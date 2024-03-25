using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Buff
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 20;
    }
    public override void use()
    {
        player.setActualVit(player.getActualVit() + damage);
        if (player.getActualVit() > player.getVit())
            player.setActualVit(player.getVit());
        player.ActiveVitbar();
        Destroy(gameObject);
    }
}
