using System.Collections.Generic;
using System.Linq;
using PopovRadio.Scripts.Gameplay.EventSystem.xNodeEditor.Nodes;
using UnityEngine;
using XNode;

namespace PopovRadio.Scripts.Gameplay.EventSystem.xNodeEditor
{
    [CreateAssetMenu]
    public class AssemblyTree : NodeGraph
    {
        #region Public Methods

        //Фукнция, возвращающая индексы детей данного элемента в дереве
        public List<EventNode> GetChildren(int index, IEnumerable<EventNode> availableNodes)
        {
            var eventNodes = new List<EventNode>();
            foreach (var node in availableNodes)
            {
                if (!node.HasPort("input")) continue;
                
                if (node.index != index || !node.Outputs.Any()) continue;

                var children = node.Outputs.ToArray();
                var connections = children[0].GetConnections();
                if (!connections.Any()) break;

                foreach (var connection in connections)
                {
                    var childEventNode = (EventNode) connection.node;
                    eventNodes.Add(childEventNode);
                }
            }

            return eventNodes;
        }

        // Функция, возвращающая все корни дерева
        public List<EventNode> GetRootNodes()
        {
            var rootNodes = new List<EventNode>();
            foreach (var node in nodes)
            {
                foreach (var input in node.Inputs)
                {
                    if (input.IsConnected) continue;
                    var eventNode = (EventNode) node;
                    rootNodes.Add(eventNode);
                }
            }

            return rootNodes;
        }

        #endregion
    }
}