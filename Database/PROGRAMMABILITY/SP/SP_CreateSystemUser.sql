USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_CreateSystemUser]    Script Date: 12/15/2025 6:01:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_CreateSystemUser]
    @UserName NVARCHAR(100),
    @Password NVARCHAR(50), 
    @UserRole NVARCHAR(50),
    @CreatedBy NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @UserId INT; 

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @PasswordHash VARBINARY(128);
        SET @PasswordHash = HASHBYTES('SHA2_256', @Password); 
        IF EXISTS (SELECT 1 FROM [dbo].[SystemUser] WHERE UserName = @UserName)
        BEGIN
            THROW 50000, N'Username sudah ada (Duplicate key violation).', 1;
        END
        INSERT INTO [dbo].[SystemUser] (
            UserName, PasswordHash, UserRole, IsActive, CreatedDate, CreatedBy
        )
        VALUES (
            @UserName, @PasswordHash, @UserRole, 1, GETDATE(), @CreatedBy
        );

        SET @UserId = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
        
        SELECT 
            UserId, 
            UserName, 
            UserRole
        FROM 
            [dbo].[SystemUser]
        WHERE 
            UserId = @UserId;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        THROW;
    END CATCH
END
GO

