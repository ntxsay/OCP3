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
using Moq.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Services;

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


    #region Integration tests

    private readonly string _connectionString = Environment.OSVersion.Platform == PlatformID.Win32NT 
        ? "Server=localhost\\SQLEXPRESS;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true" 
        : "Server=192.168.1.14;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;User Id=sa;Password=azerty";
    
    /// <summary>
    /// Ce test vise à créer un produit et vérifie qu'il a bien été créé via son Id
    /// </summary>
    [Fact]
    public void CreateProduct()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;
        
        IProductRepository service = new ProductRepository(new P3Referential(options,configuration));
        var randomNumber = new Random().Next(1, 100);
        
        var product = new Product
        {
            Name = $"Produit {randomNumber}",
            Description = $"Description du produit {randomNumber}",
            Details = $"Details du produit {randomNumber}",
            Price = Convert.ToDouble(new Random().Next(1, 99) + 0.99),
            Quantity = new Random().Next(1, 200)
        };
        
        //Act
        service.SaveProduct(product);

        //Assert
        Assert.True(product.Id > 0);
    }
    
    /// <summary>
    /// Ce test vise à créer un produit et vérifie qu'il a bien été créé via son Id
    /// </summary>
    [Fact]
    public void CreateProductViewModel()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        var randomNumber = new Random().Next(1, 100);
        
        var product = new ProductViewModel()
        {
            Name = $"Produit {randomNumber}",
            Description = $"Description du produit {randomNumber}",
            Details = $"Details du produit {randomNumber}",
            Price = Convert.ToDouble(new Random().Next(1, 99) + 0.99).ToString(CultureInfo.CurrentCulture),
            Stock = new Random().Next(1, 200).ToString()
        };
        
        //Act
        productService.SaveProduct(product);

        //Assert
        
    }
    
    /// <summary>
    /// Ce test vise à récupérer les modèles de vue des produits et vérifie qu'il contient bien des produits
    /// </summary>
    [Fact]
    public void GetAllProductsViewModel()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        //Act
        var productList = productService.GetAllProductsViewModel();

        //Assert
        Assert.NotEmpty(productList);
        Assert.IsType<List<ProductViewModel>>(productList);
    }
    
    /// <summary>
    /// Ce test vise à récupérer les produits et vérifie qu'il contient bien des produits
    /// </summary>
    [Fact]
    public void GetAllProducts()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        //Act
        var productList = productService.GetAllProducts();

        //Assert
        Assert.NotEmpty(productList);
        Assert.IsType<List<Product>>(productList);
    }
    
    /// <summary>
    /// Ce test vise à récupérer les produits de manière asynchrone et vérifie qu'il contient bien des produits
    /// </summary>
    [Fact]
    public async Task GetAllProductsAsync()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        //Act
        var productList = await productService.GetProduct();

        //Assert
        Assert.NotEmpty(productList);
        Assert.IsType<List<Product>>(productList);
    }
    
    /// <summary>
    /// Ce test vise à récupérer un modèle de vue d'un produit avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(1)]
    public void GetProductViewModel(int id)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        //Act
        var product = productService.GetProductByIdViewModel(id);

        //Assert
        Assert.NotNull(product);
        Assert.IsType<ProductViewModel>(product);
    }
    
    /// <summary>
    /// Ce test vise à récupérer un produit avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(1)]
    public void GetProductById(int id)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        //Act
        var product = productService.GetProductById(id);

        //Assert
        Assert.NotNull(product);
        Assert.IsType<Product>(product);
    }
    
    /// <summary>
    /// Ce test vise à récupérer un produit de manière asynchrone avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(1)]
    public async Task GetProductByIdAsync(int id)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        //Act
        var product = await productService.GetProduct(id);

        //Assert
        Assert.NotNull(product);
        Assert.IsType<Product>(product);
    }
    
    /// <summary>
    /// Ce test vise à récupérer un modèle de vue d'un produit avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(1, 2)]
    public async Task UpdateProductQuantityAsync(int productId, int productQuantity)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        //Act
        var expectedStock = 0;
        var product = await productService.GetProduct(productId);
        if (product != null)
        {
            expectedStock = product.Quantity - productQuantity;
            cart.AddItem(product, productQuantity);
            productService.UpdateProductQuantities();
        }

        //Assert
        Assert.NotNull(product);
        Assert.Equal(expectedStock, product.Quantity);
    }
    
    /// <summary>
    /// Ce test vise à supprimer un produit avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(1)]
    public async Task DeleteProduc(int productId)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();
        
        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        ICart cart = new Cart();
        IProductRepository productRepository = new ProductRepository(dbContext);
        IOrderRepository orderRepository = new OrderRepository(dbContext);
        IProductService productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);
        
        //Act
        productService.DeleteProduct(productId);
        var product = (await productService.GetProduct()).FirstOrDefault(f => f.Id == productId);
        
        //Assert
        Assert.Null(product);
    }
    
    #endregion
}