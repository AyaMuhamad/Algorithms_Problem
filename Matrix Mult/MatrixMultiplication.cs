using System;
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
    public static class MatrixMultiplication
    {
        #region YOUR CODE IS HERE
        
        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 square matrices in an efficient way [Strassen's Method]
        /// </summary>
        /// <param name="M1">First square matrix</param>
        /// <param name="M2">Second square matrix</param>
        /// <param name="N">Dimension (power of 2)</param>
        /// <returns>Resulting square matrix</returns>
        static public int[,] MatrixMultiply(int[,] M1, int[,] M2, int N)
        {
            //REMOVE THIS LINE BEFORE START CODING
            //throw new NotImplementedException();

            // Create the result matrix
            if (N <= 0) throw new ArgumentException("Invalid matrix size", nameof(N));
            if (M1.GetLength(1) != M2.GetLength(0)) throw new ArgumentException("Incompatible matrix dimensions");
            // Create the result matrix

            if (M1.GetLength(1) != M2.GetLength(0)) throw new ArgumentException("Incompatible matrix dimensions");

            
            
            int[,] maltiplicationMatrix = new int[N, N];

            if (N <= 64 || N % 2 != 0)
            {
                return NaiveMatrixMultiply(M1, M2);
            }
            else
            {
                // Create the sub-matrices
                int[,] A11 = new int[N / 2, N / 2];
                int[,] A12 = new int[N / 2, N / 2];
                int[,] A21 = new int[N / 2, N / 2];
                int[,] A22 = new int[N / 2, N / 2];
                int[,] B11 = new int[N / 2, N / 2];
                int[,] B12 = new int[N / 2, N / 2];
                int[,] B21 = new int[N / 2, N / 2];
                int[,] B22 = new int[N / 2, N / 2];

                // Divide the matrices into 4 sub-matrices
                Split(M1, A11, 0, 0);
                Split(M1, A12, 0, N / 2);
                Split(M1, A21, N / 2, 0);
                Split(M1, A22, N / 2, N / 2);

                Split(M2, B11, 0, 0);
                Split(M2, B12, 0, N / 2);
                Split(M2, B21, N / 2, 0);
                Split(M2, B22, N / 2, N / 2);

                // Calculate the 7 products
                Task<int[,]> P1 = new Task<int[,]>(() => MatrixMultiply(Add(A11, A22), Add(B11, B22), N / 2));
                Task<int[,]> P2 = new Task<int[,]>(() => MatrixMultiply(Add(A21, A22), B11, N / 2));
                Task<int[,]> P3 = new Task<int[,]>(() => MatrixMultiply(A11, Subtract(B12, B22), N / 2));
                Task<int[,]> P4 = new Task<int[,]>(() => MatrixMultiply(A22, Subtract(B21, B11), N / 2));
                Task<int[,]> P5 = new Task<int[,]>(() => MatrixMultiply(Add(A11, A12), B22, N / 2));
                Task<int[,]> P6 = new Task<int[,]>(() => MatrixMultiply(Subtract(A21, A11), Add(B11, B12), N / 2));
                Task<int[,]> P7 = new Task<int[,]>(() => MatrixMultiply(Subtract(A12, A22), Add(B21, B22), N / 2));

                P1.Start(); P2.Start(); P3.Start(); P4.Start(); P5.Start(); P6.Start(); P7.Start();
                
                P1.Wait();  P2.Wait();  P3.Wait();  P4.Wait();  P5.Wait();  P6.Wait();  P7.Wait();
                
                Task<int[,]> C11 = new Task<int[,]>(() => Add(Subtract(Add(P1.Result, P4.Result), P5.Result), P7.Result));
                Task<int[,]> C12 = new Task<int[,]>(() => Add(P3.Result, P5.Result));
                Task<int[,]> C21 = new Task<int[,]>(() => Add(P2.Result, P4.Result));
                Task<int[,]> C22 = new Task<int[,]>(() => Add(Subtract(Add(P1.Result, P3.Result), P2.Result), P6.Result));

                C11.Start(); C12.Start(); C21.Start(); C22.Start();
                
                C11.Wait();  C12.Wait();  C21.Wait();  C22.Wait();
                
                // Join the 4 sub-matrices into one result matrix
                Join(C11.Result, maltiplicationMatrix, 0, 0);
                Join(C12.Result, maltiplicationMatrix, 0, N / 2);
                Join(C21.Result, maltiplicationMatrix, N / 2, 0);
                Join(C22.Result, maltiplicationMatrix, N / 2, N / 2);
            }

            return maltiplicationMatrix;
        }
        
         // Function to split parent matrix into child matrices
        static void Split(int[,] P, int[,] C, int iB, int jB)
        {
            for (int i1 = 0, i2 = iB; i1 < C.GetLength(0); i1++, i2++)
            {
                for (int j1 = 0, j2 = jB; j1 < C.GetLength(1); j1++, j2++)
                {
                    C[i1, j1] = P[i2, j2];
                }
            }
        }

        // Function to join child matrices into parent matrix
        static void Join(int[,] C, int[,] P, int iB, int jB)
        {
            for (int i1 = 0, i2 = iB; i1 < C.GetLength(0); i1++, i2++)
            {
                for (int j1 = 0, j2 = jB; j1 < C.GetLength(1); j1++, j2++)
                {
                    P[i2, j2] = C[i1, j1];
                }
            }
        }

        // Function to add two matrices
        static int[,] Add(int[,] A, int[,] B)
        {
            int N = A.GetLength(0);
            int[,] result = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result[i, j] = A[i, j] + B[i, j];
                }
            }

            return result;
        }

        // Function to subtract two matrices
        static int[,] Subtract(int[,] A, int[,] B)
        {
            int N = A.GetLength(0);
            int[,] result = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result[i, j] = A[i, j] - B[i, j];
                }
            }

            return result;
        }
        public static int[,] NaiveMatrixMultiply(int[,] matrixA, int[,] matrixB)
        {
            int rowsA = matrixA.GetLength(0);
            int colsA = matrixA.GetLength(1);
            int rowsB = matrixB.GetLength(0);
            int colsB = matrixB.GetLength(1);

            int[,] maltiplicationMatrix = new int[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        sum += matrixA[i, k] * matrixB[k, j];
                    }
                    maltiplicationMatrix[i, j] = sum;
                }
            }

            return maltiplicationMatrix;
        }
        #endregion
    }
}
