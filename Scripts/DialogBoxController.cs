using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace pm_march_jamgame;

public class DialogBoxController : IComponent
{
    public GameObject GameObject {get;init;}

    public SpriteFont arial;
    public SpriteFont monogram;
    public Game1 game;

    public string[] textOptions = 
    ["The system configuration has changed. Would you like to restart to confirm this configuration?",
    "The system configuration is invalid. No changes have been made. Please shut down the system and fix the configuration.", 
    "The system configuration is correct. Well done. Thanks for playing :)."];

    public Sprite Button;

    public Rectangle buttonHitbox;

    public Texture2D ok;
    public Texture2D okHover;

    private bool hover = false;

    private TextSystem textSystem;
    public SoundEffect thoughtBeep;
    
    private string[] swearList = ["Crap", "I'll have to try again.", "Stupid Machine", "I'd better get overtime for this.", "I hate this system."];

    public DialogBoxController(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public void Initialize()
    {
        if (game.GameState == GameState.DIALOGBOXINVALID)
        {
            textSystem = new TextSystem(TextObject.CreateLinesOfText(swearList[new Random().Next(0, swearList.Length)], 15, 
                new Vector2(400, 300), 4, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, 0.1f, new Color(0xff8800cc), new SpriteFont[] {monogram}));
            textSystem.textBeep = thoughtBeep;
        }
        
        if (game.GameState == GameState.DIALOGBOXFIXED)
        {
            textSystem = new TextSystem(TextObject.CreateLinesOfText("Finally, I can go home.", 15, 
                new Vector2(400, 300), 4, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, 0.1f, new Color(0xff8800cc), new SpriteFont[] {monogram}));
            textSystem.textBeep = thoughtBeep;
        }
    }

    public void PreUpdate(GameTime gameTime)
    {
    }

    public void Update(GameTime gameTime)
    {
        if (textSystem != null) textSystem.Update(gameTime);

        hover = false;
        MouseState mouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2((mouseState.X - ScreenScaling.topLeft.X) / ScreenScaling.scale, 
			(mouseState.Y - ScreenScaling.topLeft.Y) / ScreenScaling.scale);

        if (mousePosition.X >= buttonHitbox.Left && mousePosition.X <= buttonHitbox.Right && 
        mousePosition.Y >= buttonHitbox.Top && mousePosition.Y <= buttonHitbox.Bottom)
        {
            hover = true;
            
            // If the button is clicked.
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (game.GameState == GameState.DIALOGBOXFIXED) 
                {
                    game.GameState = GameState.FIXED;
                    game.StartCrash();
                }
                else 
                {
                    // gamestate is either invalid (retry) or fixed (game over).
                    System.Threading.Thread.Sleep(200);
                    System.Environment.Exit(0);
                }
            }
        }
    }

    public void PostUpdate(GameTime gameTime)
    {
    }

    public void ButtonDraw()
    {
        if (hover) Button.Texture = ok;
        else Button.Texture = okHover;

        Button.Draw();

        if (textSystem != null) textSystem.Draw(game.spriteBatch);
    }
}