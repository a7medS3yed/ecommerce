using Admin.Dashbord.Helpers;
using Admin.Dashbord.Models.Products;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Persistance.UnitOfWork;
using ECommerce.Service.Specification.ProductsSpecification;
using ECommerce.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Dashbord.Controllers
{
    public class ProductsController(IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var productRepo = unitOfWork.GenaricRepository<Product, int>();

            var queryParam = new ProductQueryParam();
            var productSpecification = new ProductsWithTypesAndBrandsSpecification(queryParam);
            var products = await productRepo.GetAllAsync(productSpecification);

            var productViewModel = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                BrandId = product.BrandId,
                TypeId = product.TypeId,
                Brand = product.ProductBrand,
                Type = product.ProductType
            });

            return View(productViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                    model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
                //else
                //    // adding default image

                var mappedProduct = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    BrandId = model.BrandId,
                    TypeId = model.TypeId,
                    Price = model.Price,
                    PictureUrl = model.PictureUrl!
                };

                var productRepo = unitOfWork.GenaricRepository<Product, int>();
                await productRepo.AddAsync(mappedProduct);
                await unitOfWork.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);


        }
    }
}
