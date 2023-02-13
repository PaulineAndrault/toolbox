using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        // Constructors
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach(Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.FAILURE:
                        return NodeState.FAILURE;
                    default:
                        return NodeState.SUCCESS;
                }
            }

            return anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        }
    }
}

