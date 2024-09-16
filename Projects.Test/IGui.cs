﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Test
{
    public interface IGui
    {
        //Connect gui to controller
        //(This method will be called before any other methods)
        void Connect(IController controller);

        //Initialise the gui
        void Init();

        //Called to change the displayed text
        void SetDisplay(string s);
    }
}
