using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Neuron : ScriptableObject {
	
	public List<Synapse> InputSynapses; 
	public List<Synapse> OutputSynapses;
	public double Bias { get; set; }
	public double BiasDelta { get; set; }
	public double Gradient { get; set; }
	public double Value { get; set; }

	void Start(){
		Bias = NeuralNetwork.NextRandom();
	}
	public void Create(){
		InputSynapses = new List<Synapse>();
		OutputSynapses = new List<Synapse>();
	}
	public virtual double CalculateValue()
	{
		//return Value = NeuralNetwork.SigmoidFunction(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value) + Bias);
		return Value = NeuralNetwork.SigmoidFunction(Sum(InputSynapses) + Bias);
	}

	public double Sum(List<Synapse> list){
		double temp = 0;
		foreach(Synapse s in list){
			temp += s.Weight * s.InputNeuron.Value; 
		}
		return temp;
	}

	public virtual double CalculateDerivative()
	{
		return NeuralNetwork.SigmoidDerivative(Value);
	}

	public double CalculateError(double target)
	{
		return target - Value;
	}

	public double CalculateGradient(double target)
	{
		return Gradient = CalculateError(target) * CalculateDerivative();
	}

	public double CalculateGradient()
	{
		//return Gradient = OutputSynapses.Sum(a => a.OutputNeuron.Gradient * a.Weight) * CalculateDerivative();
		return Gradient = Sum(OutputSynapses) * CalculateDerivative();
	}

	public void UpdateWeights(double learnRate, double momentum)
	{
		var prevDelta = BiasDelta;
		BiasDelta = learnRate * Gradient; // * 1
		Bias += BiasDelta + momentum * prevDelta;

		foreach (var s in InputSynapses)
		{
			prevDelta = s.WeightDelta;
			s.WeightDelta = learnRate * Gradient * s.InputNeuron.Value;
			s.Weight += s.WeightDelta + momentum * prevDelta;
		}
	}
}
