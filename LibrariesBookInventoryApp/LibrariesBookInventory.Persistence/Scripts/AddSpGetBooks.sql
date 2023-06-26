CREATE PROCEDURE GetBooks
    @PageSize INT,
    @PageNumber INT,
    @SearchText NVARCHAR(100),
    @SortColumn NVARCHAR(50),
    @SortDirection NVARCHAR(4)
AS
BEGIN
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @SearchQuery NVARCHAR(MAX) = '%' + @SearchText + '%';

    SELECT *
FROM (
    SELECT ROW_NUMBER() OVER (
        ORDER BY
            CASE WHEN @SortDirection = 'asc' AND @SortColumn = 'Id' THEN CAST(B.Id AS bigint) END ASC,
            CASE WHEN @SortDirection = 'asc' AND @SortColumn = 'Title' THEN B.Title END ASC,
            CASE WHEN @SortDirection = 'asc' AND @SortColumn = 'Author' THEN B.Author END ASC,
            CASE WHEN @SortDirection = 'asc' AND @SortColumn = 'PublicationYear' THEN B.PublicationYear END ASC,
            CASE WHEN @SortDirection = 'asc' AND @SortColumn = 'ISBN' THEN B.ISBN END ASC,
            CASE WHEN @SortDirection = 'asc' AND @SortColumn = 'Quantity' THEN B.Quantity END ASC,
            CASE WHEN @SortDirection = 'desc' AND @SortColumn = 'Id' THEN CAST(B.Id AS bigint) END DESC,
            CASE WHEN @SortDirection = 'desc' AND @SortColumn = 'Title' THEN B.Title END DESC,
            CASE WHEN @SortDirection = 'desc' AND @SortColumn = 'Author' THEN B.Author END DESC,
            CASE WHEN @SortDirection = 'desc' AND @SortColumn = 'PublicationYear' THEN B.PublicationYear END DESC,
            CASE WHEN @SortDirection = 'desc' AND @SortColumn = 'ISBN' THEN B.ISBN END DESC,
            CASE WHEN @SortDirection = 'desc' AND @SortColumn = 'Quantity' THEN B.Quantity END DESC
    ) AS RowNum,
    B.*
    FROM Books AS B
    WHERE (B.Title LIKE @SearchQuery OR B.Author LIKE @SearchQuery OR B.ISBN LIKE @SearchQuery)
) AS Results
WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
ORDER BY RowNum;

END