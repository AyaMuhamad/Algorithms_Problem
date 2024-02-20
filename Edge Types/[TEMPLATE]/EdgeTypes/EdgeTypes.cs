using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
  /*  class DFS_Shared_Variables
    {
        public Byte[] NodeColors;
        public int CountEdges;
        public int[] ParentNodes;

        public DFS_Shared_Variables(int length)
        {
            NodeColors = new Byte[length];
            ParentNodes = new int[length];
        }
    }
    */
    public static class EdgeTypes
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Detect & count the edge types of the given UNDIRECTED graph by applying COMPLETE-DFS on the entire graph 
        /// NOTE: during search, break ties (if any) by selecting the verices in ASCENDING numeric order
        /// </summary>
        /// <param name="vertices">array of vertices in the graph (named from 0 to |V| - 1)</param>
        /// <param name="edges">array of edges in the graph</param>
        /// <returns>return array of 3 numbers: outputs[0] number of backward edges, outputs[1] number of forward edges, outputs[2] number of cross edges</returns>
        public static int[] DetectEdges(int[] vertices, KeyValuePair<int, int>[] edges)
        {
            List<int>[] GraphListNodes = new List<int>[vertices.Length];

            //.. all nodes not visited by Make the boolean flag equals "false" 

            /*
                          #### Psoudo code ####
                   for each vertex u in Vertcies
                         color[u] = 0  => this indecate white color.

           */
            Byte[] _nodeColors_ = new Byte[vertices.Length]; // contains for each vertix 0 (white) OR 1 (Gray)
            /*  foreach (int vertex in vertices)
              {
                  nodeColors[vertex] =0;
              }*/

            // bool[] visited_Node = new bool[vertices.Length];
            /* for (int v = 0; v < vertices.Length; v++) // increase the time so the defualt of bool is false and i make all visited_Node false without loop.
             {
                 visited_Node[ v ]= false;
             }*/


            //.. make  Backword and Foroward and cross Edge counter intially values= zeros in each counter.
            // int backwardEdgeCount = 0, forwardEdgeCount = 0, crossEdgeCount = 0;
            // incremented when it arrive to Node not visited and Not parentNode.

            int _countEdges_ = 0;

            int[] parentNodes = new int[vertices.Length];
            //int[] Backword_Foroward_crossEdge_count = { 0, 0, 0 };


            // **Create the Graph**
            foreach (int Vertix in vertices)// ..add the vertices in the list.
            {
                GraphListNodes[Vertix] = new List<int>();
            }
            foreach (var Edge in edges) //.. For each edge, the loop is adding the "to" vertex to the adjacency list of the "from" vertex, and vice versa, since this is an undirected graph.
            {
                GraphListNodes[Edge.Key].Add(Edge.Value);
                GraphListNodes[Edge.Value].Add(Edge.Key);
            }

           // DFS_Shared_Variables SharedVars = new DFS_Shared_Variables(vertices.Length);

            /*
             ## ANOTHER LOGIC ##

              HashSet<int> visitedNodes = new HashSet<int>();
            int edgeCount = 0;

            for (int i = 0; i < vertices.Length; i++)
            {
                if (!visitedNodes.Contains(i))
                {
                    DFSAlgorithm(i, GraphList, visitedNodes, ref edgeCount, -1);
                }
            }

            return new int[] { edgeCount, edgeCount, 0 };
        }
             */


            // .. check if i visit this node or not yet.

            /*
                     #### Psoudo code ####

                for each vertex Adj in Vertcies
                    if color[Adj] ==0 then 
                     		DFS-Visit(Adj)
             */

            foreach (int _vertex_ in vertices)
            {
                if (_nodeColors_[_vertex_] == 0)
                {
                    /*
                        SharedVars.ParentNodes[vertex] = vertex;
                        DFSAlgorithm_Visit(vertex, GraphListNodes, SharedVars);
                     */

                    parentNodes[_vertex_] = _vertex_;
                    DFSAlgorithm_Visit(_vertex_, GraphListNodes, parentNodes, ref _countEdges_, _nodeColors_);
                }
            }


            /* foreach (int i in vertices)
             {
                 if (!visited_Node[i]) // if not yet => apply DFS
                 {
                     DFSAlgorithm(i, adjacency_list, visited_Node, ref countEdges, -100);
                 }
             }*/
            // .. finally return the count of each Edge
            return new int[] { _countEdges_, _countEdges_, 0 }; // BackwordCount = ForwordCount && CrossCount =0 becouse it Undirected graph.
        }

        private static void DFSAlgorithm_Visit(int vertix, List<int>[] GraphListNodes, int[] parentNode, ref int _countEdges_, Byte[] _nodeColors_)
        {
            //   visitedNodes.Add(vertex);
            // SharedVars.NodeColors[vertex] = 1;
            _nodeColors_[vertix] = 1; // become this node visited.   **discovered**

            //  loop adjacent vertex u of v "from LECTURE"

            /*
             
                  #### Psoudo code ####

                  for each v in Adj[Node]
                       if color[v] = WHITE then 	**new**
       	                   DFS-Visit(v)

             */

            foreach (int Adjacent in GraphListNodes[vertix])
            {
                // if (!visitedNodes.Contains(adjacent))
                if (_nodeColors_[Adjacent] == 0) // check it visited or not?
                {
                    parentNode[Adjacent] = vertix;
                    //if not .. applay DFS Algorithm on it again.
                    // DFSAlgorithm_Visit(adjacent, GraphListNodes, SharedVars);
                    DFSAlgorithm_Visit(Adjacent, GraphListNodes, parentNode , ref _countEdges_, _nodeColors_);
                }
                // if it visited.. it is the parent? so ignore it
                /*  else if (Adjacent == parentNode)
                  {
                      continue;
                  }*/
                // if it visited.. it is the parent? so check if this node backword , forword , cross or not all of them.
                //else if (adjacent != SharedVars.ParentNodes[vertex] && vertex < adjacent)
                else if (Adjacent != parentNode[vertix] && vertix < Adjacent)
                {
                    //  If u is visited and u is not v's parent, then classify the edge from v to u. && detect the backword age
                    _countEdges_++; //detect the backword age
                                    // else if (vertix > Adjacent) { forwardEdgeCount++; }//detect  the forward edge
                                    // else { crossEdgeCount++; } // detect cross edge
                }
            }
        }


        #endregion
    }
}
