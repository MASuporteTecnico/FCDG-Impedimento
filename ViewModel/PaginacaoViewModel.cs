namespace MaSistemas.ViewModel;

public class PaginacaoViewModel
{
  public dynamic Filtro { get; set; } = null!;
  public int page { get; set; }
  public int itemsPerPage { get; set; }
  public int pageCount { get; set; }
  public int itemsLength { get; set; }
  public List<SortByItem> sortBy { get; set; } = null!;
}

public class SortByItem
{
  public string key { get; set; } = "";
  public string order { get; set; } = "asc";
}