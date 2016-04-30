using UnityEngine;
using System.Collections;


public class Test : MonoBehaviour {
	NeuralNetwork2.NeuralNetwork network;
	// Use this for initialization
	void Start () {
		network = new NeuralNetwork2.NeuralNetwork(2,3,1);
		for (int i = 0; i < 100000; i++)
		{
			network.Train(0, 0);
			network.BackPropagate(1);

			network.Train(1, 0);
			network.BackPropagate(0);

			network.Train(0, 1);
			network.BackPropagate(0);

			network.Train(1, 1);
			network.BackPropagate(1);
		}
		double error;
		double output;

		output = network.Compute(0, 0)[0];
		error = network.CalculateError(1);
		Debug.Log("0 XOR 0 = " + output.ToString("F5") + ", Error = " + error.ToString());

		output = network.Compute(1, 0)[0];
		error = network.CalculateError(0);
		Debug.Log("1 XOR 0 = " + output.ToString("F5") + ", Error = " + error.ToString());

		output = network.Compute(0, 1)[0];
		error = network.CalculateError(0);
		Debug.Log("0 XOR 1 = " + output.ToString("F5") + ", Error = " + error.ToString());

		output = network.Compute(1, 1)[0];
		error = network.CalculateError(1);
		Debug.Log("1 XOR 1 = " + output.ToString("F5") + ", Error = " + error.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
