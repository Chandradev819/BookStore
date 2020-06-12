using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetSubCategory(int catId)
        {
            var data = _unitOfWork.SubCategory.GetAll(catId)
               .Where(x => x.CategoryId == catId)
               .Select(x => new { Value = x.Id, Text = x.Name });
            return Json(data);
        }
        public JsonResult GetMiniCategory(int subCateId)
        {
            var data = _unitOfWork.Subss.GetSubCate(subCateId)
                .Where(x => x.SubCategoryId == subCateId)
                .Select(x => new { Value = x.Id, Text = x.Name });
            return Json(data);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                SubCategoryList = _unitOfWork.SubCategory.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                SubssList = _unitOfWork.Subss.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            //this is for edit
            productVM.Product = _unitOfWork.Product.GetProductWithImgOnId(id.GetValueOrDefault());
            if (productVM.Product == null)
            {

                return NotFound();
            }
            return View(productVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                List<ProductImg> prodImgs = new List<ProductImg>();
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\products");
                        var extenstion = Path.GetExtension(files[i].FileName);

                        if (productVM.Product.Id != 0)
                        {
                            Product objProduct = _unitOfWork.Product.GetProductWithImgOnId(productVM.Product.Id);
                            //this is an edit and we need to remove old image
                            foreach (var item in objProduct.ImgDetails)
                            {
                                var imagePath = Path.Combine(webRootPath, item.ImageUrl.TrimStart('\\'));
                                if (System.IO.File.Exists(imagePath))
                                {
                                    System.IO.File.Delete(imagePath);
                                }
                            }
                        }
                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                        {
                            files[i].CopyTo(filesStreams);
                        }
                        ProductImg prodImg = new ProductImg()
                        {
                            ImageUrl = @"\images\products\" + fileName + extenstion
                        };
                        prodImgs.Add(prodImg);
                    }
                    productVM.Product.ImgDetails = prodImgs;
                }
                else
                {
                    //update when they do not change the image
                    if (productVM.Product.Id != 0)
                    {
                        Product objFromDb = _unitOfWork.Product.GetProductWithImgOnId(productVM.Product.Id);
                        foreach (var item in objFromDb.ImgDetails)
                        {
                            ProductImg prodImg = new ProductImg()
                            {
                                ImageUrl = item.ImageUrl
                            };
                            prodImgs.Add(prodImg);
                        }
                        productVM.Product.ImgDetails = prodImgs;
                    }
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);

                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVM.SubCategoryList = _unitOfWork.SubCategory.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVM.SubssList = _unitOfWork.Subss.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
                }
            }
            return View(productVM);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll(includeProperties: "Category,SubCategory,Subss");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Product.GetProductWithImgOnId(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            string imagePath = string.Empty;
            foreach (var item in objFromDb.ImgDetails)
            {
                imagePath = Path.Combine(webRootPath, item.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            //Delete the record
            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion

    }
}