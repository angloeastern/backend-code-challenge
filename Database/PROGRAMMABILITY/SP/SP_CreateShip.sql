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
        -- 1. Validasi Business Logic (Cek ShipCode Duplikat)
        IF EXISTS (SELECT 1 FROM [dbo].[Ship] WHERE ShipCode = @ShipCode)
        BEGIN
            THROW 50000, N'Kode Kapal (ShipCode) sudah ada (Duplicate key violation).', 1;
        END

        -- 2. Insert ke Tabel Ship
        INSERT INTO [dbo].[Ship] (
            ShipCode, 
            ShipName, 
            FiscalYear, 
            Status, 
            IsActive, -- Kolom tetap di-insert
            CreatedDate, 
            CreatedBy
        )
        VALUES (
            @ShipCode, 
            @ShipName, 
            @FiscalYear, 
            @Status, 
            1, -- Nilai default 1 (Aktif) dimasukkan langsung
            GETDATE(), 
            @CreatedBy
        );
        
        -- Ambil ShipCode yang baru dibuat (untuk return value)
        SET @NewShipCode = @ShipCode;
        
        -- 3. Commit Transaksi
        COMMIT TRANSACTION;
        
        -- 4. RETURN VALUE SEBAGAI RESULT SET
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
            
        -- Meneruskan error
        THROW;
    END CATCH
END
GO

