Save System example from the game PapyBoom
------------------------------------------------------------------------------
For portfolio purpose, the files have been extracted from the project. They may require external classes to work completely.	
------------------------------------------------------------------------------

InventoryBase : scriptable object that handles the saving logic using Json and stores all the items owned by the player. In PapyBoom, we use 1 InventoryBase for unequiped items and 1 InventoryBase per playable charaters.

ItemDatabase : scriptable object that stores every existing item of the game (holding their int Id and their custom class ItemBase).

ItemBase : scriptable object that stores all data about an item (name, prefab, description, special effects...)

ItemEnums : stores useful enums.

Screenshot : shows Lucien's vestiaire where are displayed saved items.