IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdatePQDealerInquiryDetails_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdatePQDealerInquiryDetails_V14]
GO

	


-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	To Update response after PQ dealer add inquiry details before calling to api
-- exec [dbo].[SavePQDealerInquiryDetails] 35,1,""
-- Modidfied by vinayak 6/11/2014, Calling SP to increase the counter for autobiz add
-- Modidfied by Vikas J 01/10/2015, avoid counter increase in case of email update
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePQDealerInquiryDetails_V14.11.2.3] 
	-- Add the parameters for the stored procedure here
	@Id NUMERIC
	,@PushStatus INT
	,@CampaignId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @OldPushStatus BIGINT;

    -- Insert statements for procedure here
	SELECT @OldPushStatus =PushStatus FROM PQDealerAdLeads with (nolock) WHERE Id = @Id

	UPDATE PQDealerAdLeads
	SET PushStatus =@PushStatus
	WHERE  Id = @Id

	IF @OldPushStatus is null
	BEGIN
		EXEC [dbo].[UpdatePQDealerCounter] @Id,@PushStatus,@CampaignId --Calling SP to increase the counter for autobiz add
	END
END
