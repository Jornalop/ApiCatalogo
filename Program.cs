using Microsoft.Extensions.Hosting;
using tp1;
using tpfinal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.UseUrls("http://0.0.0.0:5000");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

var estrategia = new Estrategia();

// 1. Crear la raíz
ArbolGeneral<ItemCat> arbol = new ArbolGeneral<ItemCat>(
    new ItemCat("Catalogo Global", TipoElemento.Categoria)
);

Util.init(arbol);

// 1.Todos (GET)
app.MapGet("/api/catalogo/todos", () =>
{
        var result = estrategia.Todos(arbol);
        return Results.Ok( result );
   
})
.WithName("TodoselCatalogo")
.WithOpenApi();

// 2.Agregar (POST)
app.MapPost("/api/catalogo/agregar", (ItemCat nuevoDato, string rutaAlPadre) =>
{
    try
    {
        estrategia.Agregar(arbol, nuevoDato, rutaAlPadre);
        return Results.Created($"/api/catalogo/itemPorId/{nuevoDato.Id}", nuevoDato);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("AgregarProductoAlCatalogo")
.WithOpenApi();


// 3. Buscar (GET)
app.MapGet("/api/catalogo/buscar", (string elemento) =>
{
    var collected = estrategia.Buscar(arbol, elemento);
    
    return Results.Ok( collected );
})
.WithName("BuscaProductoDelCatalogo")
.WithOpenApi();


// 5. URLs SEO (GET)
app.MapGet("/api/catalogo/urls-seo", () =>
{
    return Results.Ok(estrategia.GetURLsSEO(arbol));
})
.WithName("GetURLsSEO")
.WithOpenApi();

// 6. URLs SEO por ID (GET)
app.MapGet("/api/catalogo/url-seoPorId", (int id) =>
{
    return Results.Ok(estrategia.GetUrlSeoPorId(arbol, id));
})
.WithName("url-seoPorId")
.WithOpenApi();

// 7. Consulta  por niveles (GET)
app.MapGet("/api/catalogo/niveles", () =>
{
    return Results.Ok(estrategia.ConsultaNiveles(arbol));
})
.WithName("ConsultaNiveles")
.WithOpenApi();

app.Run();


