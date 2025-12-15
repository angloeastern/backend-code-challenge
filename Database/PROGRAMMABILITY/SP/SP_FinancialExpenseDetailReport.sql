USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_FinancialExpenseDetailReport]    Script Date: 12/15/2025 6:01:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[SP_FinancialExpenseDetailReport]
    @ShipCode NVARCHAR(10),
    @PeriodID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Base.ParentAccountNumber AS ParentAccountNumber,
        Base.ParentAccountDescription AS ParentAccountDescription,
        
        Base.ChildAccountNumber AS AccountNumber,
        Base.ChildAccountDescription AS AccountDescription,
        Base.AccountType,
        Base.CurrentActual,
        Base.CurrentBudget,
        Base.CurrentVariance,
        Base.YTDActual,
        Base.YTDBudget,
        Base.YTDVariance,
        Base.StartPeriodIDName,
        Base.TargetPeriodIDName as CurrentPeriodIDName,
        CASE WHEN Base.CurrentBudget > 0 THEN (Base.CurrentVariance / Base.CurrentBudget) ELSE 0 END AS CurrentVariancePercent,
        CASE WHEN Base.YTDBudget > 0 THEN (Base.YTDVariance / Base.YTDBudget) ELSE 0 END AS YTDVariancePercent

    FROM
        [dbo].[FN_GetFinancialBaseData](@ShipCode, @PeriodID) Base
    
    ORDER BY
        Base.ParentAccountNumber,
        Base.ChildAccountNumber;
END
GO

