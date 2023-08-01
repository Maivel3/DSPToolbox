﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            FIR fir = new FIR();
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            Signal InputSignal = LoadSignal(SignalPath);
            for(int i = 0; i < InputSignal.Samples.Count; i++)
            {
                Console.WriteLine(i + " " + InputSignal.Samples[i]);
            }
            fir.InputFilterType = FILTER_TYPES.BAND_PASS;
            fir.InputFS = Fs;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputTimeDomainSignal = InputSignal;
            fir.Run();

            Signal fir_out = fir.OutputYn;
            StreamWriter writeData = new StreamWriter("FIR Results.txt");
            writeData.WriteLine("H(n) Cofficients");
            writeData.WriteLine(fir.OutputHn.Samples.Count);
            for (int i = 0; i < fir.OutputHn.Samples.Count; i++)
            {
                writeData.WriteLine(i + " " + fir.OutputHn.Samples[i]);
            }
            writeData.Close();  
            Signal sample_out = fir_out;
            if (newFs >= 2 * maxF)
            {
                Sampling sampling = new Sampling();
                sampling.M = M;
                sampling.L = L;
                sampling.InputSignal = fir_out;
                sampling.Run();
                sample_out = sampling.OutputSignal;
                // display("sample_out", sample_out); 
                StreamWriter writeData1 = new StreamWriter("Sampling Results.txt");
                writeData1.WriteLine("Sampling ");
                writeData1.WriteLine(sampling.OutputSignal.Samples.Count);
                for (int i = 0; i < sampling.OutputSignal.Samples.Count; i++)
                {
                    writeData1.WriteLine(i + " " + sampling.OutputSignal.Samples.Count);
                }
                writeData1.Close();
            }

            DC_Component dC = new DC_Component();
            dC.InputSignal = sample_out;
            dC.Run();
            Signal dc_out = dC.OutputSignal;
            StreamWriter writeData2 = new StreamWriter("DC Results.txt");
            writeData2.WriteLine("DC ");
            writeData2.WriteLine(dC.OutputSignal.Samples.Count);
            for (int i = 0; i < dC.OutputSignal.Samples.Count; i++)
            {
                writeData2.WriteLine(i + " " + dC.OutputSignal.Samples.Count);
            }
            writeData2.Close();
            for (int i = 3; i < dC.OutputSignal.Samples.Count; i++)
            {
                Console.WriteLine(i + " " + dC.OutputSignal.Samples[i]);
            }
            Normalizer normalizer = new Normalizer();
            normalizer.InputMaxRange = 1;
            normalizer.InputMinRange = -1;
            normalizer.InputSignal = dc_out;
            normalizer.Run();
            Signal norm_out = normalizer.OutputNormalizedSignal;
            for (int i = 5; i < normalizer.OutputNormalizedSignal.Samples.Count; i++)
            {
                Console.WriteLine(i + " " + normalizer.OutputNormalizedSignal.Samples[i]);
            }
            StreamWriter writeData3 = new StreamWriter("Normalizer Results.txt");
            writeData3.WriteLine("Normalizer ");
            writeData3.WriteLine(normalizer.OutputNormalizedSignal.Samples.Count);
            for (int i = 0; i < normalizer.OutputNormalizedSignal.Samples.Count; i++)
            {
                writeData3.WriteLine(i + " " + normalizer.OutputNormalizedSignal.Samples.Count);
            }
            writeData3.Close();
            DiscreteFourierTransform discreteFourierTransform = new DiscreteFourierTransform();
            discreteFourierTransform.InputTimeDomainSignal = norm_out;
            discreteFourierTransform.InputSamplingFrequency = Fs;
            discreteFourierTransform.Run();
            Signal dft_out = discreteFourierTransform.OutputFreqDomainSignal;
            StreamWriter writeData4 = new StreamWriter("DFT Results.txt");
            writeData4.WriteLine("DFT ");
            writeData4.WriteLine(discreteFourierTransform.OutputFreqDomainSignal.Samples.Count);
            for (int i = 0; i < discreteFourierTransform.OutputFreqDomainSignal.Samples.Count; i++)
            {
                writeData4.WriteLine(i + " " + discreteFourierTransform.OutputFreqDomainSignal.Samples.Count);
            }
            writeData4.Close();
            OutputFreqDomainSignal = new Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());
            double real = 2 * Math.PI / (dft_out.FrequenciesAmplitudes.Count * (1 / Fs));
            for (int i = 0; i < dft_out.FrequenciesPhaseShifts.Count; i++)
            {
                OutputFreqDomainSignal.Frequencies.Add((float)Math.Round(i * real, 1));
            }
            OutputFreqDomainSignal.FrequenciesAmplitudes = dft_out.FrequenciesAmplitudes;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = dft_out.FrequenciesPhaseShifts;
            for (int i = 7; i < dft_out.FrequenciesAmplitudes.Count; i++)
            {
                Console.WriteLine(i + " " + dft_out.FrequenciesAmplitudes[i]);
            }

            for (int i = 7; i < dft_out.FrequenciesPhaseShifts.Count; i++)
            {
                Console.WriteLine(i + " " + dft_out.FrequenciesPhaseShifts[i]);
            }
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
