USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetCrewList]    Script Date: 12/15/2025 6:02:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetCrewList]
(
    @ShipCode NVARCHAR(10),
    @PageSize INT=10,
    @PageNumber INT=1,
    @SortColumn NVARCHAR(50)='CrewMemberId', 
    @SortDirection NVARCHAR(4)='ASC', 
    @SearchTerm NVARCHAR(100) = NULL 
)
AS
BEGIN
    SET NOCOUNT ON;
    IF @PageNumber IS NULL OR @PageNumber < 1
    BEGIN
        SET @PageNumber = 1;
    END    
    IF @PageSize IS NULL OR @PageSize <= 0
    BEGIN
        SET @PageSize = 10;
    END

    IF @SearchTerm = ''
    BEGIN
        SET @SearchTerm = NULL;
    END

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    WITH Filtered_CTE AS
    (
        SELECT
            v.* FROM 
            dbo.V_CrewSummaryPerTodaySearch v 
        WHERE
            v.ShipCode = @ShipCode 
            AND (@SearchTerm IS NULL OR v.SearchableText LIKE +'%'+ @SearchTerm + '%')
    )
    SELECT  
        RankName, CrewMemberId, FirstName, LastName, Age, Nationality,
        FormattedSignOnDate AS SignOnDate, 
        [Status] 
    FROM Filtered_CTE
    ORDER BY
        CASE WHEN @SortColumn = 'RankName' AND @SortDirection = 'ASC' THEN RankName END ASC,
        CASE WHEN @SortColumn = 'RankName' AND @SortDirection = 'DESC' THEN RankName END DESC,
        CASE WHEN @SortColumn = 'CrewMemberId' AND @SortDirection = 'ASC' THEN CrewMemberId END ASC,
        CASE WHEN @SortColumn = 'CrewMemberId' AND @SortDirection = 'DESC' THEN CrewMemberId END DESC,
        CASE WHEN @SortColumn = 'FirstName' AND @SortDirection = 'ASC' THEN FirstName END ASC,
        CASE WHEN @SortColumn = 'FirstName' AND @SortDirection = 'DESC' THEN FirstName END DESC,
        CASE WHEN @SortColumn = 'LastName' AND @SortDirection = 'ASC' THEN LastName END ASC,
        CASE WHEN @SortColumn = 'LastName' AND @SortDirection = 'DESC' THEN LastName END DESC,
        CASE WHEN @SortColumn = 'Age' AND @SortDirection = 'ASC' THEN Age END ASC,
        CASE WHEN @SortColumn = 'Age' AND @SortDirection = 'DESC' THEN Age END DESC,
        CASE WHEN @SortColumn = 'Nationality' AND @SortDirection = 'ASC' THEN Nationality END ASC,
        CASE WHEN @SortColumn = 'Nationality' AND @SortDirection = 'DESC' THEN Nationality END DESC,
        CASE WHEN @SortColumn = 'SignOnDate' AND @SortDirection = 'ASC' THEN SignOnDate END ASC, 
        CASE WHEN @SortColumn = 'SignOnDate' AND @SortDirection = 'DESC' THEN SignOnDate END DESC, 
        CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'ASC' THEN [Status] END ASC,
        CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'DESC' THEN [Status] END DESC,
        CrewMemberId ASC 

    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

END
GO

