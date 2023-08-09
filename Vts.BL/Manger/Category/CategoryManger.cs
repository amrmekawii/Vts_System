using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vts.DAL;

namespace Vts.BL
{
    public class CategoryManger : ICategoryManger
    {
        private readonly ICatRepo _categrtRepo;

        CategoryManger(ICatRepo categrtRepo)
        {
            _categrtRepo = categrtRepo;
        }

        public void AddCategory(AddCategortDto CategortDto)
        {
            var NewCat = new Category
            {
                Id = Guid.NewGuid(),
                Name = CategortDto.Name,
            };

            _categrtRepo.Add(NewCat);
            _categrtRepo.Save();
        }

        public void EditUsingViewModel(EditCatDto editCatDto)
        {
            Category CAT2 = _categrtRepo.GetById(editCatDto.Id);

            CAT2.Name = editCatDto.Name;

            _categrtRepo.Save();
        }

        public ReadCategoryDto? GetByIdAsEditViewModel(Guid id)
        {
            Category cat = _categrtRepo.GetById(id);
            if (cat is null)
            {
                return null;
            }
            return new ReadCategoryDto
            {
                Id = cat.Id,
                Name = cat.Name,

            };
        }

        public DetailsCategoryDto? GetDetails(Guid id)
        {
            Category cats = _categrtRepo.GetDetailsCat(id);
            return
                new DetailsCategoryDto
                {
                    Name = cats.Name,
                    Items = cats.Items.Select(p=> new ItemDto
                    {
                        Name= p.Name,
                        Description= p.Description,
                           ImageURL= p.ImageURL,
                           Price = p.Price,
                    }).ToList(),
                };
        }

        public void RemoveCategory(Guid id)
        {
            Category cat = _categrtRepo.GetById(id);
            _categrtRepo.Remove(cat);
            _categrtRepo.Save();

        }
    }
}
