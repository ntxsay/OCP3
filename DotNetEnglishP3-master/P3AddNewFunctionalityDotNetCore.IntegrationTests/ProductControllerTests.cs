using System.Net;
using System.Text;
using System.Text.Json;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.IntegrationTests.Helpers;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.IntegrationTests;

public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ProductControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });
            });
        }).CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    /// <summary>
    /// Teste permettant de récupérer les produits
    /// </summary>
    [Fact]
    public async Task GetProducts()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<P3Referential>();


        // Act
        var products = await dbContext.Product.ToArrayAsync();
        
        // Assert
        
        //Vérifie que la variable products n'est pas nulle
        Assert.NotNull(products);
        
        //Vérifie que la variable products contient au moins un élément
        Assert.NotEmpty(products);
    }
    
    /// <summary>
    /// Teste permettant de créer un produit
    /// </summary>
    [Fact]
    public async Task CreateProduct()
    {
        // Arrange
        using var createProductPageResponse = await _client.GetAsync("/Product/Create");
        var content = await HtmlHelpers.GetDocumentAsync(createProductPageResponse);
        var form = content.QuerySelector("form") as IHtmlFormElement;

        //Act
        HttpResponseMessage? response = null;
        if (form != null)
        {
            response = await _client.SendAsync(form,
                new List<KeyValuePair<string, string>>()
                {
                    new("Name", "Produit 1"),
                    new("Price", "10"),
                    new("Description", "Description du produit 1"),
                    new ("Stock", "10"),
                    new("Details", "Détails du produit 1"),
                });
        }
        
        // Assert
        
        //Vérifie que la variable form n'est pas nulle
        Assert.NotNull(form);
        
        //Vérifie que la variable response n'est pas nulle
        Assert.NotNull(response);
        
        //Vérifie que le code de statut de la réponse de la page de création de produit est égal à 200 (OK)
        Assert.Equal(HttpStatusCode.OK, createProductPageResponse.StatusCode);
        
        //Vérifie que le code de statut de la réponse de la sousmission du formulaire est égal à 302 (Redirection)
        Assert.Equal(HttpStatusCode.Redirect, response?.StatusCode);
        
        //Vérifie que l'url redirigé est égal à "/Product/Admin"
        Assert.Equal("/Product/Admin", response?.Headers.Location?.OriginalString);
        
        response?.Dispose();
    }
    
    /// <summary>
    /// Teste permettant de supprimer un produit
    /// </summary>
    [Fact]
    public async Task DeleteProduct()
    {
        // Arrange
        const int productIdToDelete = 10;
        using var createProductPageResponse = await _client.GetAsync("/Product/Admin");
        var content = await HtmlHelpers.GetDocumentAsync(createProductPageResponse);
        var form = content.QuerySelector($"form[id='{productIdToDelete}']") as IHtmlFormElement;

        //Act
        HttpResponseMessage? response = null;
        if (form != null)
        {
            response = await _client.SendAsync(form,
                new List<KeyValuePair<string, string>>()
                {
                    new("id", $"{productIdToDelete}")
                });
        }
        
        // Assert
        
        //Vérifie que la variable form n'est pas nulle
        Assert.NotNull(form);
        
        //Vérifie que la variable response n'est pas nulle
        Assert.NotNull(response);
        
        //Vérifie que le code de statut de la réponse de la page de création de produit est égal à 200 (OK)
        Assert.Equal(HttpStatusCode.OK, createProductPageResponse.StatusCode);
        
        //Vérifie que le code de statut de la réponse de la sousmission du formulaire est égal à 302 (Redirection)
        Assert.Equal(HttpStatusCode.Redirect, response?.StatusCode);
        
        //Vérifie que l'url redirigé est égal à "/Product/Admin"
        Assert.Equal("/Product/Admin", response?.Headers.Location?.OriginalString);
        
        response?.Dispose();
    }
}