using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{

    public static class MatrixMultiplication
    {
        #region YOUR CODE IS HERE
        static public int[,] MatrixMultiply(int[,] M1, int[,] M2, int N)
        {

            int[,] Matrix = new int[N, N];

            // handle possible general cases
            if ( M1.GetLength(1) != M2.GetLength(0))
            {
                return new int[0, 0];
            }

            if (N <= 64 || N % 2 != 0)
            {
                //NaiveSolutionForMatrecis
                if (N <= 4)
                {
                    return NaiveSolutionForMatrecisLess2(M1, M2, N);
                }
                else
                {
                    return NaiveSolutionForMatrecis(M1, M2, N);
                }
            }
            else
            {
                int newSize = N / 2;
                // create 8 sub matrices to put each part form 4 parts of each matrix on it each part is N/2 
                int[,] XfirstQuartwer = new int[newSize, newSize], XSecondQuartwer = new int[newSize, newSize], XThirdQuartwer = new int[newSize, newSize],
                      XFourthQuartwer = new int[newSize, newSize], YfirstQuartwer = new int[newSize, newSize], YSecondQuartwer = new int[newSize, newSize],
                       YThirdQuartwer = new int[newSize, newSize], YFourthQuartwer = new int[newSize, newSize];

                /*
                Task d1 = new Task(() => devide(M1, XfirstQuartwer, 0, 0, newSize));
                Task d2 = new Task(() => devide(M1, XSecondQuartwer, 0, newSize, newSize));
                Task d3 = new Task(() => devide(M1, XThirdQuartwer, newSize, 0, newSize));
                Task d4 = new Task(() => devide(M1, XFourthQuartwer, newSize, newSize, newSize));

                Task d5 = new Task(() => devide(M2, YfirstQuartwer, 0, 0, newSize));
                Task d6 = new Task(() => devide(M2, YSecondQuartwer, 0, newSize, newSize));
                Task d7 = new Task(() => devide(M2, YThirdQuartwer, newSize, 0, newSize));
                Task d8 = new Task(() => devide(M2, YFourthQuartwer, newSize, newSize, newSize));

                d1.Start(); d2.Start(); d3.Start(); d4.Start(); d5.Start(); d6.Start(); d7.Start(); d8.Start();

                d1.Wait(); d2.Wait(); d3.Wait(); d4.Wait(); d5.Wait(); d6.Wait(); d7.Wait(); d8.Wait();
                */

                //divide each matrix into 4 small matrecies
                for (int i = 0; i < newSize; i++)
                {
                    for (int j = 0; j < newSize; j++)
                    {
                        // First Matrix
                        XfirstQuartwer[i, j] = M1[i, j];
                        XSecondQuartwer[i, j] = M1[i, j + newSize];
                        XThirdQuartwer[i, j] = M1[i + newSize, j];
                        XFourthQuartwer[i, j] = M1[i + newSize, j + newSize];

                        // Secound Matrix
                        YfirstQuartwer[i, j] = M2[i, j];
                        YSecondQuartwer[i, j] = M2[i, j + newSize];
                        YThirdQuartwer[i, j] = M2[i + newSize, j];
                        YFourthQuartwer[i, j] = M2[i + newSize, j + newSize];
                    }
                }

                /*
                // Divide each matrix into 4 sub matrices
                devide(M1, XfirstQuartwer, 0, 0, newSize);
                devide(M1, XSecondQuartwer, 0, newSize, newSize);
                devide(M1, XThirdQuartwer, newSize, 0, newSize);
                devide(M1, XFourthQuartwer, newSize, newSize, newSize);

                devide(M2, YfirstQuartwer, 0, 0, newSize);
                devide(M2, YSecondQuartwer, 0, newSize, newSize);
                devide(M2, YThirdQuartwer, newSize, 0, newSize);
                devide(M2, YFourthQuartwer, newSize, newSize, newSize);
                */
                // Make 7 products for Strassen’s method in parallel becouse no data dependncy here
                Task<int[,]> P1 = new Task<int[,]>(() => MatrixMultiply(AddtionMatrecis(XfirstQuartwer, XFourthQuartwer, newSize), AddtionMatrecis(YfirstQuartwer, YFourthQuartwer, newSize), newSize));
                Task<int[,]> P2 = new Task<int[,]>(() => MatrixMultiply(AddtionMatrecis(XThirdQuartwer, XFourthQuartwer, newSize), YfirstQuartwer, newSize));
                Task<int[,]> P3 = new Task<int[,]>(() => MatrixMultiply(XfirstQuartwer, SubtractionMatrecis(YSecondQuartwer, YFourthQuartwer, newSize), newSize));
                Task<int[,]> P4 = new Task<int[,]>(() => MatrixMultiply(XFourthQuartwer, SubtractionMatrecis(YThirdQuartwer, YfirstQuartwer, newSize), newSize));
                Task<int[,]> P5 = new Task<int[,]>(() => MatrixMultiply(AddtionMatrecis(XfirstQuartwer, XSecondQuartwer, newSize), YFourthQuartwer, newSize));
                Task<int[,]> P6 = new Task<int[,]>(() => MatrixMultiply(SubtractionMatrecis(XThirdQuartwer, XfirstQuartwer, newSize), AddtionMatrecis(YfirstQuartwer, YSecondQuartwer, newSize), newSize));
                Task<int[,]> P7 = new Task<int[,]>(() => MatrixMultiply(SubtractionMatrecis(XSecondQuartwer, XFourthQuartwer, newSize), AddtionMatrecis(YThirdQuartwer, YFourthQuartwer, newSize), newSize));

                P1.Start(); P2.Start(); P3.Start(); P4.Start(); P5.Start(); P6.Start(); P7.Start();

                P1.Wait(); P2.Wait(); P3.Wait(); P4.Wait(); P5.Wait(); P6.Wait(); P7.Wait();
                /*
                // calculate r,s,t and u in parallel also..
                 Task<int[,]> r = new Task<int[,]>(() => AddtionMatrecis(SubtractionMatrecis(AddtionMatrecis(P1.Result, P4.Result, newSize), P5.Result, newSize), P7.Result, newSize)); //r= P5 + P4 – P2 + P6   
                 Task<int[,]> s = new Task<int[,]>(() => AddtionMatrecis(P3.Result, P5.Result, newSize)); //s	= P1 + P2
                 Task<int[,]> t = new Task<int[,]>(() => AddtionMatrecis(P2.Result, P4.Result, newSize)); //t	= P3  + P4
                 Task<int[,]> u = new Task<int[,]>(() => AddtionMatrecis(SubtractionMatrecis(AddtionMatrecis(P1.Result, P3.Result, newSize), P2.Result, newSize), P6.Result,newSize)); //u = P5 + P1 – P3 – P7

                 r.Start(); s.Start(); t.Start(); u.Start();

                 r.Wait(); s.Wait(); t.Wait(); u.Wait();
                */
                /*
                Task c1 = new Task(() => combineSolutions(r.Result, Matrix, 0, 0, newSize));
                Task c2 = new Task(() => combineSolutions(s.Result, Matrix, 0, newSize, newSize));
                Task c3 = new Task(() => combineSolutions(t.Result, Matrix, newSize, 0, newSize));
                Task c4 = new Task(() => combineSolutions(u.Result, Matrix, newSize, newSize, newSize));

                c1.Start(); c2.Start(); c3.Start(); c4.Start();

                c1.Wait(); c2.Wait(); c3.Wait(); c4.Wait();
                */

                int[,] r = AddtionMatrecis(SubtractionMatrecis(AddtionMatrecis(P1.Result, P4.Result, newSize), P5.Result, newSize), P7.Result, newSize);
                int[,] s = AddtionMatrecis(P3.Result, P5.Result, newSize);
                int[,] t = AddtionMatrecis(P2.Result, P4.Result, newSize);
                int[,] u = AddtionMatrecis(SubtractionMatrecis(AddtionMatrecis(P1.Result, P3.Result, newSize), P2.Result, newSize), P6.Result, newSize);


                combineSolutions(r, Matrix, 0, 0, newSize);
                combineSolutions(s, Matrix, 0, newSize, newSize);
                combineSolutions(t, Matrix, newSize, 0, newSize);
                combineSolutions(u, Matrix, newSize, newSize, newSize);
                /*
                 Task c1 = new Task(() => combineSolutions(r, Matrix, 0, 0, newSize));
                 Task c2 = new Task(() => combineSolutions(s, Matrix, 0, newSize, newSize));
                 Task c3 = new Task(() => combineSolutions(t, Matrix, newSize, 0, newSize));
                 Task c4 = new Task(() => combineSolutions(u, Matrix, newSize, newSize, newSize));

                 c1.Start(); c2.Start(); c3.Start(); c4.Start();

                 c1.Wait(); c2.Wait(); c3.Wait(); c4.Wait();
                */
            }

            return Matrix;
        }
        /*
        // Function to split Main matrix into sub matrices (get the Quatrter for each sub matrix)
        static void devide(int[,] X, int[,] Y, int first, int Last, int size)
        {
            for (int i = 0, j = first; i < size; i++, j++)
            {
                for (int k = 0, L = Last; k < size; k++, L++)
                {
                    Y[i, k] = X[j, L];
                }
            }
        }
        */

        public static int[,] NaiveSolutionForMatrecisLess2(int[,] FirstMatrix, int[,] SecondMatrix, int size)
        {

            int[,] maltiplicationMatrix = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    long Result = 0;
                    for (int k = 0; k < size; k++)
                    {
                        Result += FirstMatrix[i, k] * SecondMatrix[k, j];
                    }
                    maltiplicationMatrix[i, j] = (int)Result;
                }
            }

            return maltiplicationMatrix;

        }


        public static int[,] NaiveSolutionForMatrecis(int[,] FirstMatrix, int[,] SecondMatrix, int size)
        {
            int[,] multiplicationMatrix = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int Aya = 0, Ayaa = 0, Ayaaa = 0, Ayaaaa = 0;
                    for (int k = 0; k < size; k += 4) // Loop unrolling by 4  ==> T(N) = N * N * N / 4
                    {
                        Aya += FirstMatrix[i, k] * SecondMatrix[k, j];
                        Ayaa += FirstMatrix[i, k + 1] * SecondMatrix[k + 1, j];
                        Ayaaa += FirstMatrix[i, k + 2] * SecondMatrix[k + 2, j];
                        Ayaaaa += FirstMatrix[i, k + 3] * SecondMatrix[k + 3, j];
                    }
                    multiplicationMatrix[i, j] = Aya + Ayaa + Ayaaa + Ayaaaa;
                }
            }

            return multiplicationMatrix;
        }

        static int[,] SubtractionMatrecis(int[,] FirstMtrix, int[,] SecondMatrix, int size)
        {
            int[,] SubtractionResult = new int[size, size]; // SecondMatrix.GetLength(0)= N/2

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    SubtractionResult[i, j] = (FirstMtrix[i, j] - SecondMatrix[i, j]);
                }
            }

            return SubtractionResult;
        }

        static int[,] AddtionMatrecis(int[,] FirstMtrix, int[,] SecondMtrix, int size)
        {
            //long size = SecondMtrix.GetLength(0);
            int[,] AddtionResult = new int[size, size]; // SecondMtrix.GetLength(0) = N/2

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    AddtionResult[i, j] = FirstMtrix[i, j] + SecondMtrix[i, j];
                }
            }

            return AddtionResult;
        }


        static void combineSolutions(int[,] X, int[,] Y, int First, int Last, int size)
        {
            for (int i = 0, j = First; i < size; i++, j++) // size = N/2
            {
                for (int k = 0, L = Last; k < size; k++, L++)
                {
                    Y[j, L] = X[i, k];
                }
            }
        }
        #endregion

    }
}

