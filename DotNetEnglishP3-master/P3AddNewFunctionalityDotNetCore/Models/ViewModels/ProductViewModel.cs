﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [RegularExpression("^[0-9]+$", ErrorMessage = "StockNotAnInteger")]
        [Range(1, int.MaxValue, ErrorMessage = "StockNotGreaterThanZero")]
        public string Stock { get; set; }


        [Required(ErrorMessage = "ErrorMissingPrice")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}
