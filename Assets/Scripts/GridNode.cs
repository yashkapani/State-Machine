using UnityEngine;
using System.Collections.Generic;

namespace AISandbox {
    public class GridNode : MonoBehaviour {
        public Grid grid;
        public int column;
        public int row;

        private SpriteRenderer _renderer;
        private Color _orig_color;
        private Color _blocked_color;
        private Color _start_color;
        private Color _goal_color;
        private Color _path_color;
        private Color _calc_path_color;
        private Color _forest_color;
        private Color _swamp_color;

        [SerializeField]
        private bool _blocked = false;

        public enum NodeType { blocked, normal, forest, swamp };
        public enum NodeColor { start, goal, path, calcPath, blocked, normal, forest, swamp };

        public NodeType node_type;
        public NodeColor node_color;

        public bool blocked {
            get {
                return _blocked;
            }
            set {
                if (node_color == NodeColor.start || node_color == NodeColor.goal)
                    return;

                _blocked = value;
                _renderer.color = _blocked ? _blocked_color : _orig_color;
                node_color = _blocked ? NodeColor.blocked : NodeColor.normal;
            }
        }

        private void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
            _orig_color = _renderer.color;
            _blocked_color = new Color( _orig_color.r * 0.5f, _orig_color.g * 0.5f, _orig_color.b * 0.5f );
            _start_color = Color.red;
            _goal_color = Color.blue;
            _path_color = Color.green;
            _calc_path_color = Color.yellow;
            _forest_color = new Color(0.75f, 1.0f, 0.72f);
            _swamp_color = new Color(0.63f, 0.55f, 0.29f);

            node_type = NodeType.normal;
            node_color = NodeColor.normal;
        }

        private void Update()
        {
            if (node_color == NodeColor.start)
                //_renderer.color = _start_color;

            if (node_color == NodeColor.goal)
                _renderer.color = _goal_color;

            if (node_color == NodeColor.blocked)
                _renderer.color = _blocked_color;
            else if (node_color == NodeColor.path)
                _renderer.color = _path_color;
            else if (node_color == NodeColor.calcPath)
                _renderer.color = _calc_path_color;
            else if (node_color == NodeColor.normal)
                _renderer.color = _orig_color;
            else if (node_color == NodeColor.forest)
                _renderer.color = _forest_color;
            else if (node_color == NodeColor.swamp)
                _renderer.color = _swamp_color;


            //if(node_type == NodeType.normal)
            //{
            //    node_color = NodeColor.normal;
            //    _renderer.color = _orig_color;
            //}
            //else if (node_type == NodeType.blocked)
            //{
            //    node_color = NodeColor.blocked;
            //    _renderer.color = _blocked_color;
            //}
            //else if (node_type == NodeType.forest)
            //{
            //    node_color = NodeColor.forest;
            //    _renderer.color = _forest_color;
            //}
            //else if (node_type == NodeType.swamp)
            //{
            //    node_color = NodeColor.swamp;
            //    _renderer.color = _swamp_color;
            //}
        }

        public IList<GridNode> GetNeighbors( bool include_diagonal = false ) {
            return grid.GetNodeNeighbors( row, column, include_diagonal );
        }
    }
}