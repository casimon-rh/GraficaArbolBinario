using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    class Program
    {
        static void Main(string[] args)
        {
            ABB arbol = null;
            List<int> lista = new List<int>() { 2, 1, 4, 3, 5, 7, 6 };
            foreach (int i in lista)
                arbol = UsoABB.Inserta(arbol, i);
            Console.WriteLine("Inorden:");
            foreach (int i in UsoABB.RecorreInorden(arbol))
                Console.Write(i + "-");
            Console.WriteLine("\nPreorden:");
            foreach (int i in UsoABB.RecorrePreorden(arbol))
                Console.Write(i + "-");
            Console.WriteLine("\nPostorden:");
            foreach (int i in UsoABB.RecorrePostorden(arbol))
                Console.Write(i + "-");
            Console.ReadKey();
        }
    }
}
