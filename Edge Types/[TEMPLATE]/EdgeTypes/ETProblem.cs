using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GraphGenerator;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "EdgeTypes"; } }

        public override void TryMyCode()
        {            
            //Case0
            int[] vertices0 = { 0, 1, 2, 3, 4};
            KeyValuePair<int, int>[] edges0 = new KeyValuePair<int, int>[3];
            edges0[0] = new KeyValuePair<int, int>(0, 1);
            edges0[1] = new KeyValuePair<int, int>(1, 2);
            edges0[2] = new KeyValuePair<int, int>(4, 3);
            int[] expected0 = { 0, 0, 0 };
            int[] output0 = EdgeTypes.DetectEdges(vertices0, edges0);
            PrintCase(vertices0, edges0, output0, expected0);

            //Case1
            int[] vertices1 = { 0, 1, 2, 3, 4, 5 };
            KeyValuePair<int, int>[] edges1 = new KeyValuePair<int, int>[5];
            edges1[0] = new KeyValuePair<int, int>(0, 2);
            edges1[1] = new KeyValuePair<int, int>(0, 1);
            edges1[2] = new KeyValuePair<int, int>(1, 2);
            edges1[3] = new KeyValuePair<int, int>(4, 3);
            edges1[4] = new KeyValuePair<int, int>(5, 3);

            int[] expected1 = { 1, 1, 0 };
            int[] output1 = EdgeTypes.DetectEdges(vertices1, edges1);
            PrintCase(vertices1, edges1, output1, expected1);

            //Case2
            int[] vertices2 = { 0, 1, 2, 3, 4, 5, 6 };
            KeyValuePair<int, int>[] edges2 = new KeyValuePair<int, int>[8];
            edges2[0] = new KeyValuePair<int, int>(0, 1);
            edges2[1] = new KeyValuePair<int, int>(1, 2);
            edges2[2] = new KeyValuePair<int, int>(1, 3);
            edges2[3] = new KeyValuePair<int, int>(1, 4);
            edges2[4] = new KeyValuePair<int, int>(2, 4);
            edges2[5] = new KeyValuePair<int, int>(3, 4);
            edges2[6] = new KeyValuePair<int, int>(4, 5);
            edges2[7] = new KeyValuePair<int, int>(6, 3);
            int[] expected2 = { 2, 2, 0 };
            int[] output2 = EdgeTypes.DetectEdges(vertices2, edges2);
            PrintCase(vertices2, edges2, output2, expected2);

            //Case3
            int[] vertices3 = { 0, 1, 2, 3, 4, 5 };
            KeyValuePair<int, int>[] edges3 = new KeyValuePair<int, int>[6];
            edges3[0] = new KeyValuePair<int, int>(0, 1);
            edges3[1] = new KeyValuePair<int, int>(1, 2);
            edges3[2] = new KeyValuePair<int, int>(4, 3);
            edges3[3] = new KeyValuePair<int, int>(4, 5);
            edges3[4] = new KeyValuePair<int, int>(2, 4);
            edges3[5] = new KeyValuePair<int, int>(3, 1);
            int[] expected3 = { 1, 1, 0 };
            int[] output3 = EdgeTypes.DetectEdges(vertices3, edges3);
            PrintCase(vertices3, edges3, output3, expected3);

            //Case4
            int[] vertices4 = { 0, 1, 2, 3, 4, 5, 6 };
            KeyValuePair<int, int>[] edges4 = new KeyValuePair<int, int>[10];
            edges4[0] = new KeyValuePair<int, int>(0, 3);
            edges4[1] = new KeyValuePair<int, int>(0, 2);
            edges4[2] = new KeyValuePair<int, int>(0, 1);
            edges4[3] = new KeyValuePair<int, int>(1, 3);
            edges4[4] = new KeyValuePair<int, int>(2, 4);
            edges4[5] = new KeyValuePair<int, int>(3, 5);
            edges4[6] = new KeyValuePair<int, int>(4, 5);
            edges4[7] = new KeyValuePair<int, int>(4, 6);
            edges4[8] = new KeyValuePair<int, int>(6, 3);
            edges4[9] = new KeyValuePair<int, int>(5, 0);

            int[] expected4 = { 4,4,0};
            int[] output4 = EdgeTypes.DetectEdges(vertices4, edges4);
            PrintCase(vertices4, edges4, output4, expected4);
 
        }

        

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            int testCases;
            int[] actualResult = new int[3];
            int[] output = null;

            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            testCases = int.Parse(line);
   
            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                line = sr.ReadLine();
                int v = int.Parse(line);
                line = sr.ReadLine();
                int e = int.Parse(line);
                
                int[] vertices = new int[v];
                for (int k = 0; k < v; k++)
                {
                    vertices[k] = k;
                }
                var edges = new KeyValuePair<int, int>[e];
                for (int j = 0; j < e; j++)
                {
                    line = sr.ReadLine();
                    string[] lineParts = line.Split(',');
                    edges[j] = new KeyValuePair<int, int>(int.Parse(lineParts[0]), int.Parse(lineParts[1]));
                }
                line = sr.ReadLine();
                string[] results = line.Split(',');

                actualResult[0] = int.Parse(results[0]);
                actualResult[1] = int.Parse(results[1]);
                actualResult[2] = int.Parse(results[2]);
                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            Stopwatch sw = Stopwatch.StartNew();
                            output = EdgeTypes.DetectEdges(vertices, edges);
                            sw.Stop();
                            //PrintCase(vertices,edges, output, actualResult);
                            Console.WriteLine("|V| = {0}, |E| = {1}, time in ms = {2}", vertices.Length, edges.Length, sw.ElapsedMilliseconds);
                            Console.WriteLine("{0},{1},{2}", output[0], output[1], output[2]);

                        }
                        catch
                        {
                            caseException = true;
                            output = null;
                        }
                        caseTimedOut = false;
                    });

                    //StartTimer(timeOutInMillisec);
                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = int.Parse(sr.ReadLine().Split(':')[1]);
                    }
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
					tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (output[0] == actualResult[0] && output[1] == actualResult[1] && output[2] == actualResult[2])    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer = {0}, {1}, {2}, correct answer = {3}, {4}, {5}", output[0], output[1], output[2], actualResult[0], actualResult[1], actualResult[2]);
                    wrongCases++;
                }

                i++;
            }
            file.Close();
            sr.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0)); 
        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();

        }

        #endregion

        #region Helper Methods
        private static void PrintCase(int[] vertices, KeyValuePair<int, int>[] edges, int[] output, int[] expected)
        {
            Console.Write("Vertices: ");
            for (int i = 0; i < vertices.Length; i++)
            {
                Console.Write("{0}  ", vertices[i]);
            }
            Console.WriteLine();
            Console.WriteLine("Edges: ");
            for (int i = 0; i < edges.Length; i++)
            {
                Console.WriteLine("{0}, {1}", edges[i].Key, edges[i].Value);
            }
            Console.WriteLine("Outputs: # backward = {0}, # forward = {1}, # cross = {2}", output[0], output[1], output[2]);
            Console.WriteLine("Expected: # backward = {0}, # forward = {1}, # cross = {2}", expected[0], expected[1], expected[2]);
            if (output[0] == expected[0] && output[1] == expected[1] && output[2] == expected[2])    //Passed
            {
                Console.WriteLine("CORRECT");
            }
            else                    //WrongAnswer
            {
                Console.WriteLine("WRONG");
            }
            Console.WriteLine();
        }
        
        #endregion
   
    }
}
