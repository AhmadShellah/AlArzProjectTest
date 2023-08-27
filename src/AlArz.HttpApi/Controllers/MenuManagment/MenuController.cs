﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlArz.MenuManagment.Interfaces;
using Volo.Abp.AspNetCore.Mvc;

namespace AlArz.Controllers.MenuManagment
{
    public class MenuController : AbpController, IMenuService
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
    }
}
