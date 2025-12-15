USE [AE_Challange]
GO

/****** Object:  View [dbo].[V_CrewSummaryPerTodaySearch]    Script Date: 12/15/2025 6:04:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[V_CrewSummaryPerTodaySearch] AS
SELECT
    a.*, 
    b.RankName,
    c.FirstName,
    c.LastName,
    c.Nationality,
    DATEDIFF(year, c.BirthDate, GETDATE()) AS Age,
    FORMAT(a.SignOnDate, 'dd MMM yyyy') AS FormattedSignOnDate,
    CONCAT(
        b.RankName, ' ', 
        a.CrewMemberId, ' ', 
        c.FirstName, ' ', 
        c.LastName, ' ', 
        c.Nationality, ' ', 
        FORMAT(a.SignOnDate, 'dd MMM yyyy')
    ) AS SearchableText 
FROM
    V_CrewStatusPerToday a 
INNER JOIN [Rank] b ON a.RankCode = b.RankCode
INNER JOIN CrewMember c ON a.CrewMemberId = c.CrewMemberId
WHERE 
    a.[Status] <> 'Signed Off'; 
GO

