using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Interfaces
{
    public interface ICommand<T>
    {
        T Redo(T input);
        T Undo(T input);
    }
}
