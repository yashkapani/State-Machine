using UnityEngine;
using System.Collections;
using System;

namespace AISandbox
{
    public class GridGenerator : MonoBehaviour
    {
        public Grid grid;
        public GameObject _actor;

        public GameObject _key;
        public GameObject _door;
        public GameObject _door_open;
        public GameObject _treasure;

        void Start()
        {
            // Create and center the grid
            grid.Create(30, 50);
            Vector2 gridSize = grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            grid.transform.position = gridPos;

            GameObject actor = (GameObject)Instantiate(_actor, new Vector3(-17.0f, 0.6f, -1.0f), Quaternion.identity);
            actor.name = "TreasureSeeker";
            actor.transform.parent = transform;
            actor.gameObject.SetActive(true);
            actor.gameObject.GetComponentInChildren<Transform>().gameObject.SetActive(true);

            BuildTreasureMap();
        }

        private void BuildTreasureMap()
        {
            BuildTerrain();

            // Keys
            GameObject redKey = (GameObject)Instantiate(_key, new Vector3(15 * 0.64f, -3 * 0.64f, -1.0f), Quaternion.identity);
            redKey.name = "RedKey";
            redKey.transform.parent = transform;
            redKey.gameObject.SetActive(true);
            redKey.GetComponent<SpriteRenderer>().color = Color.red;

            GameObject blueKey = (GameObject)Instantiate(_key, new Vector3(15 * 0.64f, -19 * 0.64f, -1.0f), Quaternion.identity);
            blueKey.name = "BlueKey";
            blueKey.transform.parent = transform;
            blueKey.gameObject.SetActive(true);
            blueKey.GetComponent<SpriteRenderer>().color = Color.blue;

            GameObject greenKey = (GameObject)Instantiate(_key, new Vector3(-15 * 0.64f, 15 * 0.64f, -1.0f), Quaternion.identity);
            greenKey.name = "GreenKey";
            greenKey.transform.parent = transform;
            greenKey.gameObject.SetActive(true);
            greenKey.GetComponent<SpriteRenderer>().color = Color.green;

            // Closed Doors
            GameObject redDoor = (GameObject)Instantiate(_door, new Vector3(-25 * 0.64f, 19 * 0.64f, -1.0f), Quaternion.identity);
            redDoor.name = "RedDoor";
            redDoor.transform.parent = transform;
            redDoor.gameObject.SetActive(true);
            redDoor.GetComponent<SpriteRenderer>().color = Color.red;
            grid.GetNodethatActorIsOn(redDoor.transform.position).blocked = true;

            GameObject blueDoor = (GameObject)Instantiate(_door, new Vector3(-17 * 0.64f, -11 * 0.64f, -1.0f), Quaternion.identity);
            blueDoor.name = "BlueDoor";
            blueDoor.transform.parent = transform;
            blueDoor.gameObject.SetActive(true);
            blueDoor.GetComponent<SpriteRenderer>().color = Color.blue;
            grid.GetNodethatActorIsOn(blueDoor.transform.position).blocked = true;

            GameObject greenDoor = (GameObject)Instantiate(_door, new Vector3(-35 * 0.64f, -25 * 0.64f, -1.0f), Quaternion.identity);
            greenDoor.name = "GreenDoor";
            greenDoor.transform.parent = transform;
            greenDoor.gameObject.SetActive(true);
            greenDoor.GetComponent<SpriteRenderer>().color = Color.green;
            grid.GetNodethatActorIsOn(greenDoor.transform.position).blocked = true;

            // Doors Opened
            GameObject redDoorOpen = (GameObject)Instantiate(_door_open, new Vector3(-25 * 0.64f, 19 * 0.64f, -1.0f), Quaternion.identity);
            redDoorOpen.name = "RedDoorOpen";
            redDoorOpen.transform.parent = transform;
            redDoorOpen.gameObject.SetActive(true);
            redDoorOpen.GetComponent<SpriteRenderer>().color = Color.red;

            GameObject blueDoorOpen = (GameObject)Instantiate(_door_open, new Vector3(-17 * 0.64f, -11 * 0.64f, -1.0f), Quaternion.identity);
            blueDoorOpen.name = "BlueDoorOpen";
            blueDoorOpen.transform.parent = transform;
            blueDoorOpen.gameObject.SetActive(true);
            blueDoorOpen.GetComponent<SpriteRenderer>().color = Color.blue;

            GameObject greenDoorOpen = (GameObject)Instantiate(_door_open, new Vector3(-35 * 0.64f, -25 * 0.64f, -1.0f), Quaternion.identity);
            greenDoorOpen.name = "GreenDoorOpen";
            greenDoorOpen.transform.parent = transform;
            greenDoorOpen.gameObject.SetActive(true);
            greenDoorOpen.GetComponent<SpriteRenderer>().color = Color.green;

            // Treasure
            GameObject treasure = (GameObject)Instantiate(_treasure, new Vector3(35 * 0.64f, 25 * 0.64f, -1.0f), Quaternion.identity);
            treasure.name = "Treasure";
            treasure.transform.parent = transform;
            treasure.gameObject.SetActive(true);
            treasure.GetComponent<SpriteRenderer>().color = Color.black;
        }

        private void BuildTerrain()
        {
            // Setting forest nodes
            for (int row = 1; row < 5; row++)
                for (int col = 2; col < 30; col++)
                {
                    grid.GetNode(row, col).node_type = GridNode.NodeType.forest;
                    grid.GetNode(row, col).node_color = GridNode.NodeColor.forest;
                }
            for (int row = 9; row < 23; row++)
                for (int col = 2; col < 12; col++)
                {
                    grid.GetNode(row, col).node_type = GridNode.NodeType.forest;
                    grid.GetNode(row, col).node_color = GridNode.NodeColor.forest;
                }
            for (int row = 1; row < 20; row++)
                for (int col = 37; col < 49; col++)
                {
                    grid.GetNode(row, col).node_type = GridNode.NodeType.forest;
                    grid.GetNode(row, col).node_color = GridNode.NodeColor.forest;
                }

            // Setting swamp nodes
            for (int row = 6; row < 15; row++)
                for (int col = 19; col < 32; col++)
                {
                    grid.GetNode(row, col).node_type = GridNode.NodeType.swamp;
                    grid.GetNode(row, col).node_color = GridNode.NodeColor.swamp;
                }
            for (int row = 23; row < 29; row++)
                for (int col = 16; col < 50; col++)
                {
                    grid.GetNode(row, col).node_type = GridNode.NodeType.swamp;
                    grid.GetNode(row, col).node_color = GridNode.NodeColor.swamp;
                }

            // Setting blocked nodes
            {
                int col = 16;
                for (int row = 6; row < 20; row++)
                {
                    grid.GetNode(row, col).node_type = GridNode.NodeType.blocked;
                    grid.GetNode(row, col).node_color = GridNode.NodeColor.blocked;

                    grid.GetNode(row, col).blocked = true;
                }
            }
            {
                int col = 36;
                for (int row = 0; row < 15; row++)
                {
                    grid.GetNode(row, col).node_type = GridNode.NodeType.blocked;
                    grid.GetNode(row, col).node_color = GridNode.NodeColor.blocked;

                    grid.GetNode(row, col).blocked = true;
                }
            }
            {
                int row = 17;
                for (int col = 20; col < 50; col++)
                {
                    grid.GetNode(row, col).node_type = GridNode.NodeType.blocked;
                    grid.GetNode(row, col).node_color = GridNode.NodeColor.blocked;

                    grid.GetNode(row, col).blocked = true;
                }
            }
        }

        void Update()
        {

        }
    }
}
