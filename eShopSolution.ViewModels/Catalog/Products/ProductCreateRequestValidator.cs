
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Details).MaximumLength(500).WithMessage("Details can not over 500 characters");
        }
    }
}
