IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiriesWorkSheetFormOnLoad_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiriesWorkSheetFormOnLoad_12Apr]
GO

	

-- Author:		Surendra
-- Create date: 12 Jan 2012
-- Description:	This procedure will be used to bind Controls in Search fields in Inquiries Form
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiriesWorkSheetFormOnLoad_12Apr]
AS
BEGIN	
	-- Give all Make as Table
	EXECUTE TC_GetCarMake	
		
	-- Give Inquiry type table
	EXECUTE TC_InquiryTypeSelect
	
	-- Give Inquiry followup actions table
	EXECUTE TC_InquiriesFollowupActionSelect	
	
	-- Give all Status table
	EXECUTE TC_InquiryStatusSelect
	
	-- Give all Source as Table
	EXECUTE TC_InquirySourceSelect
END


