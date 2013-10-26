using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WpfApplication2.Interfaces;

namespace WpfApplication2.Helpers
{
    class AddElementCommand : ICommand<Visual>
    {
        private Visual _Value;

      public Visual Value
      {
         get
         {
            return _Value;
         }
         set
         {
            _Value = value;
         }
      }

      public AddElementCommand()
      {
         _Value = null;
      }
      public AddElementCommand(Visual value)
      {
         _Value = value;
      }

      public Visual Do(Visual value)
      {
         return _Value;
      }
      public Visual Undo(Visual value)
      {
         return _Value;
      }


    }
}
