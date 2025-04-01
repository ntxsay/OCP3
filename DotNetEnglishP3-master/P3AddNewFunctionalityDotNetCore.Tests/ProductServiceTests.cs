using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Services;

namespace P3AddNewFunctionalityDotNetCore.Tests;

public class ProductServiceTests
{
    #region Check Product Name

    /// <summary>
    /// L'objectif de ce test est de vérifier que le nom du produit n'est pas null et ne contient pas d'espaces blancs
    /// </summary>
    [Fact]
    public void CheckProductNameIsNotNullOrWhitespaceFree()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var isProductNameNullOrWhiteSpace = string.IsNullOrWhiteSpace(product.Name);

        // Assert

        //S'assure que le nom du produit n'est pas null ou qu'il ne contient pas d'espaces blancs
        Assert.False(isProductNameNullOrWhiteSpace);
    }

    /// <summary>
    /// L'objectif de ce test est de vérifier que le nom du produit n'est pas une chaîne vide
    /// </summary>
    [Fact]
    public void CheckProductNameIsNotNullOrEmpty()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var isProductNameNullOrEmpty = string.IsNullOrEmpty(product.Name);

        // Assert

        //S'assure que le nom du produit n'est pas null ou vide
        Assert.False(isProductNameNullOrEmpty);
    }

    /// <summary>
    /// L'objectif de ce test est de vérifier que le nom du produit correspond bien à celui qui a été défini
    /// </summary>
    [Fact]
    public void CheckProductNameEquals()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var productName = product.Name;

        // Assert

        //S'assure que le nom du produit est bien celle définie
        Assert.Equal("Produit 1", productName);
    }

    #endregion

    #region Check Product Stock

    /// <summary>
    /// L'objectif de ce test est de vérifier que la valeur du stock de type string est convertible en double
    /// </summary>
    [Fact]
    public void CheckProductStockDoubleConversion()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var isConvertibleToDouble = double.TryParse(product.Stock, out _);

        // Assert

        //S'assure que la valeur du stock du produit est un nombre à virgule
        Assert.True(isConvertibleToDouble);
    }

    /// <summary>
    /// L'objectif de ce test est de vérifier que le stock du produit est supérieur à 0
    /// </summary>
    [Fact]
    public void CheckProductStockGreaterThanZero()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var stockValue = double.TryParse(product.Stock, out var result)
            ? result
            : 0;

        // Assert

        //S'assure que le stock est supérieur à 0
        Assert.True(stockValue > 0);
    }

    /// <summary>
    /// L'objectif de ce test est de vérifier que le stock est égal à celui défini
    /// </summary>
    [Fact]
    public void CheckProductStockEquals()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var stockValue = double.TryParse(product.Stock, out var result)
            ? result
            : 0;

        // Assert

        //S'assure que le stock est égal à la valeur définie
        Assert.Equal(20, stockValue);
    }

    #endregion

    #region Check Product Price

    /// <summary>
    /// L'objectif de ce test est de vérifier que le prix du produit est convertible en type double
    /// </summary>
    [Fact]
    public void CheckProductPriceDoubleConversion()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var isConvertibleToDouble = double.TryParse(product.Price, out _);


        // Assert

        //S'assure que la valeur du prix du produit est un nombre décimal ou entier
        Assert.True(isConvertibleToDouble);
    }

    /// <summary>
    /// L'objectif de ce test est de vérifier que le prix du produit est supérieur à 0
    /// </summary>
    [Fact]
    public void CheckProductPriceGreaterThanZero()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var priceValue = double.TryParse(product.Price, out var result)
            ? result
            : 0;

        // Assert

        //S'assure que le prix est supérieur à 0
        Assert.True(priceValue > 0);
    }

    /// <summary>
    /// L'objectif de ce test est de vérifier que le prix est égal à celui défini
    /// </summary>
    [Fact]
    public void CheckProductPriceEquals()
    {
        // Arrange 
        var product = new ProductViewModel()
        {
            Name = "Produit 1",
            Description = "Description du Produit 1",
            Details = "Détails du Produit 1",
            Price = "1,20",
            Stock = "20"
        };

        // Act
        var priceValue = double.TryParse(product.Price, out var result)
            ? result
            : 0;

        // Assert

        //S'assure que le prix est égal à la valeur définie
        Assert.Equal(1.20, priceValue);
    }

    #endregion
}