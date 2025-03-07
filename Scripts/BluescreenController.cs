using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace pm_march_jamgame;

public class BluescreenController : IComponent
{
    public GameObject GameObject {get;init;}

    private float percentage = 0;
    private float finalSleepTime = 0;

    private bool firstFinal = true;

    public SpriteFont monogram {get;set;}

    public Game1 game;

    private TextSystem textSystem;
    private string[] swearList = ["Crap", "Shit", "No No No", "Stupid Machine"];

    public double startTime = 0;

    public BluescreenController(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public void Initialize()
    {
        if (game.SaveData.GameBootCount == 0)
        {
            textSystem = new TextSystem(TextObject.CreateLinesOfText("Not again...", 10, 
                new Vector2(400, 300), 4, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, 0.1f, new Color(0xff8800cc), new SpriteFont[] {monogram}));
        }
        else {
            textSystem = new TextSystem(TextObject.CreateLinesOfText(swearList[new Random().Next(0, swearList.Length)], 10, 
                new Vector2(400, 300), 4, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, 0.1f, new Color(0xff8800cc), new SpriteFont[] {monogram}));
        }
    }

    public void PreUpdate(GameTime gameTime)
    {
    }

    public void Update(GameTime gameTime)
    {
        if (gameTime.TotalGameTime.TotalSeconds <= startTime + 0.6) return;

        textSystem.Update(gameTime);

        if ((int)percentage == 99)
        {
            //Console.WriteLine("99");
            if (firstFinal) 
            {
                finalSleepTime = (float)gameTime.TotalGameTime.TotalSeconds + 2;
                firstFinal = false;
            }

            else if ((float)gameTime.TotalGameTime.TotalSeconds >= finalSleepTime) percentage = 100;
        }

        else if (percentage == 44)
        {
            percentage = 47;
        }

        else if (percentage < 80 && percentage >= 72)
        {
            percentage += new Random().Next(20, 40) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        else if (percentage < 34 && percentage >= 18)
        {
            percentage += new Random().Next(0, 10) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        else {
            percentage += new Random().Next(0, 20) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        if (percentage >= 100)
        {
            percentage = 100;

            if (Keyboard.GetState().GetPressedKeyCount() > 0)
            {
                if (game.SaveData.GameBootCount >= 1) {
                    if (OperatingSystem.IsWindows()) {
                        ProcessStartInfo startInfo = new ProcessStartInfo(System.IO.Directory.GetCurrentDirectory() + "\\ROOT");
                        startInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
                        startInfo.UseShellExecute = true;
                        Process.Start(startInfo);
                    }
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
        game.Bluescreen.Draw();

        string percentString = ((int)percentage).ToString();
        if ((int)percentage < 100) percentString = "0" + percentString;
        if ((int)percentage < 10)  percentString = "0" + percentString;

        spriteBatch.DrawString(monogram, percentString, new Vector2(70, 305), Color.White);
        textSystem.Draw(spriteBatch);
    }
}