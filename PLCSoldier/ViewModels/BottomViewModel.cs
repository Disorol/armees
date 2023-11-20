﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Interfaces;

namespace PLCSoldier.ViewModels
{
    public class BottomViewModel : ITabItem
    {
        public string IdentificationName { get; set; }
        public string Header { get; set; }
        public bool isCloseButtonVisible { get; set; }
        public object? Content { get; set; }
    }
}