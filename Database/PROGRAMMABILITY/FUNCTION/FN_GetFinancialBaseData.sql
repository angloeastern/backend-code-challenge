USE [AE_Challange]
GO

/****** Object:  UserDefinedFunction [dbo].[FN_GetFinancialBaseData]    Script Date: 12/15/2025 6:04:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   FUNCTION [dbo].[FN_GetFinancialBaseData]
(
    @ShipCode NVARCHAR(10),
    @PeriodID INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        MFD.ShipCode,
        MFD.PeriodID,
        
        -- COA Detail (Child Account)
        C_Child.COA_ID,
        C_Child.AccountNumber AS ChildAccountNumber,
        C_Child.AccountDescription AS ChildAccountDescription,
        C_Child.ParentCOA_ID,
        C_Child.AccountType,

        -- COA Parent (Induk)
        C_Parent.AccountNumber AS ParentAccountNumber,
        C_Parent.AccountDescription AS ParentAccountDescription,
    
        -- Data Current Period (Diubah: Current_Actual -> CurrentActual)
        MFD.Actual AS CurrentActual,
        MFD.Budget AS CurrentBudget,
        MFD.Variance AS CurrentVariance,
        
        -- Data YTD (Diubah: YTD_Actual -> YTDActual)
        ISNULL(YTD.YTD_Actual, 0) AS YTDActual,
        ISNULL(YTD.YTD_Budget, 0) AS YTDBudget,
        ISNULL(YTD.YTD_Variance, 0) AS YTDVariance,
        YTD.StartPeriodIDName,YTD.TargetPeriodIDName
        
    FROM
        [dbo].[V_MonthlyFinancialData] MFD 
    JOIN 
        [dbo].[ChartOfAccount] C_Child
        ON MFD.COA_ID = C_Child.COA_ID
    LEFT JOIN
        [dbo].[ChartOfAccount] C_Parent
        ON C_Child.ParentCOA_ID = C_Parent.COA_ID
    LEFT JOIN
        [dbo].[FN_CalculateYTD](@ShipCode, @PeriodID) YTD 
        ON MFD.COA_ID = YTD.COA_ID
    
    WHERE
        MFD.ShipCode = @ShipCode
        AND MFD.PeriodID = @PeriodID
        AND (MFD.Actual <> 0 OR MFD.Budget <> 0 OR ISNULL(YTD.YTD_Actual, 0) <> 0 OR ISNULL(YTD.YTD_Budget, 0) <> 0)
);
GO

