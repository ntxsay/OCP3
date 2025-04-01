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
    #region Unit tests

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
    

    #endregion
    
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
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();

        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        var dbContext = new P3Referential(options, configuration);
        
        IProductRepository productRepository = new ProductRepository(dbContext);
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
        productRepository.SaveProduct(product);

        //Assert
        Assert.True(product.Id > 0);
    }

    /// <summary>
    /// Ce test vise à créer un produit et vérifie qu'il a bien été créé via son Id
    /// </summary>
    [Fact]
    public async Task CreateProductViewModelAsync()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

        var randomNumber = new Random().Next(1, 100);
        string productName = $"Produit {randomNumber}";

        var viewModel = new ProductViewModel()
        {
            Name = productName,
            Description = $"Description du produit {randomNumber}",
            Details = $"Details du produit {randomNumber}",
            Price = Convert.ToDouble(new Random().Next(1, 99) + 0.99).ToString(CultureInfo.CurrentCulture),
            Stock = new Random().Next(1, 200).ToString()
        };

        //Act
        productService.SaveProduct(viewModel);
        var count = await dbContext.Product.CountAsync(f => f.Name == productName);

        //Assert
        Assert.True(count > 0);
    }

    /// <summary>
    /// Ce test vise à récupérer les modèles de vue des produits et vérifie qu'il contient bien des produits
    /// </summary>
    [Fact]
    public void GetAllProductsViewModel()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

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
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

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
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

        //Act
        var productList = await productService.GetProduct();

        //Assert
        Assert.NotEmpty(productList);
        Assert.IsType<List<Product>>(productList);
    }

    /// <summary>
    /// Ce test vise à récupérer un modèle de vue d'un produit avec l'id spécifié
    /// </summary>
    [Theory]
    [InlineData(1)]
    public void GetProductViewModel(int id)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

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
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

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
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

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
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

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
    public async Task DeleteProduct(int productId)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
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
        IProductService productService =
            new ProductService(cart, productRepository, orderRepository, mockLocalizer.Object);

        //Act
        productService.DeleteProduct(productId);
        var countId = await dbContext.Product.CountAsync(f => f.Id == productId);

        //Assert
        Assert.Equal(0, countId);
    }

    #endregion
}