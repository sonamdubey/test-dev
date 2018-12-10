IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_CloseFollowUp_DropOffUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_CloseFollowUp_DropOffUser]
GO

	
CREATE PROCEDURE [dbo].[TC_Deals_CloseFollowUp_DropOffUser] @DealInquiryId INT
	,@Comment VARCHAR(500)
AS
--Author:Vinayak 11 jan 16
--set follow up data
--exec [dbo].[TC_Deals_CloseFollowUp_DropOffUser]
BEGIN
	UPDATE TC_Deals_DroppedOffUsersCalls
	SET Status=0
		,Comments = ISNULL(Comments, '') + '|' + @Comment
	WHERE DealInquiries_Id = @DealInquiryId
END

