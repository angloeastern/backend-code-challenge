USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_AssignShipToUser]    Script Date: 12/15/2025 6:00:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_AssignShipToUser]
    @UserId INT,
    @ShipCode NVARCHAR(50),
    @CreatedBy NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemUser] WHERE UserId = @UserId)
        BEGIN
            THROW 50000, N'Error: User ID not found.', 1;
        END
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Ship] WHERE ShipCode = @ShipCode AND IsActive = 1)
        BEGIN
            THROW 50000, N'Error: Ship Code not found or Ship is inactive.', 1;
        END
        IF EXISTS (SELECT 1 FROM [dbo].[UserShipAssignment] WHERE UserId = @UserId AND ShipCode = @ShipCode)
        BEGIN
            THROW 50000, N'Error: Assignment record for this ShipCode and UserId already exists.', 1;
        END
        
        INSERT INTO [dbo].[UserShipAssignment] (
            UserId, 
            ShipCode, 
            IsActive,      
            CreatedDate,   
            CreatedBy      
        )
        VALUES (
            @UserId, 
            @ShipCode, 
            1, -- Default: Active
            GETDATE(), 
            @CreatedBy
        );
        
        COMMIT TRANSACTION;
        
        SELECT
            T1.UserId,
            T2.UserName,
            T3.ShipCode,
            T3.ShipName,  
            T3.FiscalYear,
            T3.[Status],
            T3.CreatedBy as AssignedByUsername,
            T3.CreatedDate as AssignedDate

        FROM 
            [dbo].[UserShipAssignment] T1
        JOIN 
            [dbo].[SystemUser] T2 ON T1.UserId = T2.UserId
        JOIN 
            [dbo].[Ship] T3 ON T1.ShipCode = T3.ShipCode
        WHERE 
            T1.UserId = @UserId AND T1.ShipCode = @ShipCode;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        THROW;
    END CATCH
END
GO

