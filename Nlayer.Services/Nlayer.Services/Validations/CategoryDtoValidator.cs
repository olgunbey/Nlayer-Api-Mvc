using FluentValidation;
using Nlayer.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Services.Validations
{
    public class CategoryDtoValidator:AbstractValidator<CategoryDTO>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen boş geçme")
                .NotNull().WithMessage("Lütfen boş geçme").MinimumLength(3).WithMessage("Lütfen 3 harften fazla bir değer girin");
        }
    }
}
