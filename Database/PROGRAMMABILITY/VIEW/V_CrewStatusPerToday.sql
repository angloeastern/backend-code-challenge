USE [AE_Challange]
GO

/****** Object:  View [dbo].[V_CrewStatusPerToday]    Script Date: 12/15/2025 6:04:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[V_CrewStatusPerToday] AS
SELECT
    a.ServiceHistoryId,a.CrewMemberId,a.ShipCode,a.RankCode,
    a.SignOffDate,a.SignOnDate,a.EndOfContractDate
    ,CASE
        WHEN a.SignOffDate IS NOT NULL THEN 'Signed Off'
        WHEN a.SignOnDate > GETDATE() THEN 'Planned'
        WHEN a.SignOffDate IS NULL 
             AND GETDATE() > DATEADD(day, 30, a.EndOfContractDate) THEN 'Relief Due'             
        WHEN a.SignOnDate <= GETDATE() 
             AND a.SignOffDate IS NULL 
             AND GETDATE() <= a.EndOfContractDate THEN 'Onboard'
        ELSE 'Status Not Defined' 
    END AS [Status]
FROM
    CrewServiceHistory a;
GO

