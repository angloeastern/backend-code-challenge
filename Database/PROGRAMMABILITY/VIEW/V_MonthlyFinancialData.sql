USE [AE_Challange]
GO

/****** Object:  View [dbo].[V_MonthlyFinancialData]    Script Date: 12/15/2025 6:04:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[V_MonthlyFinancialData]
AS
SELECT
    COALESCE(A.ShipCode, B.ShipCode) AS ShipCode,
    COALESCE(A.COA_ID, B.COA_ID) AS COA_ID,    
    FP.PeriodID,
    FP.FiscalYear,
    FP.PeriodMonth,
    FP.StartDate,
    FORMAT(FP.StartDate, 'MMMM yyyy') AS PeriodName,
    ISNULL(A.ActualAmount, 0.00) AS Actual,
    ISNULL(B.BudgetTarget, 0.00) AS Budget,
    (ISNULL(A.ActualAmount, 0.00) - ISNULL(B.BudgetTarget, 0.00)) AS Variance

FROM 
    [dbo].[ActualExpense] A
FULL OUTER JOIN 
    [dbo].[Budget] B
    ON A.ShipCode = B.ShipCode
    AND A.COA_ID = B.COA_ID
    AND A.PeriodID = B.PeriodID
INNER JOIN
    [dbo].[FiscalPeriod] FP
    ON FP.PeriodID = COALESCE(A.PeriodID, B.PeriodID)
WHERE 
    COALESCE(A.IsActive, 1) = 1 AND COALESCE(B.IsActive, 1) = 1;
GO

