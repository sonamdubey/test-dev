IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_SetFollowUp_DropOffUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_SetFollowUp_DropOffUser]
GO

	--------------------------------
--Author:Vinayak 11 jan 16
--set follow up data
--exec [dbo].[TC_Deals_SetFollowUp_DropOffUser]
--------------------------------
CREATE PROCEDURE [dbo].[TC_Deals_SetFollowUp_DropOffUser] @DealInquiryId INT
	,@Followup VARCHAR(70)
	,@Comment VARCHAR(500)
AS

BEGIN
	UPDATE TC_Deals_DroppedOffUsersCalls
	SET FollowUpTime = CONVERT(Datetime, @Followup, 120)
		,LastCallTime = GETDATE()
		,Comments = ISNULL(Comments, '') + '|' + @Comment
	WHERE DealInquiries_Id = @DealInquiryId
END

