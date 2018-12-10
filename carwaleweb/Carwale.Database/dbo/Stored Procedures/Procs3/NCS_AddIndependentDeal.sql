IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddIndependentDeal]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddIndependentDeal]
GO

	


--THIS PROCEDURE INSERTS THE VALUES FOR THE IndependentDeal

CREATE PROCEDURE [dbo].[NCS_AddIndependentDeal]
	@DealerId			NUMERIC,
	@VersionId			NUMERIC,
	@Discount			DECIMAL(10,2),
	@Comments			VARCHAR(200),
	@LastUpdated		DATETIME,
	@Status				BIT OUTPUT
 AS
	
BEGIN
	SET @Status = 0

	UPDATE NCS_IndependentDeal 
	SET Discount = @Discount, Comments = @Comments, LastUpdated = @LastUpdated
	WHERE DealerId = @DealerId AND VersionId = @VersionId
	
	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO NCS_IndependentDeal
			( DealerId, VersionId, Discount, Comments, LastUpdated )			
			Values
			( @DealerId, @VersionId, @Discount, @Comments, @LastUpdated )	

			SET @Status = 1
		END	
	ELSE
		SET @Status = 1	
END


