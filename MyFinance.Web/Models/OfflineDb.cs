using IndexedDB.Blazor;
using Microsoft.JSInterop;
using MyFinance.Web.DTOs;

public class MyFinanceLocalDb : IndexedDb
{
    public MyFinanceLocalDb(IJSRuntime jSRuntime)
        : base(jSRuntime, name: "MyFinanceLocalDb", version: 2) { } // Versão 2!

    public IndexedSet<LancamentoDto> LancamentosPendentes { get; set; }
    public IndexedSet<CategoriaDto> CacheCategorias { get; set; }
    public IndexedSet<DashboardDto> CacheDashboard { get; set; }
}