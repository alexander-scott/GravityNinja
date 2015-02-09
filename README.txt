Danger Ducks #SWAG
TODO: Add accelerometer support, add gesture support, ensure when the camera is flipped via gestures that the duck is rotated appropiately.
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
