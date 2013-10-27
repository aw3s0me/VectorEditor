using System.Collections.Generic;
using WpfApplication2.Interfaces;

namespace WpfApplication2.Layers
{
    public class UndoRedoLayer
    {

        private static readonly UndoRedoLayer Instance = new UndoRedoLayer();

        private Stack<ICommand> _undo;
        private Stack<ICommand> _redo;

        public static UndoRedoLayer GetInstance
        {
            get
            {
                return Instance;
            }
        }

        public int UndoCount
        {
            get
            {
                return _undo.Count;
            }
        }

        public int RedoCount
        {
            get
            {
                return _redo.Count;
            }
        }

        public void Add(ICommand cmd)
        {
            _undo.Push(cmd);
            _redo.Clear();
        }

        public UndoRedoLayer()
        {
            Reset();
        }

        public void Reset()
        {
            _undo = new Stack<ICommand>();
            _redo = new Stack<ICommand>();
        }

     /*   public void Do(ICommand command)
        {
            T output = command.Redo(input);
            _undo.Push(command);
            _redo.Clear(); //при новой команде очищается redo стек
            return output;
        } */

        public void Undo()
        {
            if (_undo.Count <= 0) return;
            var cmd = _undo.Pop();
            cmd.Undo();
            //T output = cmd.Undo(input);
            _redo.Push(cmd);
            //return output;
        }
        public void Redo()
        {
            if (_redo.Count <= 0) return;
            var cmd = _redo.Pop();
            cmd.Redo();
            //T output = cmd.Redo(input);
            _undo.Push(cmd);
            //return output;
            //return input;
        }
    }
}
