
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal
{

	public class Estrategia
	{
		
		public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            return "Implementar";
        }
        

        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol)
		{
			return ["HOLA"];
		}
        
        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
		{
            return [["Implementar"]];
        }

        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            List<ItemCat> listaProductos = new List<ItemCat>();

            RecolectarProductos(arbol, listaProductos);

            return listaProductos;
            
            /* 
            instanciar lista vacia []
            foreach recorriendo el arbol

            Agregar elementos a la lista vacía
            aplicar recursividad para no repetir el elemento y recorrer todo el árbol
            retorna lista con el total de los elementos
            */
        }
        private void RecolectarProductos(ArbolGeneral<ItemCat> nodo, List<ItemCat> lista)
        {
            if (nodo == null) return;

            if (nodo.getDatoRaiz().Tipo == TipoElemento.Producto)
            {
                lista.Add(nodo.getDatoRaiz());
            }
            foreach (var hijo in nodo.getHijos())
            {
                RecolectarProductos(hijo, lista);
            }
        }

        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
		{
            string[] nombresRuta = rutaAlPadre.Split('/', StringSplitOptions.RemoveEmptyEntries);

            ArbolGeneral<ItemCat> nodoActual = arbol;

            foreach (string nombreNodo in nombresRuta)
            {
                ArbolGeneral<ItemCat>? nodoHijoEncontrado = null;
                Cola<ArbolGeneral<ItemCat>> colaHijos = new Cola<ArbolGeneral<ItemCat>>();

                foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos())
                {
                    colaHijos.encolar(hijo);
                }
                while (!colaHijos.esVacia())
                {
                    ArbolGeneral<ItemCat> hijoAuxiliar = colaHijos.desencolar();
                    if (hijoAuxiliar.getDatoRaiz().Nombre == nombreNodo)
                    {
                        nodoHijoEncontrado = hijoAuxiliar;
                        break;
                    }
                }
                if (nodoHijoEncontrado == null)
                {
                    ItemCat nuevoDatoPadre = new ItemCat();
                    nodoHijoEncontrado = new ArbolGeneral<ItemCat>(nuevoDatoPadre);
                    nodoActual.agregarHijo(nodoHijoEncontrado);
                }
                nodoActual = nodoHijoEncontrado;
            }

            ArbolGeneral<ItemCat> nuevoNodo = new ArbolGeneral<ItemCat>(dato);
            nodoActual.agregarHijo(nuevoNodo);
            
            /*
                inserta item en ruta indicada
                if rutaAlPadre !=exist 
                    crea los nodos necesarios(agregarHijo())
                    inserta elemento (ItemCat dato) en esa ubicación
                
                inserta elemento (ItemCat dato) en esa ubicación
                
            */
        }

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
		{
            List<ItemCat> resultados = new List<ItemCat>() ;

            if(arbol == null)
            {
                return resultados;
            }

            string terminoBusqueda = elementoABuscar.Trim().ToLower();
            
            Cola<ArbolGeneral<ItemCat>> colaNodos = new Cola<ArbolGeneral<ItemCat>>();
            colaNodos.encolar(arbol);

            while (!colaNodos.esVacia())
            {
                ArbolGeneral<ItemCat> nodoActual = colaNodos.desencolar();
                ItemCat datoActual = nodoActual.getDatoRaiz();

                if(datoActual != null && !string.IsNullOrEmpty(datoActual.Nombre))
                {
                    string nombreEnMinuscula = datoActual.Nombre.Trim().ToLower();
                    if (nombreEnMinuscula.Contains(terminoBusqueda))
                    {
                        resultados.Add(datoActual);
                    }
                }
                foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos())
                {
                    colaNodos.encolar(hijo);
                }
            }
            return resultados;
            /*
            return [];
            instanciar lista vacia [] para guardar coincidencias
                string s1 = "El perro no puede comer";
                string s2 = "Perro";
                bool b = s1.Contains(s2.ToLower());

                Console.WriteLine(b);
                agregar elementos que coinciden con la busqueda a la lista vacía 
                aplicar recursividad para no repetir el elemento y recorrer todo el árbol
                retorna lista con el total de los elementos
            */

		}
            
    }
}