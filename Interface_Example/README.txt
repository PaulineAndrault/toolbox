Interface Example
------------------------------------------------------------------------------
This example comes from the FPS Audio Exercice project I did during school. I created and implemented an interface called IUsable to allow the player to interact with all kind of interactable game objects.
------------------------------------------------------------------------------

IUsable : the interface with a blank Use() method.

Door and NPCTalker : the main classes of the two interactable game objects. They both implement the IUsable interface and its Use() method.

PlayerInteraction : the player's component that detects if an interactable object (= an object implementing the IUsable interface) is in front of them and allows them to interact with (activating the Use() method). In this project, the player is allowed to open/close the nightclub's door and to talk to the NPC.


