IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerPrice_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerPrice_Save]
GO

	-- =============================================    
-- Author:  Vikas J
-- Create date:17/05/2013 
-- Description: To save prices of car versions updated by dealer on Trading car for his website
-- =============================================    
CREATE PROCEDURE [dbo].[Microsite_DealerPrice_Save]     
(    
 @DealerId INT,    
 @CityId INT,  
 @VersionId INT,
 @Price INT,
 @RTO INT,
 @Insurance INT,
 @CRTMCharges INT,
 @DriveAssure INT,
 @WithOctroi BIT 
)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 SET NOCOUNT ON;
	DECLARE @COUNT INT=0;
	SELECT @COUNT=COUNT(*) FROM  DealerWebsite_ExShowRoomPrices WHERE DealerId=@DealerId AND CarVersionId=@VersionId AND CityId=@CityId;   
	IF(@COUNT=0)  
		BEGIN    
			INSERT INTO DealerWebsite_ExShowRoomPrices(DealerId,CityId,CarVersionId,ExShowroomPrice,RTO,Insurance,CRTMCharges,DriveAssure,withOctroi,EntryDate)   
			VALUES(@DealerId,@CityId,@VersionId,@Price,@RTO,@Insurance,@CRTMCharges,@DriveAssure,@WithOctroi,GETDATE())  
		END    
	ELSE    
		BEGIN    
			UPDATE DealerWebsite_ExShowRoomPrices 
			SET ExShowroomPrice=@Price,RTO=@RTO,Insurance=@Insurance,
				CRTMCharges=@CRTMCharges,DriveAssure=@DriveAssure,withOctroi=@WithOctroi,EntryDate=GETDATE()
			WHERE DealerId=@DealerId AND CarVersionId=@VersionId AND CityId=@CityId   
	END     
END
