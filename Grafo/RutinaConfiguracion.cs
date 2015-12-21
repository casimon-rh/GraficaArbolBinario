using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grafo
{
    public class RutinaConfiguracion : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private List<PointPlainObject> puntos = new List<PointPlainObject>();
        private List<LinePlainObject> lineas = new List<LinePlainObject>();
        private List<ElementoDeArbol> nodos = new List<ElementoDeArbol>();
        private LinePlainObject linea = new LinePlainObject();
        private ElementoDeArbol nodo = new ElementoDeArbol();
        private int num;
        private ABB _Arbol;

        #region Public
        public List<PointPlainObject> Puntos
        {
            get { return puntos; }
            set { puntos = value; NotifyPropertyChanged("Puntos"); }
        }
        public List<LinePlainObject> Lineas
        {
            get { return lineas; }
            set { lineas = value; NotifyPropertyChanged("Lineas"); }
        }
        public LinePlainObject Linea
        {
            get { return linea; }
            set { linea = value; NotifyPropertyChanged("Linea"); }
        }

        public List<ElementoDeArbol> Nodos
        {
            get { return nodos; }
            set { nodos = value; NotifyPropertyChanged("Nodos"); }
        }

        public ElementoDeArbol Nodo
        {
            get { return nodo; }
            set { nodo = value; NotifyPropertyChanged("Nodo"); }
        }
        public int Num
        {
            get { return num; }
            set
            {
                num = value;
                //addRemovePoints(); 
                NotifyPropertyChanged("Num");
            }
        }
        public ABB Arbol
        {
            get { return _Arbol; }
            set
            {
                _Arbol = value;

            }
        }
        #endregion

        public RutinaConfiguracion() { num = 0; }
        public RutinaConfiguracion(int n) { num = n; }



        public void ActualizaListaDeNodos()
        {
            try
            {
                Arbol = null;
                foreach (int i in Nodos.Select(x => x.valor).ToList())
                    Arbol = UsoABB.Inserta(Arbol, i);
                bool derecha = false;
                int anterior = -1;
                int lvAnt = 0;
                foreach (int i in UsoABB.RecorreAnchura(Arbol))
                {
                    int lv = 0;
                    UsoABB.buscaNodo(i, Arbol, ref lv, ref derecha);

                    PointPlainObject pAnterior = null;
                    if (anterior != -1)
                    {
                        pAnterior = (Puntos.Where(x => x.id == anterior).FirstOrDefault());
                    }
                    int xx = 0;
                    if (derecha && anterior != -1)
                        xx = (50 / ((lv - 1) == 0 ? 50 : (lv - 1))) + (50 / lv) + pAnterior.x;
                    else if (anterior != -1)
                        xx = pAnterior.x - (50 / lv);
                    else xx = (50 / lv);

                    PointPlainObject pActual = new PointPlainObject() { id = i, x = xx, y = 50 - (lv * 3) };
                    if (Puntos.Where(x => x.id == i).Count() == 0)
                    {
                        Puntos.Add(pActual);
                        if (anterior != -1)
                        {
                            LinePlainObject ln = new LinePlainObject() { uno = pAnterior, dos = pActual };
                            if (lineas.Where(x => (x.uno.id == pAnterior.id && x.dos.id == (pActual.id)) || (x.uno.id == pActual.id && x.dos.id == (pAnterior.id))).Count() == 0 && lv != lvAnt)
                                Lineas.Add(ln);
                        }
                    }
                    anterior = i;
                    lvAnt = lv;
                }
            }
            catch (Exception)
            {
            }

        }

        public void Inserta(object Sender, RoutedEventArgs e)
        {
            Nodos.Add(new ElementoDeArbol() { valor = Num });
            UsoABB.Inserta(Arbol, Num);
            int anterior = -1;
            bool derecha = false;
            if (Puntos.Where(x => x.id == Num).Count() == 0)
            {
                int lv = 0;
                UsoABB.buscaNodo(Num, Arbol, ref lv, ref derecha);
                PointPlainObject pActual = new PointPlainObject() { id = Num, x = (derecha ? 50 / ((lv - 1) == 0 ? 50 : (lv - 1)) : 0) + (50 / lv), y = 50 - (lv * 3) };
                Puntos.Add(pActual);
                foreach (int i in UsoABB.RecorreAnchura(Arbol))
                {
                    if (i == Num)
                    {
                        PointPlainObject pAnterior = (Puntos.Where(x => x.id == anterior).FirstOrDefault());
                        Lineas.Add(new LinePlainObject() { uno = pAnterior, dos = pActual });
                    }
                    anterior = i;
                }

            }
        }

        public void Borra(object Sender, RoutedEventArgs e)
        {
            Nodos.Remove((Nodos.Where(x => x.valor == Num).FirstOrDefault()));
            UsoABB.Borra(Arbol, Num);
            ActualizaListaDeNodos();
        }




        //public void addRemovePoints()
        //{
        //    Random r = new Random();
        //    if (Puntos.Count <= num)
        //    {
        //        int c = Puntos.Count;
        //        for (int i = 0; i < num - c; i++)
        //        {
        //            while (true)
        //            {
        //                int x = r.Next(0, 50);
        //                int y = r.Next(0, 50);
        //                if (Puntos.Where(p => p.x == x && p.y == y).ToList().Count == 0)
        //                {
        //                    Puntos.Add(new PointPlainObject() { id = Puntos.Count+1, x = x, y = y });
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //        while (Puntos.Count > num)
        //        {
        //            var p = Puntos[Puntos.Count - 1];
        //            if (Lineas != null && Lineas.Count > 0)
        //            {
        //                var coincide = Lineas.Where(l => l.id1 == p.id || l.id2 == p.id).Select(l => l).ToList();
        //                foreach (LinePlainObject _l in coincide)
        //                    Lineas.Remove(_l);
        //            }
        //            Puntos.Remove(p);
        //        }
        //    num = num;
        //}
    }
    public class PointPlainObject
    {
        public int x { get; set; }
        public int y { get; set; }
        public int id { get; set; }
        public string display { get { return id + ":" + "(" + x + "," + y + ")"; } }
    }
    public class LinePlainObject
    {
        public string otro { get; set; }
        public int id1 { get { return uno.id; } }
        public int id2 { get { return dos.id; } }
        private PointPlainObject _uno = new PointPlainObject();
        private PointPlainObject _dos = new PointPlainObject();

        public PointPlainObject dos
        {
            get { return _dos; }
            set { _dos = value; }
        }
        public PointPlainObject uno
        {
            get { return _uno; }
            set { _uno = value; }
        }
    }
    public class ElementoDeArbol
    {
        public string otro { get; set; }
        public int valor { get; set; }
    }
    public class ABB
    {
        public ABB() { }
        public ABB(int v){valor = v;}


        public int? valor;
        public ABB NodoIzq;
        public ABB NodoDer;
    }
    
    public static class UsoABB
    {
        public static ABB buscaNodo(int i, ABB current, ref int lv, ref bool Der)
        {
            lv++;
            if (EsVacio(current)) return null;
            else if (current.valor == i) return current;
            else if (current.valor < i)
            {
                Der = (current ?? new ABB(-1)).valor < i;
                return buscaNodo(i, current.NodoDer, ref lv, ref Der);
            }
            else
            {
                Der = (current ?? new ABB(-1)).valor < i;
                return (buscaNodo(i, current.NodoIzq, ref lv, ref Der));
            }
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
        public static IEnumerable<int> RecorreAnchura(ABB nodo)
        {
            if (nodo == null) yield break;
            else
            {
                yield return nodo.valor ?? 0;
                if (nodo.NodoIzq != null)
                    yield return nodo.NodoIzq.valor ?? 0;
                yield return nodo.valor ?? 0;
                if (nodo.NodoDer != null)
                    yield return nodo.NodoDer.valor ?? 0;

                foreach (int i in RecorreAnchura(nodo.NodoIzq))
                    yield return i;
                foreach (int i in RecorreAnchura(nodo.NodoDer))
                    yield return i;
            }
        }

    }
}