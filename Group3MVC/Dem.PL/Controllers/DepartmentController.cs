using Dem.BLL.Interfaces;
using Dem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dem.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork) //Ask CLR For Creating object from class  Implement IUnitOfWork
        {
            _unitOfWork = unitOfWork;
        }
        //baseurl/Department/index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var Result = await _unitOfWork.CompleteAsync();
                //Temp Data =>Dictionary Object 
                //Transfer Data From Action To Action
                if (Result > 0)
                {
                    TempData["SuccessMessage"] = "Department Created Successfully";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();   //Status Code 400
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null)
                return NotFound();
            return View(ViewName, department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            //if(id is null)
            //    return BadRequest();
            //var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1. Log Error
                    //2. Display Error
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            ///if (id is null)
            ///    return BadRequest();
            ///var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            ///if (department is null)
            ///    return NotFound();
            ///return View(department);

            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {

                    _unitOfWork.DepartmentRepository.Delete(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }
    }
}
