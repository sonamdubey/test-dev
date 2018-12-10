IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SaveCarSellers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SaveCarSellers]
GO

	

--Procedure Created By Sentil/Deepak On 12/11/2009
--This Procedure is used to for Insert and Update on table AE_CarSellers 

CREATE PROCEDURE [dbo].[AE_SaveCarSellers]
(
	@CarId AS NUMERIC(18,0) = NULL,
	@Name AS VARCHAR(100) = NULL,
	@Mobile AS VARCHAR(15) = NULL,
	@OtherContacts AS VARCHAR(50) = NULL,
	@CityId AS NUMERIC(18,0) = NULL,
	@Address AS VARCHAR(150) = NULL,
	@UpdatedOn AS DATETIME = NULL,
	@UpdatedBy AS BIGINT	
)
AS
BEGIN

	IF EXISTS(SELECT ID FROM AE_CarSellers WHERE CarId = @CarId )
		BEGIN
			UPDATE AE_CarSellers 
			SET 
				Name = @Name, Mobile = @Mobile, OtherContacts = @OtherContacts, CityId = @CityId, 
				Address = @Address, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
			WHERE CarId =  @CarId	
		END
	ELSE	
		BEGIN
			INSERT INTO 
			AE_CarSellers
			(
				CarId, Name, Mobile, OtherContacts, CityId, Address, CreatedOn, UpdatedBy			
			)
			VALUES
			(
				@CarId, @Name, @Mobile, @OtherContacts, @CityId, @Address, @UpdatedOn, @UpdatedBy			
			)			
				
		END
		
	
	
--SELECT * FROM AE_CarSellers
--TRUNCATE TABLE AE_CarSellers
	
END





