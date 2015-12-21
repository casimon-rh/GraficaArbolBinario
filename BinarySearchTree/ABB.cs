using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    public class ABB
    {
        public int? valor;
        public ABB() { }
        public ABB(int v)
        {
            valor = v;
        }
        public ABB NodoIzq;
        public ABB NodoDer;

    }
    public static class UsoABB
    {
        public static ABB buscaNodo(int i, ABB current)
        {
            if (EsVacio(current)) return null;
            else if (current.valor == i) return current;
            else if (current.valor < i) return buscaNodo(i, current.NodoDer);
            else return (buscaNodo(i, current.NodoIzq));
        }
        public static bool EsVacio(ABB arbol)
        {
            return arbol == null;
        }
        public static ABB Inserta(ABB arbol, int i)
        {
            ABB actual = arbol;
            ABB padre = null;
            ABB aux = null;
            while (!EsVacio(actual))
            {
                padre = actual;
                if (actual.valor == i) return null;
                actual = actual.valor < i ? actual.NodoDer : actual.NodoIzq;
            }
            if (padre == null) arbol = new ABB(i);
            else
            {
                if (padre.valor < i)
                {
                    aux = padre.NodoDer;
                    padre.NodoDer = new ABB(i);
                    padre.NodoDer.NodoDer = aux;
                }
                else
                {
                    aux = padre.NodoIzq;
                    padre.NodoIzq = new ABB(i);
                    padre.NodoIzq.NodoIzq = aux;
                }
            }
            return arbol;
        }
        public static ABB Borra(ABB arbol, int i)
        {
            ABB actual = arbol;
            ABB padre = null;
            while (!EsVacio(actual))
            {
                padre = actual;
                if (actual.valor == i) break;
                actual = actual.valor < i ? actual.NodoDer : actual.NodoIzq;
            }
            if (padre == null || actual == null)
                throw new Exception("No se encontró el valor");
            else
            {
                if (padre.NodoDer.Equals(actual))
                    padre.NodoDer = actual.NodoDer;
                else if (padre.NodoIzq.Equals(actual))
                    padre.NodoIzq = actual.NodoIzq;

            }
            return arbol;
        }
        public static IEnumerable<int> RecorreInorden(ABB nodo)
        {
            if (nodo == null) yield break;
            else
            {
                foreach (int i in RecorreInorden(nodo.NodoIzq))
                    yield return i;
                yield return nodo.valor ?? 0;
                foreach (int i in RecorreInorden(nodo.NodoDer))
                    yield return i;
            }
        }
        public static IEnumerable<int> RecorrePreorden(ABB nodo)
        {
            if (nodo == null) yield break;
            else
            {
                yield return nodo.valor ?? 0;
                foreach (int i in RecorrePreorden(nodo.NodoIzq))
                    yield return i;
                foreach (int i in RecorrePreorden(nodo.NodoDer))
                    yield return i;
            }
        }
        public static IEnumerable<int> RecorrePostorden(ABB nodo)
        {
            if (nodo == null) yield break;
            else
            {
                foreach (int i in RecorrePostorden(nodo.NodoIzq))
                    yield return i;
                foreach (int i in RecorrePostorden(nodo.NodoDer))
                    yield return i;
                yield return nodo.valor ?? 0;
            }
        }
    }
}
