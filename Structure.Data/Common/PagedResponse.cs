using System;
using System.Collections.Generic;
using System.Text;

namespace Structure.Data.Common;

public class PagedResponse<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public IEnumerable<T>? Data { get; set; }

    public List<string> Errors { get; set; } = new List<string>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalRecords { get; set; }

    public int TotalPages =>
        PageSize == 0
            ? 0
            : (int)Math.Ceiling((double)TotalRecords / PageSize);

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}