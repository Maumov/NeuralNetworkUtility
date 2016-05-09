using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brain : ScriptableObject
{

    public List<Neuron> neurons = new List<Neuron>();
    public void AddNeuron() {
        neurons.Add(ScriptableObject.CreateInstance<Neuron>());
    }
}
