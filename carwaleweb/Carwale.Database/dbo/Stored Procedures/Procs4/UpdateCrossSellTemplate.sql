IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCrossSellTemplate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCrossSellTemplate]
GO

	CREATE PROCEDURE [dbo].[UpdateCrossSellTemplate]
	@TemplateId		INT,
	@CrossSellVersionId      INT,
	@TargetVersionId	INT,
	@StateId		INT,
	@CityId            INT,
	@ZoneId         INT,
	@UpdatedBy      INT
AS
BEGIN
	UPDATE PQ_CrossSellCampaignRules
	SET TemplateId=@TemplateId,UpdatedBy=@UpdatedBy,UpdatedOn=GetDate() --Added updatedBy
	WHERE CrossSellVersion=@CrossSellVersionId
	AND   TargetVersion=@TargetVersionId
	AND   StateId=@StateId
	AND   CityId=@CityId
	AND   ZoneId=@ZoneId
END


/****** Object:  StoredProcedure [dbo].[GetNewCarDealerCityByMakeState]    Script Date: 04/01/2016 19:00:05 ******/

-----------------

