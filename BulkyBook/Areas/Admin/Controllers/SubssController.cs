using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class SubssController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IWebHostEnvironment _hostEnvironment;


        [BindProperty]
        public SubSubVM subsubvm { get; set; }
        public SubssController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            SubSubVM subSubVM = new SubSubVM()
            {
                Subss = new Subss(),
                Categorylist = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                SubCategorylist = _unitOfWork.SubCategory.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(subSubVM);
            }
            //this is for edit
           
            subSubVM.Subss = _unitOfWork.Subss.Get(id.GetValueOrDefault());
            if (subSubVM.Subss == null)
            {
                return NotFound();
            }
            return View(subSubVM);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SubSubVM subSubVM)
        {
            if (ModelState.IsValid)
            {

                if (subSubVM.Subss.Id == 0)
                {
                    _unitOfWork.Subss.Add(subSubVM.Subss);

                }
                else
                {
                    _unitOfWork.Subss.Update(subSubVM.Subss);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (subSubVM.Subss.Id != 0)
                {
                    subSubVM.Subss = _unitOfWork.Subss.Get(subSubVM.Subss.Id);
                }
            }
            return View(subSubVM);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Subss.GetAll(includeProperties: "Category,SubCategory");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Subss.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Subss.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}