namespace pm_march_jamgame;

public enum GameState
{
    FSCHECK,            // Starting state, game just checks the file system. If in initial state, go to gameplay. Else, go to dialoguebox.
    GAMEPLAY,           // This might become an office app, I'm not sure yet.
    BLUESCREEN,         // Displays after Gameplay, just before attempting to save.
    DIALOGBOXFIXED,     // If Correct, display a restart checkbox, game restarts and goes to fixed. 
    DIALOGBOXINVALID,   // if wrong, say system config invalid and 'crash'
    FIXED,              // If correct and second boot.
}