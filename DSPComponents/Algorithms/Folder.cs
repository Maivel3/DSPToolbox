using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {

            List<float> values = new List<float>();
            
            List<int> indexes = new List<int>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                int x = (InputSignal.Samples.Count) - i - 1;
                values.Add( InputSignal.Samples[x]);
                indexes.Add( -1 * InputSignal.SamplesIndices[InputSignal.Samples.Count - i-1]);
            }
            OutputFoldedSignal = new Signal(values, indexes, false);
        }
    }
}
