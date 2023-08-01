using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
            samples = new List<float>();
            double output=0;
           // throw new NotImplementedException();
           for(int i = 0; i < SamplingFrequency; i++)
            {
                if (type.ToLower().Equals("sin"))
                {
                    output = Math.Sin((2 * Math.PI) * ((samples.Count * AnalogFrequency) / SamplingFrequency) + PhaseShift);

                }
                else if (type.ToLower().Equals("cos"))
                {
                    output = Math.Cos((2 * Math.PI) * ((samples.Count * AnalogFrequency) / SamplingFrequency) + PhaseShift);
                }
                samples.Add((float)(output * A));
            }
        }
    }
}
