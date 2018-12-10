IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ICB_AddCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ICB_AddCity]
GO

	
CREATE PROCEDURE [dbo].[ICB_AddCity]
(
	@CityId AS NUMERIC(18,0),
	@IsActive AS Bit,
	@CreatedOn AS DATETIME,
	@CreatedBy	AS NUMERIC(18,0),
	@Status AS BIT OUTPUT
)
AS
BEGIN 

	IF NOT EXISTS(SELECT CityId FROM ICB_Cities WHERE CityId = @CityId )
		BEGIN
			INSERT INTO ICB_Cities ( CityId, IsActive, CreatedOn, CreatedBy)
							 VALUES( @CityId, @IsActive, @CreatedOn, @CreatedBy)
							 
			   SET @Status = 1
		END
	ELSE
		BEGIN
			   SET @Status = 0  
		END
 
END
