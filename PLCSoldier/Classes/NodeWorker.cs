using PLCSoldier.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Classes
{
    public static class NodeWorker
    {
        public static void FindAllExpandedNodes(ObservableCollection<Node> nodes, List<string> result)
        {
            foreach (Node node in nodes)
            {
                if (node.IsExpanded == true && node.PathString != null)
                {
                    result.Add(node.PathString);
                }

                if (node.Subnodes != null && node.Subnodes.Count > 0)
                {
                    FindAllExpandedNodes(node.Subnodes, result);
                }
            }
        }
    }
}
