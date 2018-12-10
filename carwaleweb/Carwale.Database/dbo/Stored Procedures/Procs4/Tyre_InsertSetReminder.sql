IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Tyre_InsertSetReminder]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Tyre_InsertSetReminder]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Tyre_InsertSetReminder] 
@ID NUMERIC,
@Name VARCHAR(100),
@Email VARCHAR(100),
@Mobile VARCHAR(20),
@TyreAge NUMERIC,
@TyreKms NUMERIC,
@FetchID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @ID = -1
		BEGIN
		INSERT INTO TyreReminder 
		(Name,Email,Mobile,TyreAge,TyreKms)
		VALUES 
		(@Name, @Email, @Mobile, @TyreAge, @TyreKms)
		SET @FetchID = SCOPE_IDENTITY() 
		END
	
END
