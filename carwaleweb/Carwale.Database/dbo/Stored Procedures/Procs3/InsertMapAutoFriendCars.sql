IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertMapAutoFriendCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertMapAutoFriendCars]
GO

	
-- PROCEDURE TO MAP AUTOFRIEND BARAND WITH CARWALE VERSIONID 

CREATE PROCEDURE [dbo].[InsertMapAutoFriendCars]
	@AutoFriendCar			VARCHAR(50),		-- Brand Name of the AutoFriend Cars
	@CarwaleVersionId		NUMERIC,		-- Provided VersionId of Carwale of AutoFriend 
	@Status			INTEGER  OUTPUT 	-- 1 if record successfully inserted and 
 AS
	
BEGIN
	
	SET @Status = 0

	SELECT CarwaleVersionId FROM AutoFriendCarMap WHERE  Lower(AutoFriendCar) = Lower(@AutoFriendCar)
	
	IF @@ROWCOUNT =  0
		BEGIN 
			INSERT INTO AutoFriendCarMap(AutoFriendCar, CarwaleVersionId)  VALUES (@AutoFriendCar, @CarwaleVersionId)
			SET @Status = 1
		END	
END