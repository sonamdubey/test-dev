IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdatePQDealerInquiryDetails_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdatePQDealerInquiryDetails_V15]
GO

	

-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	To Update response after PQ dealer add inquiry details before calling to api
-- exec [dbo].[SavePQDealerInquiryDetails] 35,1,""
-- Modidfied by vinayak 6/11/2014, Calling SP to increase the counter for autobiz add
-- Modidfied by Vikas J 01/10/2015, avoid counter increase in case of email update
-- Modidfied by vinayak 10/14/2015, Removed campaign counter for autobiz 
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePQDealerInquiryDetails_V15.10.2] 
	-- Add the parameters for the stored procedure here
	@Id INT
	,@PushStatus INT
	,@CampaignId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE PQDealerAdLeads
	SET PushStatus =@PushStatus
	WHERE  Id = @Id
	
END
