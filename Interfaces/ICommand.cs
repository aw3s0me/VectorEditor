using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Interfaces
{
    public interface ICommand
    {
        void Redo();
        void Undo();
    }
}
