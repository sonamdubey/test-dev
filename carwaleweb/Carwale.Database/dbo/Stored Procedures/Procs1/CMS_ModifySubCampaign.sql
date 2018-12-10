IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CMS_ModifySubCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CMS_ModifySubCampaign]
GO

	/*
AUTHOR:Khushaboo Patil
CREATED DATE:18/9/2013
DESCRIPTION:Insert and Update Subcampaign data
*/
CREATE PROCEDURE [dbo].[CMS_ModifySubCampaign]
@Id				BIGINT, 
@CampaignId		NUMERIC,
@CampaignType	INT,
@Name			VARCHAR(150),
@StartDate		DATETIME,
@EndDate		DATETIME,
@BookedQty		NUMERIC,
@Rate			DECIMAL(18,0),
@BookedAmt		DECIMAL(18,0),
@UpdatedOn		DATETIME,
@DeliveredQty   NUMERIC,
@NewId			BIGINT OUTPUT
AS	
BEGIN
	SET @NewId=-1
	IF @Id<>-1
		BEGIN
			UPDATE CMS_SubCampaigns SET CampaignId=@CampaignId,CampaignType=@CampaignType,
				Name=@Name,StartDate=@StartDate,EndDate=@EndDate,BookedQuantity=@BookedQty,DeliveredQuantity=@DeliveredQty,
				Rate=@Rate,BookedAmount=@BookedAmt,UpdatedOn=GETDATE()
			WHERE Id=@Id
			SET @NewId=@Id
		END
	ELSE
		BEGIN		
			INSERT INTO CMS_SubCampaigns (CampaignId,CampaignType,Name,StartDate,EndDate,BookedQuantity,Rate,BookedAmount,UpdatedOn,DeliveredQuantity)
			VALUES(@CampaignId,@CampaignType,@Name,@StartDate,@EndDate,@BookedQty,@Rate,@BookedAmt,@UpdatedOn,@DeliveredQty)
			SET @NewId=SCOPE_IDENTITY()	
		END	
END


