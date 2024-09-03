// Xunit
global using Xunit;

// Moq para mocks
global using Moq;

// Microsoft Extensions para injeção de dependência e logging
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

// Namespaces específicos da aplicação
global using CryptoOrderBookProcessor.Application.Interfaces;
global using CryptoOrderBookProcessor.Application.Services;
global using CryptoOrderBookProcessor.Domain.Entities;
global using CryptoOrderBookProcessor.Infrastructure.Repositories;

// Outras bibliotecas comuns usadas nos testes
global using System.Threading;
global using System.Threading.Tasks;
