using IndexedDB.Blazor;
using Microsoft.JSInterop;
using MyFinance.Web.DTOs;

public class MyFinanceLocalDb : IndexedDb
{
    public MyFinanceLocalDb(IJSRuntime jSRuntime)
        : base(jSRuntime, name: "MyFinanceLocalDb", version: 1) { }

    // FILA DE ESCRITA (O que vai subir para a API)
    public IndexedSet<LancamentoDto> LancamentosPendentes { get; set; }

    // CACHE DE LEITURA (O que veio da API para exibição offline)
    public IndexedSet<CategoriaDto> CacheCategorias { get; set; }
    public IndexedSet<DashboardDto> CacheDashboard { get; set; }
}