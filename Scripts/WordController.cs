using System;
using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pm_march_jamgame;

public class WordController : IComponent
{
    public GameObject GameObject {get;init;}

    public Game1 game {get;set;}
    
    private string[] thoughtText = new string[]
    {"Nearly finished with work today...", 
    "Can't wait to get out of here.", 
    "Work sucks...",
    "Just got to save this document and then I'm finished up.",
    "Just one more Control+S until the weekend."};

    private int currentTextIndex = 0;
    private Vector2 currentTextPosition;

    public SpriteFont monogram {get;set;}
    public SoundEffect thoughtBeep;
    public SoundEffect bluescreenCrash;

    public TextSystem textSystem;
    private Random random;

    public WordController(GameObject gameObject)
    {
        GameObject = gameObject;
        random = new Random();
    }

    public void Initialize()
    {        
        currentTextPosition = new Vector2(random.Next(20, 240), random.Next(290, 360));
        textSystem = new TextSystem(TextObject.CreateLinesOfText(thoughtText[currentTextIndex], 35, 
            currentTextPosition, 4, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, 0.1f, new Color(0xff8800cc), [monogram]));
        textSystem.textBeep = thoughtBeep;
    }

    public void PreUpdate(GameTime gameTime)
    {
    }

    public void Update(GameTime gameTime)
    {
        textSystem.Update(gameTime);

        if (textSystem.dialogueFinished && currentTextIndex < thoughtText.Length - 1) {
            currentTextIndex += 1;
            currentTextPosition = new Vector2(random.Next(20, 240), random.Next(290, 360));
            textSystem = new TextSystem(TextObject.CreateLinesOfText(thoughtText[currentTextIndex], 35, 
                currentTextPosition, 4, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, 0.1f, new Color(0xff8800cc), new SpriteFont[] {monogram}));
            textSystem.textBeep = thoughtBeep;
        }

        if (KeyboardExtended.KeyPressed(Keys.LeftControl) || KeyboardExtended.KeyPressed(Keys.RightControl))
        {
            bluescreenCrash.CreateInstance().Play();
            game.GameState = GameState.BLUESCREEN;
            System.Threading.Thread.Sleep(600);
        }
    }

    public void PostUpdate(GameTime gameTime)
    {
    }
}