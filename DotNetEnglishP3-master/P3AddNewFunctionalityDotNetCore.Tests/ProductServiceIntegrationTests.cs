using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests;

public class ProductServiceIntegrationTests : IDisposable
{
    private readonly string _connectionString = Environment.OSVersion.Platform == PlatformID.Win32NT
        ? "Server=localhost\\SQLEXPRESS;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true"
        : "Server=192.168.1.14;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;User Id=sa;Password=azerty";

    private const int TestProductId = 1;
    private readonly IConfiguration _configuration;
    private readonly DbContextOptions<P3Referential> _options;
    private readonly P3Referential _dbContext;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly Mock<IStringLocalizer<ProductService>> _mockLocalizer;
    private readonly ICart _cart;
    private readonly IProductService _productService;

    public ProductServiceIntegrationTests()
    {
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new KeyValuePair<string, string>[]
            {
                new("ConnectionStrings:P3Referential", _connectionString)
            })
            .Build();

        _options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(_configuration.GetConnectionString("P3Referential"))
            .Options;

        _dbContext = new P3Referential(_options, _configuration);
        _productRepository = new ProductRepository(_dbContext);
        _orderRepository = new OrderRepository(_dbContext);
        _mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        _cart = new Cart();
        _productService = new ProductService(_cart, _productRepository, _orderRepository, _mockLocalizer.Object);
    }
    
    /// <summary>
    /// Ce test vise à créer un produit et vérifie qu'il a bien été créé via son Id
    /// </summary>
    [Fact]
    public void CreateProduct()
    {
        // Arrange
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
        _productRepository.SaveProduct(product);

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
        var randomNumber = new Random().Next(1, 100);
        var productName = $"Produit {randomNumber}";
        var viewModel = new ProductViewModel()
        {
            Name = productName,
            Description = $"Description du produit {randomNumber}",
            Details = $"Details du produit {randomNumber}",
            Price = Convert.ToDouble(new Random().Next(1, 99) + 0.99).ToString(CultureInfo.CurrentCulture),
            Stock = new Random().Next(1, 200).ToString()
        };

        //Act
        _productService.SaveProduct(viewModel);
        var count = await _dbContext.Product.CountAsync(f => f.Name == productName);

        //Assert
        Assert.True(count > 0);
    }

    /// <summary>
    /// Ce test vise à récupérer les modèles de vue des produits et vérifie qu'il contient bien des produits
    /// </summary>
    [Fact]
    public void GetAllProductsViewModel()
    {
        //Act
        var productList = _productService.GetAllProductsViewModel();

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
        //Act
        var productList = _productService.GetAllProducts();

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
        //Act
        var productList = await _productService.GetProduct();

        //Assert
        Assert.NotEmpty(productList);
        Assert.IsType<List<Product>>(productList);
    }

    /// <summary>
    /// Ce test vise à récupérer un modèle de vue d'un produit avec l'id spécifié
    /// </summary>
    [Theory]
    [InlineData(TestProductId)]
    public void GetProductViewModel(int id)
    {
        //Act
        var product = _productService.GetProductByIdViewModel(id);

        //Assert
        Assert.NotNull(product);
        Assert.IsType<ProductViewModel>(product);
    }

    /// <summary>
    /// Ce test vise à récupérer un produit avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(TestProductId)]
    public void GetProductById(int id)
    {
        //Act
        var product = _productService.GetProductById(id);

        //Assert
        Assert.NotNull(product);
        Assert.IsType<Product>(product);
    }

    /// <summary>
    /// Ce test vise à récupérer un produit de manière asynchrone avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(TestProductId)]
    public async Task GetProductByIdAsync(int id)
    {
        //Act
        var product = await _productService.GetProduct(id);

        //Assert
        Assert.NotNull(product);
        Assert.IsType<Product>(product);
    }

    /// <summary>
    /// Ce test vise à récupérer un modèle de vue d'un produit avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(TestProductId, 2)]
    public async Task UpdateProductQuantityAsync(int productId, int productQuantity)
    {
        //Act
        var expectedStock = 0;
        var product = await _productService.GetProduct(productId);
        if (product != null)
        {
            expectedStock = product.Quantity - productQuantity;
            _cart.AddItem(product, productQuantity);
            _productService.UpdateProductQuantities();
        }

        //Assert
        Assert.NotNull(product);
        Assert.Equal(expectedStock, product.Quantity);
    }

    /// <summary>
    /// Ce test vise à supprimer un produit avec l'id spécifié et vérifie qu'il contient bien des produits
    /// </summary>
    [Theory]
    [InlineData(TestProductId)]
    public async Task DeleteProduct(int productId)
    {
        //Act
        _productService.DeleteProduct(productId);
        var countId = await _dbContext.Product.CountAsync(f => f.Id == productId);

        //Assert
        Assert.Equal(0, countId);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}