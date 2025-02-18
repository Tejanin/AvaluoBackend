public class PaginatedResult<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; }
    public int Page { get; set; }
    public int RecordsPerPage { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }

    public PaginatedResult(IEnumerable<TEntity> items, int page, int recordsPerPage, int totalRecords)
    {
        Items = items;
        Page = page;
        RecordsPerPage = totalRecords == 0 ? 0 : recordsPerPage;
        TotalRecords = totalRecords;
        TotalPages = totalRecords == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / Math.Max(recordsPerPage, 1));
    }

    // Constructor genérico para convertir de PaginatedResult<T> a PaginatedResult<TDestino>
    public PaginatedResult<TDestino> Convert<TDestino>(Func<TEntity, TDestino> conversion)
    {
        return new PaginatedResult<TDestino>(
            Items.Select(conversion),
            Page,
            RecordsPerPage,
            TotalRecords
        );
    }
}
