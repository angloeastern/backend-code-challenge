USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_CreateShip]    Script Date: 12/15/2025 6:01:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_CreateShip]
    @ShipCode NVARCHAR(50),    
    @ShipName NVARCHAR(150),     
    @FiscalYear INT,            
    @Status NVARCHAR(50),    
    
    @CreatedBy NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewShipCode NVARCHAR(50);

    BEGIN TRANSACTION;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM [dbo].[Ship] WHERE ShipCode = @ShipCode)
        BEGIN
            THROW 50000, N'Ship Code already exists (Duplicate key violation).', 1;
        END
        INSERT INTO [dbo].[Ship] (
            ShipCode, 
            ShipName, 
            FiscalYear, 
            Status, 
            IsActive, 
            CreatedDate, 
            CreatedBy
        )
        VALUES (
            @ShipCode, 
            @ShipName, 
            @FiscalYear, 
            @Status, 
            1, 
            GETDATE(), 
            @CreatedBy
        );
        
        SET @NewShipCode = @ShipCode;
        
        COMMIT TRANSACTION;
        
        SELECT 
            ShipCode, 
            ShipName, 
            FiscalYear, 
            Status
        FROM 
            [dbo].[Ship]
        WHERE 
            ShipCode = @NewShipCode;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;            
        THROW;
    END CATCH
END
GO

