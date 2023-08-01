using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> result = new List<float>();
            int N = InputSignal.Samples.Count;

            //intialate file
            StreamWriter File = new StreamWriter("D://dct.txt");
            File.WriteLine("coefficients");


            for (int k = 0; k < N; k++)
            {
                Double sum = 0;
                for (int n = 0; n < N; n++)
                {
                    //sum += InputSignal.Samples[n] * (Double)Math.Cos(((Math.PI * k * ((2.0 * n) + 1)) / (2.0 * N)));

                    sum += InputSignal.Samples[n] * (float)Math.Cos((Math.PI /(4*N))* (((2 * n) - 1)*((2* k) - 1)));
                    //sum += InputSignal.Samples[n] * (float)Math.Cos((Math.PI / (4 * N)) * ((2 * n) - 1) * ((2 * k) - 1));
                }
   
                sum = sum * Math.Sqrt(2.0 / N);
                result.Add((float)(sum ));
            }
            int m = result.Count;
            for (int i = 1; i <= m; i++)
            {
                File.WriteLine(result[i - 1]);

            }
            File.Close();
            OutputSignal = new Signal(result, false);
            //throw new NotImplementedException();
        }
    }
}