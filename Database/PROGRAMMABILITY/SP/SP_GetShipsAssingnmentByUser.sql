USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetShipsAssingnmentByUser]    Script Date: 12/15/2025 6:02:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetShipsAssingnmentByUser]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemUser] WHERE UserId = @UserId)
        BEGIN
            THROW 50000, N'Error: User ID not found.', 1;
        END
        SELECT
            a.UserId,
            S2.UserName,
            S.ShipCode,
            S.ShipName,
            S.FiscalYear,
            S.Status,
            A.CreatedDate AS AssignedDate,
            A.CreatedBy AS AssignedByUsername 
        FROM
            [dbo].[Ship] S
        JOIN
            [dbo].[UserShipAssignment] A ON S.ShipCode = A.ShipCode
         JOIN 
            [dbo].[SystemUser] S2 ON A.UserId = S2.UserId
        WHERE
            A.UserId = @UserId          
            AND S.IsActive = 1         
            AND A.IsActive = 1       
        ORDER BY
            S.ShipName ASC;
            
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

