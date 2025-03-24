using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests;

public class ProductServiceTests
{
    /// <summary>
    /// Take this test method as a template to write your test method.
    /// A test method must check if a definite method does its job:
    /// returns an expected value from a particular set of parameters
    /// </summary>
    [Fact]
    public void ExampleMethod()
    {
        // Arrange

        // Act


        // Assert
        Assert.Equal(1, 1);
    }
        
    [Fact]
    public void CreateProduct()
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
            
        //S'assure que la valeur du stock du produit est un nombre décimal ou entier
        Assert.True(double.TryParse(product.Stock, out var stockValue));
            
        //S'assure que le stock est supérieur à 0
        Assert.True(stockValue > 0);
            
        //S'assure que la valeur du prix du produit est un nombre décimal ou entier
        Assert.True(double.TryParse(product.Stock, out var priceValue));
            
        //S'assure que le prix est supérieur à 0
        Assert.True(priceValue > 0);
    }
        
    // TODO write test methods to ensure a correct coverage of all possibilities
}