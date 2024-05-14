Inside this folder are all the scripts needed to make a simple horror game.

lets go over all of them and what they do:

-Player Controller
	-Controls the players movement, look, and sounds.

-Sway
	-Moves objects that the player is holding, makes them move 
	 left, right, up, down, without aditional animations.

-Pistol
	-A basic pistol script (can be adapted for other weapons
	 as well) handles, shooting, reloading, animations, etc.

-Enemy Health
	-Handles the enemies health, and when takes damage, and
	 dies.

-Kill Player
	-Pretty much is game over when the player touches this
	 object.

-Use Chest
	-Handles opening the chest animations, and activates
	 whatever object you want to give the player.

-Door
	-The end game door that will check if the player has the 
	 key in order to complete the level/game.

-Key Pick Up
	-Handles picking up the key object.

-Lanturn Pick Up
	-Handles picking up the lanturn object.

-Enemy Controller
	-Controls the enemy to be idle, walk, and chase the player
	 as a NavMeshAgent.

-Cursor Control
	-The player controller locks the mouse cursor, this script
	 unlocks it for the main menu, and death screens

-Death Main Menu
	-Basic menus for restarting on death or starting the game.