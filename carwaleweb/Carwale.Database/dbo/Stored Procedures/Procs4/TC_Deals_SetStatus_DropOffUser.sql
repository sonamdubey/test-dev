IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_SetStatus_DropOffUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_SetStatus_DropOffUser]
GO

	
CREATE PROCEDURE [dbo].[TC_Deals_SetStatus_DropOffUser] @DealInquiryId INT
AS
--Author:Vinayak 11 jan 16
--set follow up data
--exec [dbo].[TC_Deals_SetStatus_DropOffUser]
BEGIN
	UPDATE TC_Deals_DroppedOffUsersCalls
	SET Status=0
	WHERE DealInquiries_Id = @DealInquiryId
END

