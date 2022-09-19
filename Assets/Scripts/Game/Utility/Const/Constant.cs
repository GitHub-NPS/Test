using System.Collections.Generic;

public static class Constant
{
    public static Dictionary<HandType, string> HandType2Anim = new Dictionary<HandType, string>() { { HandType.Click, "Click" }, { HandType.Move, "idle" } };

    public static uint InitCoin = 1000;
    public static uint InitDiamond = 0;
}
