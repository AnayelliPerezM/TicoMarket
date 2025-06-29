using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ticomarkenet.Data;
using ticomarkenet.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace ticomarkenet.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Productoes de prueba
        [Authorize(Roles = "ADMIN,VEN")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Productos.Include(p => p.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
    
        public IActionResult Create()
        {

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios.Select(u => new { u.UsuarioId, Nombre = u.Nombre }),
            "UsuarioId", "Nombre");

            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Descripcion,Precio,Categoria,UsuarioId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", producto.UsuarioId);
                return RedirectToAction(nameof(Index));
            }
           
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", producto.UsuarioId);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Descripcion,Precio,Categoria,UsuarioId")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Vista");
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", producto.UsuarioId);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Vista");
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }
   
        public IActionResult GestionProducto()
        {
            return View();
        }
        //


        //  


        //public IActionResult Vista()
        //{
        //    ViewBag.Usuarios = _context.Usuarios
        //        .Select(u => new SelectListItem
        //        {
        //            Value = u.UsuarioId.ToString(),
        //            Text = u.Nombre
        //        }).ToList();

        //    // 🔧 Ahora sí se cargan las imágenes
        //    var productos = _context.Productos
        //        .Include(p => p.Imagenes)
        //        .ToList();

        //    return View(productos);
        //}
        [Authorize(Roles = "ADMIN,VEN")]
        public IActionResult Vista()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            ViewBag.Usuarios = _context.Usuarios
                .Select(u => new SelectListItem
                {
                    Value = u.UsuarioId.ToString(),
                    Text = u.Nombre
                }).ToList();

            List<Producto> productos;

            if (rol == "VEN" && usuarioId.HasValue)
            {

                productos = _context.Productos
                    .Include(p => p.Imagenes)
                    .Where(p => p.UsuarioId == usuarioId.Value)
                    .ToList();
            }
            else
            {

                productos = _context.Productos
                    .Include(p => p.Imagenes)
                    .ToList();
            }




            return View(productos);
        }




        // Asegúrate de tener esta referencia arriba

        public IActionResult Detalles(int id)
    {
        var producto = _context.Productos.Include(p => p.Usuario).FirstOrDefault(p => p.ProductoId == id);
        if (producto == null)
        {
            return NotFound();
        }

        var relacionados = _context.Productos
            .Where(p => p.Categoria == producto.Categoria && p.ProductoId != id)
            .ToList();

        ViewData["ProductoJson"] = JsonSerializer.Serialize(producto);
        ViewData["RelacionadosJson"] = JsonSerializer.Serialize(relacionados);

        return View();
    }


        //Creado para guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(
            [Bind("ProductoId,Nombre,Descripcion,Precio,Categoria,UsuarioId")] Producto producto,
            IFormFile[] files,
            [FromForm(Name = "imagenesParaEliminar")] string imagenesParaEliminarJson)
        {
            try
            {
                var nuevasImagenes = new List<Imagen>();
                var imagenesExistentes = new List<Imagen>();

                // EDICIÓN: recuperar producto existente
                if (producto.ProductoId != 0)
                {
                    var productoExistente = await _context.Productos
                        .Include(p => p.Imagenes)
                        .FirstOrDefaultAsync(p => p.ProductoId == producto.ProductoId);

                    if (productoExistente != null)
                    {
                        imagenesExistentes = productoExistente.Imagenes?.ToList() ?? new List<Imagen>();

                        if (!string.IsNullOrEmpty(imagenesParaEliminarJson))
                        {
                            var idsEliminar = JsonSerializer.Deserialize<List<int>>(imagenesParaEliminarJson);
                            var imagenesAEliminar = _context.Imagenes
                                .Where(img => idsEliminar.Contains(img.ImagenId))
                                .ToList();

                            _context.Imagenes.RemoveRange(imagenesAEliminar);
                            await _context.SaveChangesAsync();

                            imagenesExistentes.RemoveAll(img => idsEliminar.Contains(img.ImagenId));
                        }
                    }
                }

                // SUBIDA DE IMÁGENES NUEVAS
                if (files != null && files.Length > 0)
                {
                    var rutaRaiz = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");
                    Directory.CreateDirectory(rutaRaiz); // Crea si no existe

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var nombreArchivo = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                            var rutaCompleta = Path.Combine(rutaRaiz, nombreArchivo);

                            using var stream = new FileStream(rutaCompleta, FileMode.Create);
                            await file.CopyToAsync(stream);

                            nuevasImagenes.Add(new Imagen
                            {
                                Ruta = "/imagenes/" + nombreArchivo,
                                Producto = producto
                            });
                        }
                    }
                }

                producto.Imagenes = imagenesExistentes.Concat(nuevasImagenes).ToList();

                if (producto.ProductoId == 0)
                {
                    _context.Productos.Add(producto);
                    TempData["Mensaje"] = "Producto creado correctamente.";
                }
                else
                {
                    _context.Productos.Update(producto);
                    TempData["Mensaje"] = "Producto actualizado correctamente.";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Vista");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar: " + ex.Message);
                ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar el producto.");
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", producto.UsuarioId);
                //return View("Vista", producto);
                //-------------------------------------------------
                // Recargar productos + usuarios para la vista
                ViewBag.Usuarios = _context.Usuarios
                    .Select(u => new SelectListItem
                    {
                        Value = u.UsuarioId.ToString(),
                        Text = u.Nombre
                    }).ToList();

                var productos = _context.Productos
                    .Include(p => p.Imagenes)
                    .ToList();

                return View("Vista", productos);

                //-------------------------------------------------
            }
        }





    }
}
