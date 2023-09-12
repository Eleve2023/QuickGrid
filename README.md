# QuickGridCollection
Notre version de QuickGrid offre des fonctionnalités supplémentaires par rapport à la version originale. Elle accepte les ICollection et offre des options de filtre avancées. De plus, elle permet un tri multiple et peut être utilisée avec n’importe quel fournisseur qui accepte System.Linq.Expressions, tels que Linq, Entity Framework ou Simple.Odata.Client. Le filtrage et le tri sont effectués en externe, et la pagination est indépendante de la grille.

Our version of QuickGrid offers additional features compared to the original version. It accepts ICollection and offers advanced filter options. In addition, it allows multiple sorting and can be used with any provider that accepts System.Linq.Expressions, such as Linq, Entity Framework or Simple.Odata.Client. Filtering and sorting are done externally, and pagination is independent of the grid.
## Example of use
```csharp
<QuickGridC TGridItem="WeatherForecast" Items="@forecastsGrid" FilterSortChanged="FilterSortChanged">
    <PropertyColumnC Property="@(e => e.Date)" IsSortable="true" />
    <PropertyColumnC Property="@(e => e.TemperatureC)" Title="Temp. (C)" HasAdvancedFilterOptions="true" />
    <PropertyColumnC Property="@(e => e.TemperatureF)" Title="Temp. (F)" />
    <PropertyColumnC Property="@(e => e.Summary)" />
 </QuickGridC>
 <GridPaging PaginationState="gridPagingState"/>

private WeatherForecast[]? forecastsGrid;
    private IQueryable<WeatherForecast> forecasts = default!;
    private IQueryable<WeatherForecast> queryForecastsGrid = default!;
    private GridPagingState gridPagingState = new(5);

    protected override async Task OnInitializedAsync()
    {        
        forecasts = (await ForecastService.GetForecastAsync(DateOnly.FromDateTime(DateTime.Now))).AsQueryable();
        queryForecastsGrid = forecasts;
        gridPagingState.PageChanged += HandlePageChange;
        gridPagingState.CurrentPage = 1;
    }

    private void FilterSortChanged(GridQuery<WeatherForecast> filteringAndSorting)
    { 
        queryForecastsGrid = filteringAndSorting.ApplyQueryForLinq(forecasts, true);
        gridPagingState.CurrentPage = 1;
    }

    private void HandlePageChange(object? sender, GridPageChangedEventArgs e)
    {
        e.TotalItems = queryForecastsGrid.Count();
        
        var queryable = queryForecastsGrid;

        if (e.Skip > 0)
            queryable = queryable.Skip(e.Skip);

        queryable = queryable.Take(e.ItemsPerPage);

        forecastsGrid = queryable.ToArray();
        StateHasChanged();
    }
}
 ```
 
### [Here is the link to see other examples.](https://github.com/Eleve2023/QuickGrid/tree/master/src/SampleQuickGridCollection/SimpeQuickGrid)