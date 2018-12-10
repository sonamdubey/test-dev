IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_SaveNewArea]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_SaveNewArea]
GO

	



-------------------------------------------------------------------------------------------

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <2/10/2014>
-- Description:	<Save Dealer OutletAddr>
-- =============================================
CREATE PROCEDURE [dbo].[DD_SaveNewArea]
	@PinCode	VARCHAR(10),	
	@CityId		INT = NULL,
	@Name		VARCHAR(50) = NULL,
	@Lattitude	float = NULL,
	@Longitude	float = NULL,
	@NewId		INT OUTPUT

AS
BEGIN
	IF NOT EXISTS(SELECT ID FROM Areas WHERE Name = @Name AND PinCode = @PinCode)
	BEGIN
		INSERT INTO Areas (Name,CityId,PinCode,Lattitude,Longitude,IsDeleted)
		VALUES(@Name , @CityId , @PinCode , @Lattitude , @Longitude ,0)
		SELECT ID , Name FROM Areas WHERE Name = @Name AND PinCode = @PinCode

		SET @NewId = SCOPE_IDENTITY()
	END
END

