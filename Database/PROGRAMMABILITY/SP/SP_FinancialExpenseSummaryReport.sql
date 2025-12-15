USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_FinancialExpenseSummaryReport]    Script Date: 12/15/2025 6:02:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[SP_FinancialExpenseSummaryReport]
    @ShipCode NVARCHAR(10),
    @PeriodID INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH DetailData AS (
        SELECT 
            Base.COA_ID,
            Base.ParentCOA_ID,
            COALESCE(Base.ParentAccountNumber, Base.ChildAccountNumber) AS SummaryAccountNumber,
            COALESCE(Base.ParentAccountDescription, Base.ChildAccountDescription) AS SummaryAccountDescription,
            
            Base.CurrentActual,
            Base.CurrentBudget,
            Base.CurrentVariance,
            Base.YTDActual,
            Base.YTDBudget,
            Base.YTDVariance,
            Base.StartPeriodIDName,
            Base.TargetPeriodIDName
        FROM [dbo].[FN_GetFinancialBaseData](@ShipCode, @PeriodID) Base
    ),
    SummaryAggregated AS (
        SELECT
            DD.SummaryAccountNumber,
            DD.SummaryAccountDescription,
            DD.StartPeriodIDName,
            DD.TargetPeriodIDName,
            SUM(DD.CurrentActual) AS CurrentActual,
            SUM(DD.CurrentBudget) AS CurrentBudget,
            SUM(DD.CurrentVariance) AS CurrentVariance,
            SUM(DD.YTDActual) AS YTDActual,
            SUM(DD.YTDBudget) AS YTDBudget,
            SUM(DD.YTDVariance) AS YTDVariance
        FROM
            DetailData DD
        GROUP BY 
            DD.SummaryAccountNumber,
            DD.SummaryAccountDescription,
            DD.StartPeriodIDName,
            DD.TargetPeriodIDName
    )

    SELECT
        SummaryAccountNumber AS AccountNumber,
        SummaryAccountDescription AS AccountDescription,
        CurrentActual,
        CurrentBudget,
        CurrentVariance,
        YTDActual,
        YTDBudget,
        YTDVariance,
        StartPeriodIDName,
        TargetPeriodIDName  as CurrentPeriodIDName,
        CASE WHEN CurrentBudget > 0 THEN (CurrentVariance / CurrentBudget) ELSE 0 END AS CurrentVariancePercent,
        CASE WHEN YTDBudget > 0 THEN (YTDVariance / YTDBudget) ELSE 0 END AS YTDVariancePercent
    FROM
        SummaryAggregated
    ORDER BY
        AccountNumber;
END
GO

