IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_SetFollowUp_DropOffUser_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_SetFollowUp_DropOffUser_V16]
GO

	--------------------------------
--Author:Sourav 18 feb 16
--set follow up data
--exec [dbo].[TC_Deals_SetFollowUp_DropOffUser_V16.2.6.1]
--------------------------------
CREATE PROCEDURE [dbo].[TC_Deals_SetFollowUp_DropOffUser_V16.2.6.1] @DealInquiryId INT
	,@Followup VARCHAR(70)
	,@Comment VARCHAR(500)
	,@DispositionId INT
AS

BEGIN
	UPDATE TC_Deals_DroppedOffUsersCalls
	SET FollowUpTime = CONVERT(Datetime, @Followup, 120)
		,LastCallTime = GETDATE()
		,Comments = ISNULL(Comments, '') + '|' + @Comment
		,DispositionId=@DispositionId
	WHERE DealInquiries_Id = @DealInquiryId
END


