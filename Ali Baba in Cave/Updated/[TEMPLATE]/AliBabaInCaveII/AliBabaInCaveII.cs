﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class AliBabaInCaveII
    {
        #region YOUR CODE IS HERE

        #region FUNCTION#1: Calculate the Value
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given the Camels possible load and N items, each with its weight, profit and number of instances, 
        /// Calculate the max total profit that can be carried within the given camels' load
        /// </summary>
        /// <param name="camelsLoad">max load that can be carried by camels</param>
        /// <param name="itemsCount">number of items</param>
        /// <param name="weights">weight of each item [ONE-BASED array]</param>
        /// <param name="profits">profit of each item [ONE-BASED array]</param>
        /// <param name="instances">number of instances for each item [ONE-BASED array]</param>
        /// <returns>Max total profit</returns>
        static public int SolveValue(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances)
        {
            //REMOVE THIS LINE BEFORE START CODING
            //throw new NotImplementedException();

            int[,] dp = new int[itemsCount+1, camelsLoad+1];
            for (int i = 1; i <= itemsCount; i++)
            {
                for (int j = 0; j <= camelsLoad; j++)
                {
                    int maxProfitWithoutItem = dp[i-1, j];
                    int maxProfitWithItem = 0;
                    for (int k = 1; k <= instances[i]; k++)
                    {
                        if (j >= k * weights[i])
                        {
                            maxProfitWithItem = Math.Max(maxProfitWithItem, k * profits[i] + dp[i-1, j-k*weights[i]]);
                        }
                    }
                    dp[i, j] = Math.Max(maxProfitWithoutItem, maxProfitWithItem);
                }
            }
            return dp[itemsCount, camelsLoad];


            
        }
        #endregion

        #region FUNCTION#2: Construct the Solution
        //Your Code is Here:
        //==================
        /// <returns>Tuple array of the selected items to get MAX profit (stored in Tuple.Item1) together with the number of instances taken from each item (stored in Tuple.Item2)
        /// OR NULL if no items can be selected</returns>
        static public Tuple<int, int>[] ConstructSolution(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances)
        {
            //REMOVE THIS LINE BEFORE START CODING
            //throw new NotImplementedException();
            int[,] dp = new int[itemsCount+1, camelsLoad+1];
            for (int ii = 1; ii <= itemsCount; ii++)
            {
                for (int jj = 0; jj <= camelsLoad; jj++)
                {
                    int maxProfitWithoutItem = dp[ii-1, jj];
                    int maxProfitWithItem = 0;
                    for (int k = 1; k <= instances[ii]; k++)
                    {
                        if (jj >= k * weights[ii])
                        {
                            maxProfitWithItem = Math.Max(maxProfitWithItem, k * profits[ii] + dp[ii-1, jj-k*weights[ii]]);
                        }
                    }
                    dp[ii, jj] = Math.Max(maxProfitWithoutItem, maxProfitWithItem);
                }
            }

            int maxProfit = dp[itemsCount, camelsLoad];
            List<Tuple<int, int>> solutionList = new List<Tuple<int, int>>();
            int i = itemsCount;
            int j = camelsLoad;
            while (i > 0 && j > 0)
            {
                if (dp[i, j] == dp[i-1, j])
                {
                    i--;
                }
                else
                {
                    for (int k = 1; k <= instances[i]; k++)
                    {
                        if (j >= k * weights[i] && dp[i, j] == k * profits[i] + dp[i-1, j-k*weights[i]])
                        {
                            solutionList.Add(new Tuple<int, int>(i, k));
                            j -= k * weights[i];
                            break;
                        }
                    }
                    i--;
                }
            }

            /*
             
            int maxProfit = dp[itemsCount, camelsLoad];
            List<Tuple<int, int>> solutionList = new List<Tuple<int, int>>();
            int i = itemsCount;
            int j = camelsLoad;

            for (; i > 0 && j > 0; i--)
            {
                if (dp[i, j] == dp[i-1, j])
                {
                    continue;
                }
                else
                {
                    for (int k = 1; k <= instances[i]; k++)
                    {
                        if (j >= k * weights[i] && dp[i, j] == k * profits[i] + dp[i-1, j-k*weights[i]])
                        {
                            solutionList.Add(new Tuple<int, int>(i, k));
                            j -= k * weights[i];
                            break;
                        }
                    }
                }
            }
             
             */

            if (solutionList.Count == 0)
            {
                return null;
            }
            else
            {
                solutionList.Reverse();
                return solutionList.ToArray();
            }
        }
        #endregion

        #endregion
    }
}
