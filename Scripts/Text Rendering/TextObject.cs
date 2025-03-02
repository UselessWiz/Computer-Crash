using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace pm_march_jamgame;

// This class holds all of the data necessary for the text system to render the text.
// The CharacterObject class holds specific data about individual letters (such as what it's progress through it's animation it is, etc).
public class TextObject
{
    public string text;
    public List<CharacterObject> characters = new List<CharacterObject>();

    public SpriteFont[] fonts;
    public string[] fontNames;
    
    // Text parameters.
    public Vector2 startPosition; // Position of the top most line
    public int spacing; // Distance between letters.

    public float minRotation; // The lowest and highest values that can be rotated towards.
    public float maxRotation;
    public float minRotationRate; // The lowest and highest speed of rotation.
    public float maxRotationRate;

    public float minBounce; // The lowest and highest Y values letters can bounce to.
    public float maxBounce;
    public float minBounceRate; // The lowest and highest speed of bounces.
    public float maxBounceRate;

    public float delay; // The time between letter spawns.

    public Color? defaultColor = null; // The default colour that letters will take (this can be change on a per-letter basis for weird effects).

    // Constructing the object in code (includes color)
    public TextObject(string text, Vector2 startPosition, int spacing, Vector2 rotationBounds, Vector2 rotationRateBounds, 
        Vector2 bounceBounds, Vector2 bounceRateBounds, float delay, Color color)
    {
        this.text = text;
        this.startPosition = startPosition;
        this.spacing = spacing;
        this.minRotation = rotationBounds.X;
        this.maxRotation = rotationBounds.Y;
        this.minRotationRate = rotationRateBounds.X;
        this.maxRotationRate = rotationRateBounds.Y;
        this.minBounce = bounceBounds.X;
        this.maxBounce = bounceBounds.Y;
        this.minBounceRate = bounceRateBounds.X;
        this.maxBounceRate = bounceRateBounds.Y;
        this.delay = delay;
        this.defaultColor = color;
    }

    // Constructing the object in code (does not include color)
    public TextObject(string text, Vector2 startPosition, int spacing, Vector2 rotationBounds, Vector2 rotationRateBounds, 
        Vector2 bounceBounds, Vector2 bounceRateBounds, float delay)
    {
        this.text = text;
        this.startPosition = startPosition;
        this.spacing = spacing;
        this.minRotation = rotationBounds.X;
        this.maxRotation = rotationBounds.Y;
        this.minRotationRate = rotationRateBounds.X;
        this.maxRotationRate = rotationRateBounds.Y;
        this.minBounce = bounceBounds.X;
        this.maxBounce = bounceBounds.Y;
        this.minBounceRate = bounceRateBounds.X;
        this.maxBounceRate = bounceRateBounds.Y;
        this.delay = delay;
    }

    // Used to set up strings in code. Should be used instead of flat creating an object.
    public static TextObject[] CreateLinesOfText(string text, int maxLength, Vector2 position, int spacing, Vector2 rotationBounds, Vector2 rotationRateBounds,
        Vector2 bounceBounds, Vector2 bounceRateBounds, float delay, Color color, SpriteFont[] fonts)
    {
        // Set up the number of lines and the text wrapping to create lines.
        text = WrapText(text, maxLength);
        string[] splitText = text.Split("\n");

        TextObject[] lines = new TextObject[splitText.Length];

        // Create each object based on the content of the lines.
        for (int i = 0; i < splitText.Length; i++) {
            lines[i] = new TextObject(splitText[i], position + new Vector2(0, i * 15), spacing, rotationBounds, 
                rotationRateBounds, bounceBounds, bounceRateBounds, delay, color);
        }

        // Add the relevant font list to each object (Yes, this could be done in the constructor, no I am not bothered doing that).
        for (int i = 0; i < lines.Length; i++) {
			lines[i].fonts = fonts;
		}

        return lines;
    }

    // Same as above, but does not include colour data (will use a random colour).
    public static TextObject[] CreateLinesOfText(string text, int maxLength, Vector2 position, int spacing, Vector2 rotationBounds, Vector2 rotationRateBounds,
        Vector2 bounceBounds, Vector2 bounceRateBounds, float delay, SpriteFont[] fonts)
    {
        // Set up the number of lines and the text wrapping to create lines.
        text = WrapText(text, maxLength);
        string[] splitText = text.Split("\n");

        TextObject[] lines = new TextObject[splitText.Length];

        // Create each object based on the content of the lines.
        for (int i = 0; i < splitText.Length; i++) {
            lines[i] = new TextObject(splitText[i], position + new Vector2(0, i * 15), spacing, rotationBounds, 
                rotationRateBounds, bounceBounds, bounceRateBounds, delay);

            // Add the relevant font list to each object (Yes, this could be done in the constructor. No, I am not bothered doing that).
            lines[i].fonts = fonts; 
        }

        return lines;
    }

    // Used by the above functions to wrap the text.
    private static string WrapText(string text, int maxLength)
	{
		int lastWrapIndex = 0;

        // This code may be bad, but it works (Copy pasted from previous project). 
		for (int i = 1; i < text.Length; i++) {
            // Check if the line is too long based on the length counter (last wrap index).
			if (i - lastWrapIndex == maxLength) {
                // Backtrack from this point until a space is found, then add a new line after that.
				for (int j = i; j > 0; j--) {
					if (j >= text.Length) break;
					if (text[j] == ' ') {
						text = text.Insert(j + 1, "\n");
                        // Reset the length counter.
						lastWrapIndex = j;
						break;
					}
				}
			}
		}

		return text;
    }
}
