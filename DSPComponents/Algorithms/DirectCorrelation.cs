using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        public List<float> InputSignal2_Samples = new List<float>();
        public List<float> Non_Normalized = new List<float>();
        public List<float> Normalized = new List<float>();
        public override void Run()
        {
            float result, final_result, normalized_result, normalized_value;
            float temp;
            float sum = 0;
            float sum2 = 0;
            float total;
            for (int i = 0; i < InputSignal1.Samples.Count(); i++) //Squared sum of signal 1.
            {
                sum += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
            }
            if (InputSignal2 == null) //Auto-Correlation.
            {
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    InputSignal2_Samples.Add(InputSignal1.Samples[i]);
                }
                normalized_value = (float)(Math.Sqrt((sum * sum))) / InputSignal1.Samples.Count();
            }
            else //Cross-Correlation.
            {
                for (int i = 0; i < InputSignal2.Samples.Count(); i++)
                {
                    InputSignal2_Samples.Add(InputSignal2.Samples[i]);
                }
                for (int i = 0; i < InputSignal2.Samples.Count(); i++) //Squared sum of signal 2.
                {
                    sum2 += (InputSignal2.Samples[i] * InputSignal2.Samples[i]);
                }
                total = sum * sum2;
                normalized_value = (float)(Math.Sqrt(total)) / InputSignal1.Samples.Count();
            }
            for (int j = 0; j < InputSignal1.Samples.Count(); j++)
            {
                result = 0;
                for (int n = 0; n < InputSignal1.Samples.Count(); n++)
                {
                    result += (InputSignal1.Samples[n] * InputSignal2_Samples[n]);
                }
                final_result = result / (InputSignal1.Samples.Count());
                Console.WriteLine(final_result);
                Non_Normalized.Add(final_result);
                normalized_result = final_result / normalized_value;
                Normalized.Add(normalized_result);
                if (InputSignal1.Periodic == true)
                {
                    temp = InputSignal2_Samples[0];
                    for (int i = 0; i < InputSignal1.Samples.Count() - 1; i++)
                    {
                        InputSignal2_Samples[i] = InputSignal2_Samples[i + 1];
                    }
                    InputSignal2_Samples[InputSignal1.Samples.Count() - 1] = temp;
                }
                else if (InputSignal1.Periodic == false)
                {
                    for (int k = 0; k < InputSignal2_Samples.Count - 1; k++)
                        InputSignal2_Samples[k] = InputSignal2_Samples[k + 1];
                    InputSignal2_Samples[InputSignal1.Samples.Count - 1] = 0;
                }
            }
            OutputNonNormalizedCorrelation = Non_Normalized;
            OutputNormalizedCorrelation = Normalized;
        }
    }
}