# State Machine
Created a square grid at least 20x20 nodes in size.

Via mouse mark some of the nodes as "blocked".
Each grid  has a variable cost, randomly assigned (or via the mouse), at least 3 variations. For example, if each grid node represented terrain: road tiles cost 1, forest tiles cost 2, swamp tiles cost 3.

Created a single game entity with an AI that performs it's logic via a Finite State Machine (FSM).
The FSM attempts to collect the treasure. (Collect keys -> Open doors -> Collect Treasure).

The entity pathfinding  performs "seek"-like steering for the intermediate nodes along the path, and "arrival"-like steering for the goal node.

