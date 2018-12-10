IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_SaveContactPerson]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_SaveContactPerson]
GO

	
-------------------------------------------------------------------------------------------

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <5/11/2014>
-- Description:	<Save Dealer Contact Person>
-- =============================================
CREATE PROCEDURE [dbo].[DD_SaveContactPerson]
@PersonId			INT,
@DD_OutletId	INT,
@Salutation			VARCHAR(10),
@FirstName			VARCHAR(50),
@LastName			VARCHAR(50),
@DD_DesignationsId	INT,
@EmailId			VARCHAR(50),
@CreatedBy			INT,
@NewId				INT OUTPUT
AS
BEGIN
	IF(@PersonId <> -1)
	BEGIN
		UPDATE DD_ContactPerson SET Salutation =@Salutation , FirstName =@FirstName , LastName =@LastName ,DD_DesignationsId =@DD_DesignationsId , EmailId = @EmailId
		WHERE Id = @PersonId
		SET @NewId = @PersonId
	END
	ELSE
	--IF NOT EXISTS(SELECT ID FROM DD_ContactPerson WHERE EmailId = @EmailId)--DD_DealerNamesId = @DD_DealerNamesId AND 
	BEGIN
		INSERT INTO DD_ContactPerson (DD_OutletId , Salutation , FirstName , LastName ,DD_DesignationsId , EmailId , CreatedBy , CreatedOn)
		VALUES (@DD_OutletId , @Salutation , @FirstName , @LastName , @DD_DesignationsId , @EmailId , @CreatedBy , GETDATE())

		SET @NewId = SCOPE_IDENTITY()
	END

END

