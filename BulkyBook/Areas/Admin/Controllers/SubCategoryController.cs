using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = SD.Role_Admin)]
    public class SubCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IWebHostEnvironment _hostEnvironment;


        public SubCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            SubVm subVm = new SubVm()
            {
                SubCategory = new SubCategory(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };
            if (id == null)
            {
                //this is for create
                return View(subVm);
            }
            //this is for edit
            subVm.SubCategory = _unitOfWork.SubCategory.Get(id.GetValueOrDefault());
            if (subVm.SubCategory == null)
            {
                return NotFound();
            }
            return View(subVm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {

                if (categoryVM.SubCategory.Id == 0)
                {
                    _unitOfWork.SubCategory.Add(categoryVM.SubCategory);

                }
                else
                {
                    _unitOfWork.SubCategory.Update(categoryVM.SubCategory);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                categoryVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (categoryVM.SubCategory.Id != 0)
                {
                    categoryVM.SubCategory = _unitOfWork.SubCategory.Get(categoryVM.SubCategory.Id);
                }
            }
            return View(categoryVM);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SubCategory.GetAll(includeProperties: "Category");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.SubCategory.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.SubCategory.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}