using Soliter.Core;
using Soliter.Primitives;

namespace Soliter.Views
{
    public class FieldView
    {       
        public Cell[][] Field { get; }

        public FieldView(Map map)
        {
            Field = map.Matrix;
        }
    }
}
