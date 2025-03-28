using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests;

public class ProductServiceTests
{
    /// <summary>
    /// L'objectif de ce test est de vérifier que le nom du produit est valide
    /// </summary>
    [Fact]
    public void CheckProductName()
    {
        // Arrange 
            
        // Act
        var product = new ProductViewModel()
        {
            Name = "Product 1",
            Description = "Description of the product 1",
            Details = "Details of the product 1",
            Price = "1,20",
            Stock = "20"
        };

        // Assert
            
        //S'assure que le nom du produit n'est pas null
        Assert.NotNull(product.Name);
            
        //S'assure que le nom du produit n'est pas vide
        Assert.NotEmpty(product.Name);
        
        //S'assure que le nom du produit est bien celle définie
        Assert.Equal("Product 1", product.Name);
    }
    
    /// <summary>
    /// L'objectif de ce test est de vérifier que le stock du produit est valide
    /// </summary>
    [Fact]
    public void CheckProductStock()
    {
        // Arrange 
            
        // Act
        var product = new ProductViewModel()
        {
            Name = "Product 1",
            Description = "Description of the product 1",
            Details = "Details of the product 1",
            Price = "1,20",
            Stock = "20"
        };

        // Assert
        
        //S'assure que la valeur du stock du produit est un nombre décimal ou entier
        Assert.True(double.TryParse(product.Stock, out var stockValue));
            
        //S'assure que le stock est supérieur à 0
        Assert.True(stockValue > 0);
        
        //S'assure que la valeur du stock est bien celle définie
        Assert.Equal(20, stockValue);
    }
    
    /// <summary>
    /// L'objectif de ce test est de vérifier que le prix du produit est valide
    /// </summary>
    [Fact]
    public void CheckProductPrice()
    {
        // Arrange 
            
        // Act
        var product = new ProductViewModel()
        {
            Name = "Product 1",
            Description = "Description of the product 1",
            Details = "Details of the product 1",
            Price = "1,20",
            Stock = "20"
        };

        // Assert
            
        //S'assure que la valeur du prix du produit est un nombre décimal ou entier
        Assert.True(double.TryParse(product.Price, out var priceValue));
            
        //S'assure que le prix est supérieur à 0
        Assert.True(priceValue > 0);
        
        //S'assure que la valeur du prix est bien celle définie
        Assert.Equal(1.20, priceValue);
    }
}