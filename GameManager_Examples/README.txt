GameManager examples
------------------------------------------------------------------------------
For portfolio purpose, the files have been renamed. They should be called GameManager.cs in project assets (same name as the class) to work correctly.	
------------------------------------------------------------------------------

PapyBoomGameManager :
Game Manager of the mobile game Papy Boom, currently in preproduction.
This GM handles the correct initialization of the scenes based on which scene is loaded ; handles the save data and methods. 
Example : During the last session, the player played with the characters Lucien and Marvin (called "Boomers") and unlocked the level 3. When re-lauching PapyBoom, the GameManager checks the saved data, set the playable characters to Lucien and Marvin, get their current inventories and set the next level to Level 3.