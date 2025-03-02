using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace pm_march_jamgame;

public class BluescreenController : IComponent
{
    public GameObject GameObject {get;init;}

    private float percentage = 0;
    public SpriteFont monogram {get;set;}

    public Game1 game;

    public BluescreenController(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public void Initialize()
    {
    }

    public void PreUpdate(GameTime gameTime)
    {
    }

    public void Update(GameTime gameTime)
    {
        percentage += new Random().Next(0, 20) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (percentage >= 100)
        {
            percentage = 100;

            if (Keyboard.GetState().GetPressedKeyCount() > 0)
            {
                if (game.SaveData.GameBootCount >= 2) {
                    if (OperatingSystem.IsWindows()) System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory());
                }

                game.StartCrash();
            }
        }
    }

    public void PostUpdate(GameTime gameTime)
    {
    }

    public void DrawText(SpriteBatch spriteBatch)
    {
        string percentString = ((int)percentage).ToString();
        if ((int)percentage < 100) percentString = "0" + percentString;
        if ((int)percentage < 10)  percentString = "0" + percentString;

        spriteBatch.DrawString(monogram, percentString, new Vector2(70, 304), Color.White);
    }
}