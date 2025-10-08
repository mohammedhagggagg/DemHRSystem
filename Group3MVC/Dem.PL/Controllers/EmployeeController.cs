using AutoMapper;
using Dem.BLL.Interfaces;
using Dem.DAL.Models;
using Dem.PL.Helpers;
using Dem.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dem.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,//Ask CLR For Object From Class Implemen interface IUnitOfWork
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task <IActionResult> Index(string searchValue)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(searchValue))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                ///1.ViewData =>KeyValuePair [Dictionary Object]
                ///Transfer Data From Controller [Action] To its View
                /// .Net Framework 3.5
                ///ViewData["Message"] = "Hello From ViewData";
                ///2. ViewBag => Dynamic Property [Wrapper On ViewData]
                ///Transfer Data From Controller [Action] To its View
                /// .Net Framework 4.0
                ///ViewBag.Message = "Hello From ViewBag";
                ///3. TempData => KeyValuePair [Dictionary Object]
                ///Transfer Data From Current Request To Next Request [Redirection]
                ///TempData["Message"] = "Hello From TempData";
                ///4. Strongly Typed View [Recommended]
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(searchValue);
            }
            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(MappedEmployees);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVm)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                ///var MappedEmployee = new Employee() 
                ///{
                ///    Name= employeeVm.Name,
                ///    Age = employeeVm.Age,
                ///    Address = employeeVm.Address,
                ///    Salary = employeeVm.Salary,
                ///    IsActive = employeeVm.IsActive,
                ///    Email = employeeVm.Email,
                ///    PhonwNumber = employeeVm.PhonwNumber,
                ///    HireDate = employeeVm.HireDate,
                ///    DepartmentId = employeeVm.DepartmentId
                ///};
                ///

               string fileName =  DocumentSettings.UploadFile(employeeVm.Image, "Images");
                employeeVm.ImageName = fileName;
                //_mapper.Map<Sourse,Destenation>(objectVm);
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                ///var Result = _unitOfWork.EmployeeRepository.Add(MappedEmployee);
                ///if(Result > 0) 
                ///{
                ///    TempData["SuccessMessage"] = "Employee Created Successfully";
                ///}

               await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
                var Result = await _unitOfWork.CompleteAsync();
                if (Result > 0)
                {
                    TempData["SuccessMessage"] = "Employee Created Successfully";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVm);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();   //Status Code 400
            var employee =await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, MappedEmployee);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVm, [FromRoute] int id)
        {
            if (id != employeeVm.Id)
                return BadRequest();

            if (ModelState.IsValid) //Server Side Validation
            {
                try
                {

                    string fileName = DocumentSettings.UploadFile(employeeVm.Image, "Images");
                    employeeVm.ImageName = fileName;
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                   await _unitOfWork.CompleteAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVm, [FromRoute] int id)
        {
            if (id != employeeVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                  int Result = await _unitOfWork.CompleteAsync();
                    if (Result > 0 && employeeVm.ImageName is not null) 
                    {
                        DocumentSettings.DeleteFile(employeeVm.ImageName, "Images");
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVm);

        }
    }
}
