using System.Collections;
using System.Collections.ObjectModel;

namespace Composite.NeuralNetworks {

    // Bad design
    public class BadNeuron {
        public float Value;
        public List<BadNeuron> In, Out;

        public void ConnectTo(BadNeuron other) {
            Out.Add(other);
            other.In.Add(this);
        }
    }

    public class BadNeuronLayer: Collection<BadNeuron> {

    }

    // Good design
    public class Neuron: IEnumerable<Neuron> {
        public float Value;
        public List<Neuron> In, Out;

        public void ConnectTo(Neuron other) {
            Out.Add(other);
            other.In.Add(this);
        }

        public IEnumerator<Neuron> GetEnumerator() {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NeuronLayer: Collection<Neuron> {

    }

    public static class ExtensionsMethods {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other) {
            if (ReferenceEquals(self, other)) return;

            foreach (var from in self)
                foreach (var to in other)
                    from.ConnectTo(to);
        }
    }

    public class Demo {
        public static void Run() {
            // Example of bad design
            var neuron1 = new BadNeuron();
            var neuron2 = new BadNeuron();

            neuron1.ConnectTo(neuron2);

            var later1 = new BadNeuronLayer();
            var later2 = new BadNeuronLayer();

            // neuron1.ConnectTo(later1); // not possible, we need 4 ConnectTo methods. 
            // Collection->single, single->Collection, Collection->Collection, single->single
            // And now what if we have NeuronRing? which is list? 


            // Example of good design
            var _neuron1 = new Neuron();
            var _neuron2 = new Neuron();

            _neuron1.ConnectTo(_neuron2);

            var _later1 = new NeuronLayer();
            var _later2 = new NeuronLayer();

            _later1.ConnectTo(_later2);
        }
    }
}