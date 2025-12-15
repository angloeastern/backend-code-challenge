USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetUsersList]    Script Date: 12/15/2025 6:03:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetUsersList]
    @PageSize INT = 10,
    @PageNumber INT = 1,
    @SortColumn NVARCHAR(50) = 'UserId',  
    @SortDirection NVARCHAR(4) = 'ASC',  
    @SearchTerm NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1
        SET @PageNumber = 1;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @SearchPattern NVARCHAR(102);

    IF @SearchTerm = ''
    BEGIN
        SET @SearchTerm = NULL;
    END

    ; WITH FilteredUsers AS
    (
        SELECT
            UserId,
            UserName,
            UserRole,
            CONCAT(
                CAST(UserId AS NVARCHAR(10)), ' ',
                UserName, ' ',
                UserRole
            ) AS SearchableText 
        FROM
            [dbo].[SystemUser] where IsActive=1
    )
    SELECT
        UserId,
        UserName,
        UserRole
    FROM
        FilteredUsers
    WHERE (@SearchTerm IS NULL OR SearchableText LIKE +'%'+ @SearchTerm + '%')
    ORDER BY
        CASE WHEN @SortColumn = 'UserId' AND @SortDirection = 'ASC' THEN UserId END ASC,
        CASE WHEN @SortColumn = 'UserId' AND @SortDirection = 'DESC' THEN UserId END DESC,
        CASE WHEN @SortColumn = 'UserName' AND @SortDirection = 'ASC' THEN UserName END ASC,
         CASE WHEN @SortColumn = 'UserName' AND @SortDirection = 'DESC' THEN UserName END DESC,
        CASE WHEN @SortColumn = 'UserRole' AND @SortDirection = 'ASC' THEN UserRole END ASC,
        CASE WHEN @SortColumn = 'UserRole' AND @SortDirection = 'DESC' THEN UserRole END DESC
    
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

