using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSforTSP
{
    public static class IteratedLocalSearch
    {
        static Random rand = new Random();
        static double euc_2d(double[] c1, double[] c2)
        {
            double x = c1[0] - c2[0];
            double y = c1[1] - c2[1];
            return Math.Ceiling(Math.Sqrt(x * x + y * y));
        }
        static double cost(double[] permutation, double[][] cities)
        {
            double distance = 0;
            for (int i = 1; i < permutation.Length; i++)
            {
                distance += euc_2d(cities[Convert.ToInt32(permutation[i - 1])], cities[Convert.ToInt32(permutation[i])]);
            }
            return distance += euc_2d(cities[Convert.ToInt32(permutation[permutation.Length - 1])], cities[Convert.ToInt32(permutation[0])]);
        }
        static double[] random_permutation(double[][] cities)
        {
            double[] perm = new double[cities.Length];
            for (int i = 0; i < perm.Length; i++)
                perm[i] = i;
            for (int i = 0; i < perm.Length; i++)
            {
                int r = rand.Next(perm.Length - i) + i;
                double temp = perm[r];
                perm[r] = perm[i];
                perm[i] = temp;
            }
            return perm;
        }
        public static double[][] local_search(double[][] best, double[][] cities, int max_no_improv)
        {
            int count = 0;
            double[][] candidate = new double[2][];
            candidate[1] = new double[1];
            double tmp;
            int firstSwapItem = 0, secondSwapItem = 0;
            double currentFitness, bestFitness = best[1][0];
            while (count < max_no_improv)
            {
                for (int j = 1; j < cities.Length; j++)
                {
                    for (int i = 0; i < j; i++)
                    {
                        tmp = best[0][j];
                        best[0][j] = best[0][i];
                        best[0][i] = tmp;
                        currentFitness = cost(best[0], cities);
                        if (currentFitness < bestFitness)
                        {
                            firstSwapItem = j;
                            secondSwapItem = i;
                            bestFitness = currentFitness;
                            count = 0;
                        }
                        tmp = best[0][j];
                        best[0][j] = best[0][i];
                        best[0][i] = tmp;
                    }
                }
                if (firstSwapItem != secondSwapItem)
                {
                    tmp = best[0][firstSwapItem];
                    best[0][firstSwapItem] = best[0][secondSwapItem];
                    best[0][secondSwapItem] = tmp;
                    best[1][0] = cost(best[0], cities);
                }
                count++;
            }
            return best;
        }
        static double[] double_bridge_move(double[] perm)
        {
            int pos1 = 1 + rand.Next(perm.Length / 4);
            int pos2 = pos1 + 1 + rand.Next(perm.Length / 4);
            int pos3 = pos2 + 1 + rand.Next(perm.Length / 4);
            double[] p1 = new double[perm.Length - pos3 + pos1];
            double[] p2 = new double[pos3 - pos2 + (pos2 - pos1)];
            Array.Copy(perm, p1, pos1);
            Array.Copy(perm, pos3, p1, pos1, perm.Length - pos3);
            Array.Copy(perm, pos2, p2, 0, pos3 - pos2);
            Array.Copy(perm, pos1, p2, pos3 - pos2, pos2 - pos1);
            double[] p = new double[perm.Length];
            p1.CopyTo(p, 0);
            p2.CopyTo(p, p1.Length);
            return p;
        }

        public static double[][] perturbation(double[][] cities, double[][] best)
        {
            double[][] candidate = new double[2][];
            candidate[1] = new double[1];
            candidate[0] = double_bridge_move(best[0]);
            candidate[1][0] = cost(candidate[0], cities);
            return candidate;
        }

        public static double[][] search(double[][] cities, int max_iterations, int max_no_improv)
        {
            double[][] best = new double[2][];
            best[0] = random_permutation(cities);
            best[1] = new double[1];
            best[1][0] = cost(best[0], cities);
            best = local_search(best, cities, max_no_improv);
            return best;
        }
        public static double[][] search(double[][] cities, int max_iterations, int max_no_improv,double[][] current)
        {
            double[][] best = new double[2][];
            best = local_search(current, cities, max_no_improv);
            return best;
        }
    }
}
