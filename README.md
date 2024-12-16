# Project Shmup

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

### Student Info

-   Name: Ryan Zhang
-   Section: 206

## Game Design

-   Camera Orientation: _Topdown_
-   Camera Movement: _Vertically Scrolling_
-   Player Health: _Lives_
-   End Condition: _Player dies or kills the boss at the end._
-   Scoring: _Killing enemies_

### Game Description

_A brief explanation of your game. Inculde what is the objective for the player. Think about what would go on the back of a game box._

### Controls

-   Movement
    -   Up: W/Up Arrow
    -   Down: S/Down Arrow
    -   Left: A/Left Arrow
    -   Right: D/Right Arrow
-   Fire: Left-Mouse-Click/Z
-   Focus (Move slowly): Left-Shift

### Enemies
-   Standard Pigeon - Shoots directly at the player.
-   Wandered Pigeon - Leaves a trail of wandering bullets along its path that wander around the screen for a short time.
-   Streamed Pigeon - Shoots a bunch of bullets at once. Shot in a way that's common to bullet hells and forces the player to employ a bullet hell concept called "streaming."
-   Boss Pigeon - Big pigeon that spawns at the end of the level.
    - Shoots swarms of bullets at the player alternating between two shooting behaviors.
    - Moves back and forth at the top of the screen.
    - Required to kill to beat the game.

## You Additions

_List out what you added to your game to make it different for you_\
I plan on creating a basic level with some enemies and then making a 
bullet hell-style boss fight at the end.

## Sources

-   _List all project sources here –models, textures, sound clips, assets, etc._
-   _If an asset is from the Unity store, include a link to the page and the author’s name_
- Drawn by me in Clip Studio Paint.

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_
-   A NullReferenceException sometimes occurs when restarting the level due to the Game Object Pool needing to be reset and remade and some objects trying to access a pooled object during the reset process.
    - This doesn't impact the game in any notable way. And shouldn't be an issue when running. 

### Requirements not completed

_If you did not complete a project requirement, notate that here_
