﻿using System;
using System.Linq;
using System.Text;

namespace dataAnalyze.Algorithms
{
    public class Shingles
    {
        public static bool Checkinputfile = true;
        private char[] _stopSymbols;
        public int ShingleLength;

        public Shingles(string delims, int shinglelength)
        {
//             if (!File.Exists(delims))
//                 throw new Exception("File with stop symbols not exists: " + delims);
            _stopSymbols = delims.ToCharArray();
            ShingleLength = shinglelength;

        }
        public void Length(int x)
        {
            ShingleLength = x;
        }
        public int Length()
        {
            return ShingleLength;
        }

        public double CompareStrings(string s1, string s2)
        {
            if (s1.Length <= ShingleLength) return 0.0;
            if (s2.Length <= ShingleLength) return 0.0;
            if (s1.Length > s2.Length)
            {
                if (Math.Abs((s1.Length / (double)s2.Length)) >= 1.7)
                    return 0.0;
            }
            else
            {
                if (Math.Abs((s2.Length / (double)s2.Length)) >= 1.7)
                    return 0.0;
            }


            RemoveStopSymbols(ref s1);
            RemoveStopSymbols(ref s2);

            if (s1.Length <= ShingleLength) return 0.0;
            if (s2.Length <= ShingleLength) return 0.0;
            var shingles1 = GetShingles(ref s1, ShingleLength);
            var shingles2 = GetShingles(ref s2, ShingleLength);
            if (Checkinputfile)
            {
                var same = shingles1.Count(shingles2.Contains);
                return same/((double) (shingles1.Length))*100;
            }
            else
            {
                var same = shingles2.Count(shingles1.Contains);
                return same / ((double)(shingles2.Length)) * 100;
            }
        }





        public double CompareStringsCashed(ref string[] shingles1, ref string[] shingles2, string s1, string s2)
        {
            if (s1 != null && s2 != null)
            {
                if (s1.Length > s2.Length)
                {
                    if (Math.Abs((s1.Length / (double)s2.Length)) >= 1.7)
                        return 0.0;
                }
                else
                {
                    if (Math.Abs((s2.Length / (double)s1.Length)) >= 1.7)
                        return 0.0;
                }
            }

            if (s1 != null)
            {
                if (s1.Length <= ShingleLength) return 0.0;
                string inS1 = s1;
                RemoveStopSymbols(ref inS1);
                if (inS1.Length <= ShingleLength) return 0.0;
                shingles1 = GetShingles(ref inS1, ShingleLength);
            }

            if (s2 != null)
            {
                if (s2.Length <= ShingleLength) return 0.0;
                var inS2 = s2;
                RemoveStopSymbols(ref inS2);
                if (inS2.Length <= ShingleLength) return 0.0;
                shingles2 = GetShingles(ref inS2, ShingleLength);
            }                        
           
            var same = 0;
            foreach (var t in shingles1)
            {
                if (shingles2.Contains(t))
                    same++;
            }

            return same * 2 / ((double)(/*shingles1.Length + */shingles2.Length)) * 100;
          
        }


        /// <summary>
        /// get shingles and calculate hash for everyone
        /// </summary>
        /// <param name="source"></param>
        /// <param name="shingleLength"></param>
        /// <returns></returns>
        private string[] GetShingles(ref string source, int shingleLength)
        {
            var shingles = new string[source.Length - (shingleLength - 1)];
            var shift = 0;
            for (var i = 0; i < shingles.Length; i++)
            {

                shingles[i] += 
                    (source.Length >= shift + shingleLength)
                        ? source.Substring(shift, shingleLength).GetHashCode()
                        : source.Substring(shift, source.Length - (shift + shingleLength)).GetHashCode();
                shift++;
            }
            return shingles;

        }

        /// <summary>
        /// delete some inappropriate chars from the string
        /// </summary>
        /// <param name="source"></param>
        private void RemoveStopSymbols(ref string source)
        {
            var positionForRemove = new int[source.Length];
            var arrayCounter = 0;
            FindIndexOfSymbols(ref source, ref positionForRemove, ref arrayCounter, ref _stopSymbols);
            Array.Resize(ref positionForRemove, arrayCounter);
            Array.Sort(positionForRemove);
            //Array.Reverse(positionForRemove);
            var shift = 0;
            var result = new StringBuilder(source.Length - arrayCounter);
            for (var i = 0; i < source.Length; i++)
            {
                if (i == positionForRemove[shift])
                {
                    if (positionForRemove.Length > shift + 1)
                        shift++;
                }
                else
                    result.Append(source[i]);
            }

            //positionForRemove = null;
            source = result.ToString();

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"> link for original string</param>
        /// <param name="positionsForRemove">array of indexes</param>
        /// <param name="arrayCounter">point to next element in array</param>
        /// <param name="symbols"></param>
        private void FindIndexOfSymbols(ref string source, ref int[] positionsForRemove, ref int arrayCounter, ref char[] symbols)
        {
            for (int i = 0; i < source.Length; i++)
            {
                foreach (var t in symbols)
                    if (source[i] == t)
                    {
                        positionsForRemove[arrayCounter] = i;
                        arrayCounter++;

                    }
            }
        }
    }
}
 



