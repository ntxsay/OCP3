﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Utilities.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorMissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessage = "ErrorMissingStock")]
        [Range(1, int.MaxValue, ErrorMessage = "StockNotGreaterThanZero")]
        public string Stock { get; set; }

        [Required(ErrorMessage = "ErrorMissingPrice")]
        [RegularExpression(@"^\d+(,\d{1,2})*$", ErrorMessage = "PriceNotANumber")]
        [DoubleParserValidation(ErrorMessage = "PriceNotANumber")]
        [DoubleGreaterThanValidation(0, ErrorMessage = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}
