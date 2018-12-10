IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddPackageDeal]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddPackageDeal]
GO

	


--THIS PROCEDURE INSERTS THE VALUES FOR THE PackageDeal

CREATE PROCEDURE [dbo].[NCS_AddPackageDeal]
	@DealerId			NUMERIC,
	@VersionId			NUMERIC,
	@InsPremium			DECIMAL(10,2),
	@TotalDiscount		DECIMAL(10,2),
	@Comments			VARCHAR(200),
	@LastUpdated		DATETIME,
	@Status				BIT OUTPUT
 AS
	
BEGIN
	SET @Status = 0

	UPDATE NCS_PackageDeal 
	SET TotalDiscount = @TotalDiscount, InsPremium = @InsPremium, 
		Comments = @Comments, LastUpdated = @LastUpdated
	WHERE DealerId = @DealerId AND VersionId = @VersionId

	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO NCS_PackageDeal
			( DealerId, VersionId, InsPremium, TotalDiscount, Comments, LastUpdated )			
			Values
			( @DealerId, @VersionId, @InsPremium, @TotalDiscount, @Comments, @LastUpdated )	

			SET @Status = 1
				
		END
	ELSE
		SET @Status = 1
			
END


