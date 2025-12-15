USE [AE_Challange]
GO

/****** Object:  StoredProcedure [dbo].[SP_AuthenticateUser]    Script Date: 12/15/2025 6:01:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_AuthenticateUser] 
    @Username NVARCHAR(50),
    @Password NVARCHAR(MAX)
AS

DECLARE @InputHash VARBINARY(32) = HASHBYTES('SHA2_256', @Password);
BEGIN
    SET NOCOUNT ON;
    SELECT 
        u.UserId, 
        u.Username,
        u.UserRole
    FROM 
        SystemUser u
    WHERE 
        u.Username = @Username 
        AND u.PasswordHash = @InputHash       
END
GO

