﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Interfaces
{
    public interface ITabItem
    {
        public string IdentificationName { get; set; }
        public string Header { get; set; }
        public bool isCloseButtonVisible { get; set; }
        public object? Content { get; set; }
    }
}