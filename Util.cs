
using System;
using System.Collections.Generic;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal
{

	public class Util
	{

        public static void init(ArbolGeneral<ItemCat> catalogo)
            {

                Console.WriteLine("Generando catálogo masivo...");

                // 2. Definir grandes grupos de categorías
				//
                string[] sectores = { "Electrónica", "Moda", "Hogar", "Deportes", "Belleza", "Juguetes" };
                Random rnd = new Random();

                foreach (var sectorNombre in sectores)
                {
                    // Crear la categoría principal (ej: Electrónica)
                    ArbolGeneral<ItemCat> sector = new ArbolGeneral<ItemCat>(
                        new ItemCat(sectorNombre, TipoElemento.Categoria)
                    );
                    catalogo.agregarHijo(sector);

                    // Generar subcategorías de forma asimétrica (entre 2 y 6 subcategorías por sector)
                    int numSubCategorias = rnd.Next(2, 7);
                    for (int i = 1; i <= numSubCategorias; i++)
                    {
                        ArbolGeneral<ItemCat> subCat = new ArbolGeneral<ItemCat>(
                            new ItemCat($"{sectorNombre} - Grupo {i}", TipoElemento.Categoria)
                        );
                        sector.agregarHijo(subCat);

                        // Generar productos (hojas) dentro de cada subcategoría
                        // Generamos entre 5 y 15 productos por subcategoría
						//"MacBook Pro", "iPhone 15 Pro", "iPad Pro M2", "Studio Display", "Cámara Pro Lens Z-100"
                        int numProductos = rnd.Next(5, 16);
                        for (int j = 1; j <= numProductos; j++)
                        {
                            ItemCat producto = new ItemCat(
                                $"{subCat.getDatoRaiz().Nombre} Prod {j}",
                                TipoElemento.Producto,
                                sku: $"SKU-{rnd.Next(1000, 9999)}",
                                precio: Math.Round(rnd.NextDouble() * 500, 2)
                            );
                            subCat.agregarHijo(new ArbolGeneral<ItemCat>(producto));
                        }
                    }
                }

            }

    }
}