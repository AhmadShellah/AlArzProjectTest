using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlArz.MenuManagment.Interfaces;
using Volo.Abp.AspNetCore.Mvc;

namespace AlArz.Controllers.MenuManagment
{
    public class MenuTypeController : AbpController, IMenuTypeService
    {
        private readonly IMenuTypeService _menuTypeService;

        public MenuTypeController(IMenuTypeService menuTypeService)
        {
            _menuTypeService = menuTypeService;
        }
    }
}
