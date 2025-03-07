using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Diagnostics;
using System.IO;

namespace pm_march_jamgame;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    public SpriteBatch spriteBatch;

    public RenderTarget2D mainRenderTarget;
    public Rectangle upscaledDrawTarget;

    public GameState GameState = GameState.FSCHECK;
    private FileSystemUtils FileSystemUtils;

    public SaveData SaveData;

    private Sprite Word;
    public Sprite Bluescreen;
    private Sprite DialogBox;
    private Sprite Desktop;

    private SpriteFont Arial;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Adds an artificial pause so what happened can be processed.
        //System.Threading.Thread.Sleep(2000);

        Console.WriteLine(FileSystemUtils.GenerateState("ROOT"));

        // TODO: Add your initialization logic here
        graphics.IsFullScreen = true;
        graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        graphics.SynchronizeWithVerticalRetrace = true;

        graphics.ApplyChanges();

        mainRenderTarget = new RenderTarget2D(GraphicsDevice, 640, 480, 
            false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
        upscaledDrawTarget = ScreenScaling.ChangeResolution(graphics, new Point(640, 480));

        FileSystemUtils = new FileSystemUtils("ROOT");

        GameStateCheck();

        // DEBUG - MANUAL STATE SET - NOTE: IT IS VERY DANGEROUS TO SET THIS TO BLUESCREEN, AS IT MAY NOT EXIT FROM BLUESCREEN STATE.
        //GameState = GameState.GAMEPLAY;

        // Initialize objects
        spriteBatch = new SpriteBatch(GraphicsDevice);

        Word = new Sprite("Sprites/Word95", spriteBatch, Content);
        WordController wordController = new WordController(Word);
        wordController.monogram = Content.Load<SpriteFont>("Monogram");
        wordController.thoughtBeep = Content.Load<SoundEffect>("Audio/Thought-Beep");
        wordController.bluescreenCrash = Content.Load<SoundEffect>("Audio/BlueScreen-Crash");
        wordController.Initialize();
        wordController.game = this;
        Word.AttachComponent(wordController);

        Bluescreen = new Sprite("Sprites/Retro BSoD", spriteBatch, Content);
        BluescreenController bluescreenController = new BluescreenController(Bluescreen);
        bluescreenController.monogram = Content.Load<SpriteFont>("Monogram");
        bluescreenController.game = this;
        bluescreenController.Initialize();
        Bluescreen.AttachComponent(bluescreenController);

        DialogBox = new Sprite("Sprites/DialogBox", spriteBatch, Content);
        DialogBox.Position = new Vector2(320 - DialogBox.Texture.Width / 2, 240 - DialogBox.Texture.Height /2);
        DialogBoxController dialogBoxController = new DialogBoxController(DialogBox);
        dialogBoxController.arial = Content.Load<SpriteFont>("W95");
        dialogBoxController.monogram = Content.Load<SpriteFont>("Monogram");
        dialogBoxController.Button = new Sprite("Sprites/OK", spriteBatch, Content, new Vector2(DialogBox.Position.X + (DialogBox.Texture.Width - 30) / 2, 270));
        dialogBoxController.buttonHitbox = new Rectangle((int)dialogBoxController.Button.Position.X, (int)dialogBoxController.Button.Position.Y, 
            dialogBoxController.Button.Texture.Width, dialogBoxController.Button.Texture.Height);
        dialogBoxController.ok = Content.Load<Texture2D>("Sprites/OK-Hover");
        dialogBoxController.okHover = dialogBoxController.Button.Texture;
        dialogBoxController.game = this;
        dialogBoxController.Initialize();
        DialogBox.AttachComponent(dialogBoxController);

        Desktop = new Sprite("Sprites/Desktop", spriteBatch, Content);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        Arial = Content.Load<SpriteFont>("W95-Small");
    }

    protected override void Update(GameTime gameTime)
    {
        if (gameTime.TotalGameTime.TotalSeconds <= 2) return;

        if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)))// && GameState != GameState.BLUESCREEN)
            Exit();

        KeyboardExtended.PreUpdate();

        // TODO: Add your update logic here
        switch (GameState)
        {
            case GameState.GAMEPLAY:
                Word.Update(gameTime);
                break;
            
            case GameState.BLUESCREEN:
                // Display bluescreen with percentage timer ticking up, note to hit any key to complete restart once at 100%
                Bluescreen.Update(gameTime);
                break;

            case GameState.DIALOGBOXFIXED:
                // Display dialogue box with message along the lines of "config changed, restarting to confirm changes"
                DialogBox.Update(gameTime);
                break;

            case GameState.DIALOGBOXINVALID:
                // Display dialog box with message along the lines of "invalid configuration, nothing has changed".
                DialogBox.Update(gameTime);
                break;

            case GameState.FIXED:
                // boot to desktop with window saying thanks for playing.
                DialogBox.Update(gameTime);
                break;
        }

        base.Update(gameTime);

        KeyboardExtended.PostUpdate();
    }

    protected override void Draw(GameTime gameTime)
    {
        if (gameTime.TotalGameTime.TotalSeconds <= 2) return;

        GraphicsDevice.SetRenderTarget(mainRenderTarget);
        GraphicsDevice.Clear(new Color(0xac0000));

        // TODO: Add your drawing code here
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        switch (GameState)
        {
            case GameState.GAMEPLAY:
                // MS Word old version screenshot with a whole bunch of important work, as soon as control is pressed go to bluescreen.
                Word.Draw();
                spriteBatch.DrawString(Arial, DateTime.Now.TimeOfDay.ToString().Split(".")[0], new Vector2(580, 458), Color.Black);
                Word.GetComponent<WordController>().textSystem.Draw(spriteBatch);
                break;
            
            case GameState.BLUESCREEN:
                GraphicsDevice.Clear(Color.Black);
                if (gameTime.TotalGameTime.TotalSeconds <= Bluescreen.GetComponent<BluescreenController>().startTime + 0.6) break;
                Bluescreen.GetComponent<BluescreenController>().DrawText(spriteBatch);
                break;

            case >= (GameState)3:
                // Display dialogue box with message along the lines of "config changed, restarting to confirm changes"
                Desktop.Draw();
                spriteBatch.DrawString(Arial, DateTime.Now.TimeOfDay.ToString().Split(".")[0], new Vector2(580, 458), Color.Black);
                if (gameTime.TotalGameTime.TotalSeconds > 2)
                {
                    DialogBox.Draw();
                    DialogBoxController controller = DialogBox.GetComponent<DialogBoxController>();
                    spriteBatch.DrawString(controller.arial, TextObject.WrapText(controller.textOptions[(int)GameState - 3], 40), new Vector2(200, 205), Color.Black);
                    controller.ButtonDraw();
                }
                break;
/*
            case GameState.DIALOGBOXINVALID:
                // Display dialog box with message along the lines of "invalid configuration, nothing has changed".
                if (gameTime.TotalGameTime.TotalSeconds > 2)
                {
                    DialogBox.Draw();
                    DialogBoxController controller = DialogBox.GetComponent<DialogBoxController>();
                    spriteBatch.DrawString(controller.arial, controller.textOptions[(int)GameState - 3], new Vector2(150, 200), Color.Black);
                }
                break;

            case GameState.FIXED:
                // boot to desktop with window saying thanks for playing.
                if (gameTime.TotalGameTime.TotalSeconds > 2)
                {
                    DialogBox.Draw();
                    DialogBoxController controller = DialogBox.GetComponent<DialogBoxController>();
                    spriteBatch.DrawString(controller.arial, controller.textOptions[(int)GameState - 3], new Vector2(150, 200), Color.Black);
                }
                break;
*/
        }
        spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
        spriteBatch.Draw(mainRenderTarget, upscaledDrawTarget, Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }

    // Put the game in the correct state upon startup.
    private void GameStateCheck()
    {
        Console.WriteLine(FileSystemUtils.InitialState);

        FileSystemState fileSystemState = FileSystemUtils.CheckFileSystem();
        SaveData = new SaveData();

        // If this is the first time running the game, create the SYSMAN shortcut.
        if (SaveData.GameBootCount == 0) CreateShortcut();

        if (fileSystemState == FileSystemState.INITIAL)
        {
            // ORIGINAL FILE STATE
            GameState = GameState.GAMEPLAY;
        }
        else if (fileSystemState == FileSystemState.CORRECT)
        {
            // THIS RELIES ON THE GAMESTATE BEING SET TO FIXED BEFORE 'CRASH' ON THE FIRST "CORRECT" FIND.
            if ((GameState)SaveData.LastGameState == GameState.FIXED)
            {
                GameState = GameState.FIXED;
            }
            // THIS IS THE FIRST TIME.
            else
            {
                GameState = GameState.DIALOGBOXFIXED;
            }
        }
        else
        {
            // CONFIG INVALID
            GameState = GameState.DIALOGBOXINVALID;
        }
    }

    public void StartCrash()
    {
        SaveData.GameBootCount += 1;
        SaveData.LastGameState = (int)GameState;
        SaveData.WriteSaveData();

        Console.WriteLine(SaveData.GameBootCount);
        Process process = System.Diagnostics.Process.GetCurrentProcess();
        ProcessStartInfo startInfo = new ProcessStartInfo(process.MainModule.FileName);
        startInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
        startInfo.UseShellExecute = true;
        Process.Start(startInfo);

        Environment.Exit(0);
    }

    public void CreateShortcut()
    {
        string text = ConvertToHex(@$"PATH={System.IO.Directory.GetCurrentDirectory()}\ROOT\SYS\SYSMAN\SYSMAN.exe
DESC=Runs all System utilities upon startup (runs from STARTUP folder).");
        File.WriteAllText(@$"{System.IO.Directory.GetCurrentDirectory()}\ROOT\SYS\SYSMANSTARTUP.LINK", "0x" + text);
    }

    // Thanks Nano HE (https://stackoverflow.com/questions/5664345/string-to-binary-in-c-sharp)
    public static string ConvertToHex(string asciiString)
    {
        string hex = "";
        foreach (char c in asciiString)
        {
            int tmp = c;
            hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
        }
        return hex;
    }
}
