using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace pm_march_jamgame;

// This class serves as the handling class for progressing and rendering dialogue text.
public class TextSystem
{
    private int finalDelayModifier = 8; // Final delay is calculated with (current TextObject.delay * finalDelayModifier).
    private int currentDialogue = 0;

    private TextObject[][] lines;
    private float newCharTimer = 0f;
    private int updatingLine = 0;

    public SoundEffect textBeep;

    public bool dialogueFinished = false;

    public TextSystem(TextObject[][] lines)
    {
        this.lines = lines;
    }

    public TextSystem(TextObject[] lines)
    {
        this.lines = new TextObject[][] {lines};
    }

    public void Update(GameTime gameTime)
    {
        if (lines.Length == 0) return;                  // Yes this has to be separated. No, it probably doesn't need to be checked, but better to be safe
        if (currentDialogue >= lines.Length || lines[currentDialogue].Length == 0) return; // than "Game.exe has stopped responding".

        // All lines of dialogue must be updated, as updates change rotations and bounce positions.
        for (int i = 0; i < lines[currentDialogue].Length; i++) {
            if (i > updatingLine) continue; // Wait before starting to update later lines.

            // Check if a new character needs to be added to the line.
            if (lines[currentDialogue][i].characters.Count < lines[currentDialogue][i].text.Length && 
                gameTime.TotalGameTime.TotalSeconds >= newCharTimer + lines[currentDialogue][i].delay) {
                newCharTimer = (float)gameTime.TotalGameTime.TotalSeconds;
                CharacterObject newCharacter;

                // Create a new character object for the next character, with a different constructor depending on color (or if random).
                if (lines[currentDialogue][i].defaultColor == null) {
                    newCharacter = new CharacterObject(lines[currentDialogue][i], lines[currentDialogue][i].characters.Count);
                }
                else {
                    newCharacter = new CharacterObject(lines[currentDialogue][i], lines[currentDialogue][i].characters.Count, 
                        (Color)lines[currentDialogue][i].defaultColor);
                }
                
                lines[currentDialogue][i].characters.Add(newCharacter);
                if (textBeep != null) textBeep.CreateInstance().Play();
            }

            // If at the end of the line, start updating the next one.
            if (lines[currentDialogue][i].characters.Count == lines[currentDialogue][i].text.Length && updatingLine == i) updatingLine += 1;

            // Actually update the objects.
            foreach (CharacterObject character in lines[currentDialogue][i].characters) {
                character.Update(gameTime);
            }

            // After all characters are displayed, and assuming this is the last piece of dialogue, wait for a second then stop showing it.
            if (updatingLine >= lines[currentDialogue].Length && currentDialogue >= lines.Length - 1 &&
                gameTime.TotalGameTime.TotalSeconds >= newCharTimer + (lines[currentDialogue][i].delay * finalDelayModifier)) {
                dialogueFinished = true;
                lines[currentDialogue] = new TextObject[0];
            } 
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if (currentDialogue >= lines.Length || lines[currentDialogue].Length == 0) return;

        // Loop through each character in each line and draw.
        for (int i = 0; i < lines[currentDialogue].Length; i++) {
            foreach (CharacterObject character in lines[currentDialogue][i].characters) {
                character.Draw(_spriteBatch);
            }
        }
    }

    // Change the current dialogue by one, provided the current dialogue is finished.
    // This could be expanded to finish the dialogue if it's not finished, but I can't be bothered doing that.
    public void IncreaseDialogueIndex()
    {
        if (currentDialogue >= lines.Length) return;

        if (lines[currentDialogue][lines[currentDialogue].Length - 1].characters.Count
            >= lines[currentDialogue][lines[currentDialogue].Length - 1].text.Length) {
            currentDialogue += 1;
        }
    }

    // This may not be needed, but useful if the place of dialogue needs to shift as needed.
    // Note that none of the protections from the IncreaseDialogueIndex function are included; 
    // this function will change the dialogue progress no matter what.
    public void SetDialogueIndex(int index)
    {
        currentDialogue = index;
    }
}