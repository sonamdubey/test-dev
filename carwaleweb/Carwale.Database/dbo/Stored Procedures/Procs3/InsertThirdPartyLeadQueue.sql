IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertThirdPartyLeadQueue]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertThirdPartyLeadQueue]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <26/09/2012>
-- Description:	<Inserts a record into ThirdPartyLeadQueue if the customer makes a PQ for different models or for the same model after 30 days
--                 Will work for cases where there will be only 1 campaign per brand>
-- modified by vikas j on 24 july 2014--check city id in configuration city table for ford
-- modified by vikas j on 11 Feb 2015--added condition for PushStatus
-- =============================================
CREATE PROCEDURE [dbo].[InsertThirdPartyLeadQueue]
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC(18,0),
	@ModelName VARCHAR(30),
	@MakeId NUMERIC(18,0),
	@ModelId NUMERIC(18,0),
	@CityId NUMERIC(18,0),
	@City VARCHAR(30) = Null,--check city and pass null if not found by vikas j
	@CustomerName VARCHAR(100),
	@Email VARCHAR(100),
	@Mobile VARCHAR(20),
	@OUTCity VARCHAR(30)OUTPUT,
	@ThirdPartyLeadId NUMERIC(18,0) OUTPUT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

    -- Insert statements for procedure here
	DECLARE @CampaignId NUMERIC
	SELECT @CampaignId = ThirdPartyLeadSettingId 
	FROM ThirdPartyLeadSettings 
	WHERE ((MakeId = @MakeId AND ModelId = @ModelId)OR(MakeId = @MakeId AND ModelId IS NULL))  
		AND IsActive = 1 AND (CONVERT(DATE,GETDATE()) BETWEEN CONVERT(DATE,CampaignStartDate) AND CONVERT(DATE,CampaignEndDate) ) AND (LeadVolume-LeadsSent)>0
	
	IF(@CampaignId IS NOT NULL AND (@CityId IN (SELECT CityId FROM Ford_PQLeadCitites) OR @MakeId != 5))--check city id in configuration city table for ford by Vikas J
	BEGIN
		DECLARE @ID SMALLINT
		SELECT @ID = ThirdPartyLeadId 
		FROM ThirdPartyLeadQueue TPLQ
		WHERE TPLQ.ModelId = @ModelId
			  AND DATEDIFF(DAY,TPLQ.EntryDate,GETDATE())<=30
			  AND TPLQ.Mobile=@Mobile AND TPLQ.PushStatus != '-1'-- added pushstatus = -1 by vikas j
	  
      IF(@ID IS NULL)
			BEGIN		
			    IF (@City = '' OR @City IS NULL)
			    SELECT @City=Name FROM Cities WHERE ID=@CityId
			  
				INSERT INTO ThirdPartyLeadQueue(PQId,ModelName,MakeId,ModelId,TPLeadSettingId,CityId,City,CustomerName,Email,Mobile,PushStatus)
				VALUES(@PQId,@ModelName,@MakeId,@ModelId,@CampaignId,@CityId,@City,@CustomerName,@Email,@Mobile,-1)
				
				SET @ThirdPartyLeadId = SCOPE_IDENTITY()
			END
       
       ELSE SET @ThirdPartyLeadId = -1
    END
    
    ELSE SET @ThirdPartyLeadId = -1
	--ELSE 
	--INSERT INTO ThirdPartyLeadQueueLog(PQId,ModelName,MakeId,ModelId,CityId,City,CustomerName,Email,Mobile,PushStatus)
	--VALUES(@PQId,@ModelName,@MakeId,@ModelId,@CityId,@City,@CustomerName,@Email,@Mobile,@PushStatus)
	SET @OUTCity=@City
END

