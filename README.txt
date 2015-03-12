Gravity Ninja
Change Log:
March 12th:
- Ensured the user could only move off the level loading screen once all assets are loaded
- Made the loading message on the loading screen change to ready once loaded

March 11th:
- Added a label saying 'Loading...' and a busy symbol respresnting the game loading
- Change the game initalisation so that the bulk of it is done during the loading screen

March 10th:
- Implemeted a loading screen which currently displays a introduction/narrative to the game

March 6th:
- Added a sound for menu clicks

March 4th:
- Tweaked how the HUD was being created to improve FPS on the Vita

March 3rd:
- Added developer controls for use with the simulator (arrow keys)

March 2nd:
- Fixed the issue where the movement controls would invert

February 18th: 
- Add an additional camera mode which can be toggled between

February 16th:
- Add the score to the HUD
- Add the timer to the HUD
- Create the HUD and ensure it stays rotateded correctly

February 15th:
- Implement a timer and scoring system

February 13th:
- Simplify the collision detection method that's being used

February 11th:
- Test and optimise the game for the Vita
- Redesign and recreate the collision engine
- Make the camera rotate much more smoother to the desired angle
- Add our new sprites and new screens

February 9th:
- Include sounds and music in our game
- Design new sprites for our game

February 8th:
- Switch the player movement to be controlled by the Vita's acceleromter
- Adapt our solution to enable motion support

February 6th: 
- Design and implement the most efficient collision detective system
- Place the pickups strategically in the level

February 5th:
- Draw up title and death screens
- Impelement the level into the tile engine

February 3rd:
- Allow touch screen gestures to rotate the camera
- Set up a timer

February 2nd:
- Create a tile engine
- Design a level and come up with appropiate positions of pickups

February 1st:
- Implement gravity and basic movement

Janurary 30th: 
- Ensure the camera follows the player
- Create the base framework

Concept:
Simple 2D platformer where you are unable to control the player but are able to manipulate the world around him including being able to rotate, 
flip, change gravity the level he is on. The game will be played as a side scroller where you must navigate through a maze type map with all 
kinds of obstacles/puzzles/enemies. The main point of this game is to use/abuse physics to solve problems to complete the maze. The player 
will be constantly moving in one direction and you will be around to rotate the map with the touchscreen (or whatever) to avoid obstacles 
and get to your destination.
 
How it will be made:
The maze itself will be made from hundreds of rigid, non-moveable textured blocks. The method to create the maze from these blocks will be to 
read in from a text file containing a 100x100 (or bigger/smaller) map of numbers with each number representing either an empty square or a block. 
This means it looks more professional as it’s not hard-coded and it is easier to change the design of the maze or add more maze levels or add 
another block entity. The rotation feature will be done so the map doesn’t rotate but the camera does, giving the illusion that the map is 
rotating. Alongside this the gravity vector that the player has will need to change as the camera does, as does the direction vector the 
player is moving in. 

If the slope that the player is moving up is too steep the player will move much slower or will eventually travel backwards. 
Again if the slope of the maze is too shallow the player will move quicker down it. PHYSICS. Changing gravity is simple as you just invert the 
gravity vector. The player will have a speed float which can only be affected by the slopes of the maze (steeper = slower) and the players 
velocity. The player will have a default direction vector which will change a lot as the maze gets rotated. If that vector gets too steep it 
will trigger a bool that will invert it making the player travel in the opposite direction. The player will have a velocity/momentum float that 
makes the player fall and move more realistically. If the player is falling and lands on his head he will need to rotate so his feet are on 

the surface before he starts moving again (unless of course the user decides to make him fall again).The player will be a simple textured sprite and 
possibly animated. The blocks will be textured as grass towards the beginning of the maze at 0,0 and turn darker as the player progresses through. 
Behind the maze there will be a texture which again will be light and happy towards the beginning (probably clouds or something) and turn darker 
as you progress (eventually becomes hell). As for the HUD on the screen there will most likely be time left and distance that the player has done. 
There will also be a cool start and death menu, with the death menu displaying current score which will be calculated by __________ and high scores 
which will be read from file. 

Implementing smooth touch screen usage does seem quite hard but should be fine. All code should be commented pls and good variable names and stuff. 
We can work on the design of each part of the level or indiviual levels later.