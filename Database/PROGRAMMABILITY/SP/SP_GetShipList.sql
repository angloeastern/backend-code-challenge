USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetShipList]    Script Date: 12/15/2025 6:02:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetShipList]
    @Status nvarchar(50) = NULL,
    @PageSize INT = 10,
    @PageNumber INT = 1,
    @SortColumn NVARCHAR(50) = 'ShipCode',  
    @SortDirection NVARCHAR(4) = 'ASC',  
    @SearchTerm NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1
        SET @PageNumber = 1;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    IF @SearchTerm = ''
    BEGIN
        SET @SearchTerm = NULL;
    END
    
    IF @Status = ''
    BEGIN
        SET @Status = NULL;
    END

    ; WITH FilteredShips AS
    (
        SELECT
            ShipCode,
            ShipName,
            FiscalYear,
            Status,
            CONCAT(
                ShipCode, ' ',
                ShipName, ' ',
                CAST(FiscalYear AS NVARCHAR(4)), ' ',
                Status
            ) AS SearchableText 
        FROM
            [dbo].[Ship]
        WHERE IsActive=1 and
            (@Status IS NULL OR Status = @Status)
    )
    
    SELECT
        ShipCode,
        ShipName,
        FiscalYear,
        Status
    FROM
        FilteredShips
    WHERE 
        (@SearchTerm IS NULL OR SearchableText LIKE +'%'+ @SearchTerm + '%')
    ORDER BY
        CASE WHEN @SortColumn = 'ShipCode' AND @SortDirection = 'ASC' THEN ShipCode END ASC,
        CASE WHEN @SortColumn = 'ShipName' AND @SortDirection = 'ASC' THEN ShipName END ASC,
        CASE WHEN @SortColumn = 'FiscalYear' AND @SortDirection = 'ASC' THEN FiscalYear END ASC,
        CASE WHEN @SortColumn = 'ShipCode' AND @SortDirection = 'DESC' THEN ShipCode END DESC,
        CASE WHEN @SortColumn = 'ShipName' AND @SortDirection = 'DESC' THEN ShipName END DESC,
        CASE WHEN @SortColumn = 'FiscalYear' AND @SortDirection = 'DESC' THEN FiscalYear END DESC
        
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

