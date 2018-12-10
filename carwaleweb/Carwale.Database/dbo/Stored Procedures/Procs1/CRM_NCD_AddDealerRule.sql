IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NCD_AddDealerRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NCD_AddDealerRule]
GO

	
-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 12th July 2011
-- Description:	This proc saves dealers rule in dealerrule table
-- =============================================
CREATE PROCEDURE [dbo].[CRM_NCD_AddDealerRule]
	@Id				NUMERIC,
	@DealerId		NUMERIC,
	@StateId		NUMERIC,
	@CityId			VARCHAR(200),
	@MakeId			NUMERIC,
	@ModelId		VARCHAR(MAX),
	@ZoneId			VARCHAR(500),
	@IsModelBased   BIT,
	@UpdatedBy		NUMERIC,
	@Status			BIT OUTPUT
AS
BEGIN
	SET @Status = 0
	
	IF @ZoneId IS NULL
		BEGIN
			SELECT DealerId FROM NCD_DealerRules 
			WHERE DealerId=@DealerId AND CityId=@CityId AND
			MakeId=@MakeId AND ModelId= @ModelId AND IsModelBased=@IsModelBased AND
			StateId=@StateId AND (ZoneId=@ZoneId OR ZoneId IS NULL) 
		END
	ELSE
		BEGIN
			SELECT DealerId FROM NCD_DealerRules 
			WHERE DealerId=@DealerId AND CityId=@CityId AND
			MakeId=@MakeId AND ModelId=@ModelId AND IsModelBased=@IsModelBased AND
			StateId=@StateId AND (ZoneId=@ZoneId) 
		END
	
	
	IF @@ROWCOUNT = 0
	BEGIN
		IF @Id = -1 
		BEGIN
			Insert Into NCD_DealerRules
			(
				DealerId,StateId,CityId,MakeId,ModelId,IsModelBased,CreatedOn,UpdatedOn,UpdatedBy,ZoneId
			)
			Values
			(
				@DealerId,@StateId,@CityId,@MakeId,@ModelId,@IsModelBased,GETDATE(),GETDATE(),@UpdatedBy,@ZoneId
			)
			SET @Status = 1 
		END
		ELSE
		BEGIN
			UPDATE NCD_DealerRules SET
					DealerId=@DealerId,CityId=@CityId,
					MakeId=@MakeId,ModelId=@ModelId,IsModelBased=@IsModelBased,
					StateId=@StateId,UpdatedBy=@UpdatedBy,UpdatedOn=GETDATE(),ZoneId=@ZoneId
			WHERE Id=@Id
			SET @Status = 1
		END	
	END
	ELSE
	BEGIN
		SET @Status = 0
	END
END

