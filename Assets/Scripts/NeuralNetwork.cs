using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NeuralNetwork : ScriptableObject {
	public double LearnRate { get; set; }
	public double Momentum { get; set; }
	public List<Neuron> InputLayer;
	public List<List<Neuron>> HiddenLayer;
	public List<Neuron> OutputLayer;

	public int inputSize;
	public int[] hiddenSize;
	public int outputSize;

	static System.Random random = new System.Random();
	public GameObject NeuronPrefab,SynapsePrefab;
	void Start(){
		LearnRate = .9;
		Momentum = .04;
		InputLayer = new List<Neuron>();
		HiddenLayer = new List<List<Neuron>>();
		OutputLayer = new List<Neuron>();

		for (int i = 0; i < inputSize; i++){
			InputLayer.Add(CreateNeuron(i.ToString()));
		}

		HiddenLayer.Add(new List<Neuron>());
		for (int i = 0; i < hiddenSize[0]; i++){
			HiddenLayer[0].Add(CreateNeuronWithInputs(InputLayer,"Hidden " + (i + inputSize).ToString()));
		}

		for (int i = 1; i < hiddenSize.Length; i++){
			HiddenLayer.Add(new List<Neuron>());
			for(int j = 0; j < hiddenSize[i]; j++){
				HiddenLayer[i].Add(CreateNeuronWithInputs(HiddenLayer[i-1],"Hidden " +  (i + inputSize).ToString()));	
			}
		}

		for (int i = 0; i < outputSize; i++){
			OutputLayer.Add(CreateNeuronWithInputs(HiddenLayer[HiddenLayer.Count-1],"output " + i));
		}
		//RepositionInputNeurons();
		//RepositionHiddenNeurons();
		//RepositionOutputNeurons();
	}
	//void RepositionInputNeurons(){
	//	for(int i = 0;i<InputLayer.Count;i++){
	//		InputLayer[i].transform.position = new Vector3(0f,-(InputLayer.Count * 1.5f * 0.5f) + (i * 1.5f) ,0f);
	//	}
	//}
	//void RepositionHiddenNeurons(){
	//	for(int i = 0;i<HiddenLayer.Count;i++){
	//		for(int j = 0; j< HiddenLayer[i].Count;j++){
	//			HiddenLayer[i][j].transform.position = new Vector3(1.5f + (i * 1.5f),-(HiddenLayer[i].Count * 1.5f * 0.5f) + (j * 1.5f) ,0f);	
	//		}

	//	}
	//}
	//void RepositionOutputNeurons(){
	//	for(int i = 0;i<OutputLayer.Count;i++){
	//		OutputLayer[i].transform.position = new Vector3(1.5f + (HiddenLayer.Count * 1.5f) + (i * 1.5f) ,-(OutputLayer.Count * 1.5f * 0.5f) + (i * 1.5f) ,0f);
	//	}
	//}
	Neuron CreateNeuron(string name){
		GameObject g = (GameObject)Instantiate(NeuronPrefab);
		//g.transform.SetParent(transform);
		g.name = name;
		return g.GetComponent<Neuron>();
	}
	Neuron CreateNeuronWithInputs(List<Neuron> inputs,string name){
		Neuron n = CreateNeuron(name);
		n.Create();
		foreach (Neuron inputNeuron in inputs)
		{
			Synapse synapse = CreateSynapse(inputNeuron, n);
			inputNeuron.OutputSynapses.Add(synapse);
			n.InputSynapses.Add(synapse);
		}
		return n;
	}
	Synapse CreateSynapse(Neuron input,Neuron output){
		GameObject g = (GameObject)Instantiate(SynapsePrefab);
		//g.transform.SetParent(transform);
		Synapse s = g.GetComponent<Synapse>();
		s.Create(input,output);
		return s;
	}

	public void Train(params double[] inputs)
	{
		int i = 0;
		InputLayer.ForEach(a => a.Value = inputs[i++]);

		//HiddenLayer.ForEach(a => a.CalculateValue());
		foreach(List<Neuron> l in HiddenLayer){
			foreach(Neuron n in l){
				n.CalculateValue();
			}	
		}

		OutputLayer.ForEach(a => a.CalculateValue());
	}

	public double[] Compute(params double[] inputs)
	{
		Train(inputs);
		return OutputLayer.Select(a => a.Value).ToArray();
	}

	public double CalculateError(params double[] targets)
	{
		int i = 0;
		return OutputLayer.Sum(a => Math.Abs(a.CalculateError(targets[i++])));
	}

	public void BackPropagate(params double[] targets)
	{
		int i = 0;
		OutputLayer.ForEach(a => a.CalculateGradient(targets[i++]));
		//HiddenLayer.ForEach(a => a.CalculateGradient());
		foreach(List<Neuron> l in HiddenLayer){
			foreach(Neuron n in l){
				n.CalculateGradient();
			}	
		}
		//HiddenLayer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
		foreach(List<Neuron> l in HiddenLayer){
			foreach(Neuron n in l){
				n.UpdateWeights(LearnRate, Momentum);
			}	
		}
		OutputLayer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
	}

	public static double NextRandom()
	{
		
		return 2 * random.NextDouble() - 1;
	}

	public static double SigmoidFunction(double x)
	{
		if (x < -45.0) return 0.0;
		else if (x > 45.0) return 1.0;
		return 1.0 / (1.0 + Math.Exp(-x));
	}

	public static double SigmoidDerivative(double f)
	{
		return f * (1 - f);
	}
}
