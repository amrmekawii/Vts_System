using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vts.BL
{
    public interface ICategoryManger
    {

        void AddCategory(AddCategortDto CategortDto);
        void RemoveCategory(Guid id);

        void EditUsingViewModel(EditCatDto editCatDto);
        ReadCategoryDto? GetByIdAsEditViewModel(Guid id);
        DetailsCategoryDto? GetDetails(Guid id);
    }
}
