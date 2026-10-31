using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace tpfinal
{

	public class Estrategia
	{
		string protocoloURL = "https";
        string dominioURL = "www.tienda.com";
		public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            return buscarRutaPorId(arbol, id,"");
        }
        
        private string buscarRutaPorId(ArbolGeneral<ItemCat> arbol, int id, string rutaAcumulada)
        {
            string limpieza = arbol.getDatoRaiz().Nombre.ToLower().Trim();
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string rutaActual = textInfo.ToTitleCase(limpieza).Replace(" ","");

            rutaActual = $"{rutaAcumulada}/{rutaActual}";  
            
            if (arbol.getDatoRaiz().Id == id)
            {
                return $"{protocoloURL}://{dominioURL}{rutaActual}";
            }
            
            foreach (var subcategoría in arbol.getHijos())
                {
                    string posibleRuta = buscarRutaPorId(subcategoría, id, rutaActual);
                        
                    if (posibleRuta != null)
                    {
                        return posibleRuta;
                    }    
                }
            return null!;
        }


        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol)
		{
            List<string> urls = new List<string>();

            GetURLsSEORecursivo(arbol,"",urls);

            return urls;
		}

        
        private void GetURLsSEORecursivo(ArbolGeneral<ItemCat> nodo, string rutaActual, List<string> urls)
        {
            string nuevoCamino;
            
            string capitalize = nodo!.getDatoRaiz().Nombre.ToLower().Trim();
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string nombreSEO = textInfo.ToTitleCase(capitalize).Replace(" ", "");

            if (string.IsNullOrEmpty(rutaActual)) 
            {
                nuevoCamino = nombreSEO;
            }
            else
            {
                nuevoCamino = $"{rutaActual}/{nombreSEO}";
            }
            
            if (nodo.esHoja())
            {
                urls.Add($"{protocoloURL}://{dominioURL}/{nuevoCamino}");
                return;
            }

            else
            {
                foreach (var hijo in nodo.getHijos())
                {
                    GetURLsSEORecursivo(hijo, nuevoCamino,urls);
                }
            }

        }
        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
        {
            List<List<string>> sublistaNiveles = new List<List<string>>();

            if (arbol == null) return sublistaNiveles;

            Cola<ArbolGeneral<ItemCat>> colaAuxiliar = new Cola<ArbolGeneral<ItemCat>>();
            colaAuxiliar.encolar(arbol);

            while (colaAuxiliar.cantidadElementos() > 0)
            {
                int cantidadNodosNivelActual = colaAuxiliar.cantidadElementos();
                List<string> listaNivelActual = new List<string>();
                
                for (int i = 0; i < cantidadNodosNivelActual; i++)
                {
                   ArbolGeneral<ItemCat> nodoActual = colaAuxiliar.desencolar(); 

                   listaNivelActual.Add(nodoActual.getDatoRaiz().Nombre);

                   foreach(var hijo in nodoActual.getHijos())
                    {
                        colaAuxiliar.encolar(hijo);
                    }
                }

                sublistaNiveles.Add(listaNivelActual);
            }
            return sublistaNiveles;
        }
        

        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            List<ItemCat> listaProductos = new List<ItemCat>();

            RecolectarProductos(arbol, listaProductos);

            return listaProductos;
        }
        private void RecolectarProductos(ArbolGeneral<ItemCat> nodo, List<ItemCat> lista)
        {
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

                foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos())
                {
                    if (hijo.getDatoRaiz().Nombre == nombreNodo)
                    {
                        nodoHijoEncontrado = hijo;
                        break;
                    }
                }
                if (nodoHijoEncontrado == null)
                {
                    ItemCat nuevoDatoPadre = new ItemCat(nombreNodo, TipoElemento.Categoria);
                    nodoHijoEncontrado = new ArbolGeneral<ItemCat>(nuevoDatoPadre);
                    nodoActual.agregarHijo(nodoHijoEncontrado);
                }
                nodoActual = nodoHijoEncontrado;
            }

            ArbolGeneral<ItemCat> nuevoNodo = new ArbolGeneral<ItemCat>(dato);
            nodoActual.agregarHijo(nuevoNodo);
        }

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
		{
            List<ItemCat> resultados = new List<ItemCat>() ;

            string terminoBusqueda = elementoABuscar.Trim().ToLower();
            
            Cola<ArbolGeneral<ItemCat>> colaNodos = new Cola<ArbolGeneral<ItemCat>>();
            colaNodos.encolar(arbol);

            while (!colaNodos.esVacia())
            {
                ArbolGeneral<ItemCat> nodoActual = colaNodos.desencolar();
                ItemCat datoActual = nodoActual.getDatoRaiz();

                string nombreEnMinuscula = datoActual.Nombre.Trim().ToLower();
                if (nombreEnMinuscula.Contains(terminoBusqueda))
                {
                    resultados.Add(datoActual);
                    break;
                }
                foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos())
                {
                    colaNodos.encolar(hijo);
                }
            }
            return resultados;
		}
            
    }
}