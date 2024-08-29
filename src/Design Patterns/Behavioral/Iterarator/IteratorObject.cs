using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace CSharpLearnings.src.Design_Patterns.Behavioral.Iterarator.IteratorObject
{
    public class Node<T>
    {
        public T value;
        public Node<T> left, right;
        public Node<T> parent;

        public Node(T value)
        {
            value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            value = value;
            left = left;
            right = right;

            left.parent = right.parent = this;
        }
    }

    public class InOrderIterator<T> {
        private readonly Node<T> root;
        public Node<T> current;
        private bool yieldedStart;
        public InOrderIterator(Node<T> root)
        {
            this.root = root;
            current = root;
            while (current.left != null)
            {
                current = current.left;
            }
            //    1 < - root
            //   / \
            //  2   3
            //  ^ current
        }

        public bool MoveNext() {
            if (!yieldedStart) {
                yieldedStart = true;
                return true;
            }

            if (current.right != null) {
                current = current.right;
                while (current.left != null) {
                    current = current.left;
                }
                return true;
            }
            else {
                var p = current.parent;
                while (p != null && current == p.right) {
                    current = p;
                    p = p.parent;
                }
                current = p;
                return current != null;
            }
        }

        public void Reset() {

        }
    }

    public class BinaryTree<T>
    {
        
    }

    public class IteratorObject
    {
        public static void Run()
        {
            var root = new Node<int>(1,
                new Node<int>(2),
                new Node<int>(3));

                var it = new InOrderIterator<int>(root);
                while (it.MoveNext()) {
                    Console.Write(it.current.value);
                    Console.Write(",");
                }
                Console.WriteLine();
        }
    }
}
