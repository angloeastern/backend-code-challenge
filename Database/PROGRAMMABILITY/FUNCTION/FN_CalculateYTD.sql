USE [AE_Challange]
GO

/****** Object:  UserDefinedFunction [dbo].[FN_CalculateYTD]    Script Date: 12/15/2025 6:03:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[FN_CalculateYTD]
(
    @ShipCode NVARCHAR(10),
    @TargetPeriodID INT
)
RETURNS TABLE
AS
RETURN
(
    WITH RangeConfig AS (
        SELECT 
            ShipCode=@ShipCode,FP.FiscalYear,TargetPeriodID= @TargetPeriodID,
            (
                SELECT MIN(PeriodID) 
                FROM [dbo].[FiscalPeriod] 
                WHERE FiscalYear = FP.FiscalYear
            ) AS StartPeriodID
        FROM 
            [dbo].[FiscalPeriod] FP
        WHERE 
            FP.PeriodID = @TargetPeriodID
    )
    SELECT
        MFD.ShipCode,
        MFD.COA_ID,
        SUM(MFD.Actual) AS YTD_Actual,
        SUM(MFD.Budget) AS YTD_Budget,
        SUM(MFD.Variance) AS YTD_Variance,
        FORMAT(CAST(CAST(RC.StartPeriodID AS VARCHAR(6)) + '01' AS DATE),'MMM yyyy','en-US') AS StartPeriodIDName,
        FORMAT(CAST(CAST(RC.TargetPeriodID AS VARCHAR(6)) + '01' AS DATE),'MMM yyyy','en-US') AS TargetPeriodIDName
    FROM
        [dbo].[V_MonthlyFinancialData] MFD 
    INNER JOIN 
        RangeConfig RC ON MFD.ShipCode = RC.ShipCode
    WHERE MFD.PeriodID >= RC.StartPeriodID 
        AND MFD.PeriodID <= @TargetPeriodID
    GROUP BY
        MFD.ShipCode,
        MFD.COA_ID,RC.StartPeriodID,RC.TargetPeriodID
);
GO

