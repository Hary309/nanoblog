﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nanoblog.Core
{
    public interface IMainWindow
    {
        void SetPageData(object content, object dataContext);
    }
}