using System;
using System.IO;
using System.Linq;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            //return product home page to /Product/Index
            return View();
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            //retrieve all products from db and return them in JSON format to /Product/GetAll page 
            var allObj = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new {data = allObj});
        }
        
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //initialise a new product view model 
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                //Create dropdown lists for category and cover type
                CategoryList = _unitOfWork.Category.GetAll().Select(category => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(coverType => new SelectListItem
                {
                    Text = coverType.Name,
                    Value = coverType.Id.ToString()
                })
            };
            
            //if id is null, return a product upsert form page with the empty product view model to /Product/Upsert
            if (id == null)
            {
                //this is for create
                return View(productViewModel);
            }
            
            //return a product upsert form page with the existing product in product view model to /Product/Upsert?id
            productViewModel.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productViewModel.Product == null)
            {
                return NotFound();
            }
            
            return View(productViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            //when a product upsert from is posted to Product/Upsert
            //check all the validation defined in model - double check here
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                //retrive the files
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Join(webRootPath, "images/products");
                    var extension = Path.GetExtension(files[0].FileName);
                    
                    if (productViewModel.Product.ImageUrl != null)
                    {
                        //this is an edit and we need to remove old image
                        var imagePath = Path.Join(webRootPath, productViewModel.Product.ImageUrl);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var fileStreams =
                        new FileStream(Path.Join(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    productViewModel.Product.ImageUrl = @"/images/products/" + fileName + extension;
                }
                else
                {
                    //update when they do not change the image
                    Product objFromDb = _unitOfWork.Product.Get(productViewModel.Product.Id);
                    productViewModel.Product.ImageUrl = objFromDb.ImageUrl;
                }
                
                if (productViewModel.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productViewModel.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productViewModel.Product);
                    
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //if the model state is not valid and the validation is not handled by client 
                productViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(category => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

                productViewModel.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(coverType => new SelectListItem
                {
                    Text = coverType.Name,
                    Value = coverType.Id.ToString()
                });

                if (productViewModel.Product.Id != 0)
                {
                    productViewModel.Product = _unitOfWork.Product.Get(productViewModel.Product.Id);
                }
            }
            //if the validation fails at server side, return the form to refill
            return View(productViewModel);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //get the item by id to delete from db
            var objFromDb = _unitOfWork.Product.Get(id);
            
            //if the item to delete not found, return error messages
            if (objFromDb == null)
            {
                return Json(new
                    {success = false, message = "There was an error for deleting this item. Please try again later."});
            }
            
            //get the item's image path and delete the image in the static image folder
            string webRootPath = _hostEnvironment.WebRootPath;
            //this is an edit and we need to remove old image
            var imagePath = Path.Join(webRootPath, objFromDb.ImageUrl);
            Console.WriteLine("delete path: " +  imagePath);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
                    
            //delete the item from db
            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new {success = true, message = "The item was deleted."});
        }
    }
}