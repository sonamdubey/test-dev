IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FeatureActivation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FeatureActivation]
GO

	
-- =============================================
-- Author:		<Author : Vinay Kumar Prajapati>
-- Create date: <28/02/2014>
-- Description:	<This SP save log information about dealer feature from table TC_MappingDealerFeatures .>
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_FeatureActivation]
(
 @DealerId          INT,
 @UserId            INT,
 @HasOffer          BIT,
 @HasYouTube        BIT,
 @HasListPremium    BIT
)
AS
BEGIN
	SELECT MDF.ID FROM TC_MappingDealerFeatures AS MDF WITH(NOLOCK) WHERE MDF.BranchId=@DealerId AND MDF.TC_DealerFeatureId = 1  --1 for Deal
	IF @@ROWCOUNT = 0 
		BEGIN
			IF  @HasOffer = 1
			BEGIN
				INSERT INTO TC_MappingDealerFeatures(BranchId,TC_DealerFeatureId) VALUES(@DealerId,1)
                INSERT INTO TC_DealerFeatureLog(DealerId,TC_DealerFeatureId,ActionDate,ActionTakenBy,Action) VALUES(@DealerId,1,GETDATE(),@UserId,'Inserted') 
		    END 
		END
	ELSE
		BEGIN
			IF  @HasOffer = 0
			BEGIN
				DELETE FROM TC_MappingDealerFeatures  WHERE  BranchId=@DealerId AND TC_DealerFeatureId=1
				INSERT INTO TC_DealerFeatureLog(DealerId,TC_DealerFeatureId,ActionDate,ActionTakenBy,Action) VALUES(@DealerId,1,GETDATE(),@UserId,'Deleted') 
			END 
		END
		
	SELECT MDF.ID FROM TC_MappingDealerFeatures AS MDF WITH(NOLOCK) WHERE MDF.BranchId=@DealerId AND MDF.TC_DealerFeatureId = 2  --2 for YouTube Video
	IF @@ROWCOUNT = 0 
		BEGIN
			IF  @HasYouTube = 1
			BEGIN
				INSERT INTO TC_MappingDealerFeatures(BranchId,TC_DealerFeatureId) VALUES(@DealerId,2)
                INSERT INTO TC_DealerFeatureLog(DealerId,TC_DealerFeatureId,ActionDate,ActionTakenBy,Action) VALUES(@DealerId,2,GETDATE(),@UserId,'Inserted') 
			END
		END
	ELSE
		BEGIN
				IF  @HasYouTube = 0
				BEGIN
					DELETE FROM TC_MappingDealerFeatures  WHERE  BranchId=@DealerId AND TC_DealerFeatureId=2
					INSERT INTO TC_DealerFeatureLog(DealerId,TC_DealerFeatureId,ActionDate,ActionTakenBy,Action) VALUES(@DealerId,2,GETDATE(),@UserId,'Deleted')
			    END 
		END

	SELECT  MDF.ID FROM TC_MappingDealerFeatures AS MDF WITH(NOLOCK) WHERE MDF.BranchId=@DealerId AND MDF.TC_DealerFeatureId = 3  --3 for Premium list
			
	IF @@ROWCOUNT = 0 
		BEGIN
			IF  @HasListPremium = 1
			 BEGIN
				INSERT INTO TC_MappingDealerFeatures(BranchId,TC_DealerFeatureId) VALUES(@DealerId,3)
			    INSERT INTO TC_DealerFeatureLog(DealerId,TC_DealerFeatureId,ActionDate,ActionTakenBy,Action) VALUES(@DealerId,3,GETDATE(),@UserId,'Inserted') 
		     END 
		END
	ELSE
		BEGIN
				IF   @HasListPremium  = 0
				BEGIN
					  DELETE FROM TC_MappingDealerFeatures  WHERE  BranchId=@DealerId AND TC_DealerFeatureId=3
					  INSERT INTO TC_DealerFeatureLog(DealerId,TC_DealerFeatureId,ActionDate,ActionTakenBy,Action) VALUES(@DealerId,3,GETDATE(),@UserId,'Deleted') 
			    END
		END
 
END









