using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using Nlayer.Core.ModelViews;
using Nlayer.Core.Services;
using Nlayer.Services.Exceptions;
using NLayer.WEB.Services;
using System.Text.Json;

namespace NLayer.WEB.Controllers
{
    public class ProductController : Controller
    {
       
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        public ProductController(IProductService _productService, ICategoryService categoryService, IMapper mapper)
        {
            productService = _productService;
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            CustomResponseDTO<List<ProductCategoryDTO>> ps = await productService.GetProductWithCategoryAsync();
            return View(ps.Data);
        }

        public async Task<IActionResult> Save()
        {
            IEnumerable<Category> categories =await categoryService.GetAllAsync();

            List<CategoryDTO> categoryDTO = mapper.Map<List<CategoryDTO>>(categories.ToList());

            ViewBag.Category = new SelectList(categoryDTO,"EntityID","Name"); 
            return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {

                await productService.AddAsync(mapper.Map<Product>(productDTO));
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<Category> categories = await categoryService.GetAllAsync();

            List<CategoryDTO> categoryDTO = mapper.Map<List<CategoryDTO>>(categories.ToList());

            ViewBag.Category = new SelectList(categoryDTO, "EntityID", "Name");

            return View();
         
        }
        public async Task<IActionResult> CategoriSec()
        {
            IEnumerable<Category> categories = await categoryService.GetAllAsync();
            List<CategoryDTO> categoryDTOs = mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.cs = new SelectList(categoryDTOs, "EntityID", "Name"); //ViewData.Values[0].Items
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CategoriSecc(int categoryID) 
        {
            bool categorys=await categoryService.AnyAsync(x=>x.EntityID==Convert.ToInt32(categoryID));
            
            if (categorys)
            {
                Category category = await categoryService.GetByIDAsync(Convert.ToInt32(categoryID));
                List<Product> products=productService.Where(x => x.CategoryID == category.EntityID).ToList();
                List<ProductDTO> productDTOs = mapper.Map<List<ProductDTO>>(products);
                string jsonSerializeProducts=JsonSerializer.Serialize(productDTOs);
                TempData["Products"]=jsonSerializeProducts;
                return RedirectToAction(nameof(CategoriItems));
            }
            else
            {
                throw new NotFoundException("bu kategoriye ait bir bilgi yok");
            }
        }
        public async Task<IActionResult> CategoriItems()
        {
            object jsonDeserializeProducts = TempData["Products"];
            List<ProductDTO> productDTOs=JsonSerializer.Deserialize<List<ProductDTO>>((string)jsonDeserializeProducts);
            return View(productDTOs);
        }
        
    }

}
