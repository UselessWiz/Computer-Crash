using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pm_march_jamgame;

// This class holds all of the data necessary for the text system to render the text.
// The CharacterObject class holds specific data about individual letters (such as what it's progress through it's animation it is, etc).
public class CharacterObject
{
    TextObject text;
    public int index;
    public string character;

    SpriteFont font;
    Color textColor;
    
    float currentRotation;
    float rotationRate = 2f;
    float rotationSign = 1;
    float targetRotation;

    float currentHeight;
    float bounceRate = 4f;
    float heightSign = 1;
    float targetHeight;

    bool orientationFlip = false;
    Random random;

    public CharacterObject(TextObject textObject, int index)
    {
        this.text = textObject;
        this.index = index;
        this.character = textObject.text[index].ToString();

        random = new Random();

        this.textColor = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

        this.font = text.fonts[0];
    }

    public CharacterObject(TextObject textObject, int index, Color color)
    {
        this.text = textObject;
        this.index = index;
        this.character = textObject.text[index].ToString();
        this.textColor = color;

        random = new Random();

        this.font = text.fonts[0];
    }

    public void Update(GameTime gameTime)
    {
        // Update rotations based on parameters
        currentRotation += rotationRate * rotationSign * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (currentRotation >= targetRotation && rotationSign == 1 || currentRotation <= targetRotation && rotationSign == -1) {
            rotationSign *= -1;
            targetRotation = (float)random.NextDouble() * (text.maxRotation - text.minRotation) + text.minRotation; 
        }

        // Update height based on parameters.
        currentHeight += bounceRate * heightSign * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (currentHeight >= targetHeight && heightSign == 1 || currentHeight <= targetHeight && heightSign == -1) {
            heightSign *= -1;
            targetHeight = (float)random.NextDouble() * (text.maxBounce - text.minBounce) + text.minBounce;
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        Vector2 charCentre = font.MeasureString(character);
        charCentre = new Vector2(charCentre.X / 2, charCentre.Y / 2);
        _spriteBatch.DrawString(font, character, text.startPosition + new Vector2(index * (text.spacing + 8), currentHeight), 
            textColor, currentRotation, charCentre, 1, SpriteEffects.None, orientationFlip ? random.Next(1, 2) : 0);
    }

    // This is slightly obsolete because it was hard to read.
    // A previous iteration of the system had a single line with characters that replaced the old characters with newer ones, keeping the resulting text
    // on one line. This could still work, but would probably require a flash of total blackness over the letter for a couple frames before it gets replaced
    // or something like that. The current system works fine as far as weirdness goes so I'm not too concerned.
    public void ChangeCharacter(string character)
    {
        this.character = character;
    }
}