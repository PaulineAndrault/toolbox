# toolbox
useful scripts for unity game dev

- [TEMPLATE] BehaviorTree : core scripts, usable for all kind of projects


All scripts below are not directly re-usable, they are examples taken from several projects and may need some rewriting to be implemented elsewhere.

- [EXAMPLE] GameManager : example script from my ongoing commercial project Papy Boom. Architecture, flow of the project : the GM controls the instantiation of each loaded scene depending on their name and it stores persistent data between the different menu scenes and playable scenes (such as current playable characters selected, scores, current level...). This example may not be useful for every kind of games though.

- [EXAMPLE] Interface : this example comes from the mini game "FPSAudioExercice". The interface IUsable is implemented on interactable objects and allows the Player to detect and use this objects. You may use the same logic on other projects, or simply use the example to get the basic interface code and use it with other logic and other custom methods. 

- [EXAMPLE] SaveSystem :
From my ongoing commercial game Papy Boom. Use Json library to save current inventories of the Player (unequiped items) and of every playable characters (equiped items). Stored data is used during the initialization of the scenes when the Player re-opens the game or interact with an inventory.
