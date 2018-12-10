IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_SaveCrossSell]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_SaveCrossSell]
GO

	CREATE PROCEDURE [dbo].[PQ_SaveCrossSell]
	@CampaignId		INT,
	@UpdatedBy      INT,
	@CrossSell	[dbo].[PQ_CrossSellCar] READONLY,
	@Status			BIT OUTPUT
AS
BEGIN
	DECLARE		@CrossSellId        INT,  
	            @NumberOfCars		INT = 0,
				@i					TINYINT,
				@TempCrossSellCar	INT,
				@TempTargetCar		INT,
				@TempStateId	    INT,
				@TempCityId	        INT,
				@TempZoneId	        INT,
				@TempTemplateId		INT,
				@TempCampaignId		INT


INSERT INTO PQ_CrossSellCampaign(IsActive,UpdatedOn,AddedOn,UpdatedBy,CampaignId)  ---Updated By Sourav Roy,Added Addedon and Updated By
		VALUES(1,GETDATE(),GETDATE(),@UpdatedBy,@CampaignId)

		SET @CrossSellId=SCOPE_IDENTITY()

				

	SELECT @NumberOfCars = COUNT(*) FROM @CrossSell
	SET @i = 1

	WHILE @NumberOfCars > 0
	BEGIN

		SELECT @TempTargetCar=TargetVersion, @TempCrossSellCar=CrossSellVersion,@TempStateId=StateId, @TempCityId=CityId,@TempZoneId=ZoneId,@TempTemplateId=TemplateId FROM @CrossSell WHERE Id = @i

		INSERT INTO PQ_CrossSellCampaignRules(CrossSellCampaignId,CityId,TargetVersion,CrossSellVersion,ZoneId,TemplateId,StateId,UpdatedOn,AddedOn,UpdatedBy)    ---Updated By Sourav Roy,Added Addedon and Updated By
		VALUES(@CrossSellId,@TempCityId,@TempTargetCar,@TempCrossSellCar,@TempZoneId,@TempTemplateId,@TempStateId,GETDate(),GETDate(),@UpdatedBy)

		SET @i = @i + 1
		SET @NumberOfCars = @NumberOfCars - 1
	END 
	SET @Status = 1 
END


/****** Object:  StoredProcedure [dbo].[UpdateCrossSellTemplate]    Script Date: 04/01/2016 18:54:15 ******/
SET ANSI_NULLS ON
