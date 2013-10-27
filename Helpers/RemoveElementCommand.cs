using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WpfApplication2.Interfaces;
using WpfApplication2.Layers;

namespace WpfApplication2.Helpers
{
    public class RemoveElementCommand : ICommand
    {
        private Visual _Value;
        public CanvasLayer CurCanv;

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

      public RemoveElementCommand(Visual value, CanvasLayer curCanv)
      {
         _Value = value;
          CurCanv = curCanv;
      }

      public void Redo(/*Visual value , ref CanvasLayer canvas */)
      {
          if (CurCanv!=null && _Value!=null)
              CurCanv.DeleteVisual(_Value);
          //canvas.AddVisual(value);
          //return _Value;
      }
      public void Undo(/*Visual value , ref CanvasLayer canvas */)
      {
          if (CurCanv != null && _Value != null)
              CurCanv.AddVisual(_Value);
          //canvas.DeleteVisual(value);
          //return _Value;
      }

    }
}
