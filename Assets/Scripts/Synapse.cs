using UnityEngine;
using System.Collections;

public class Synapse : ScriptableObject {

	public Neuron InputNeuron; 
	public Neuron OutputNeuron;
	public double Weight { get; set; }
	public double WeightDelta { get; set; }

	void Start(){
		
		Weight = NeuralNetwork.NextRandom();
	}
	public void Create(Neuron input, Neuron output){
		InputNeuron = input;
		OutputNeuron = output;

	}
}
