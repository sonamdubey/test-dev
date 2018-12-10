IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryTypeSelect_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryTypeSelect_12Apr]
GO

	


-- Author:		Surendra
-- Create date: 12 Jan 2012
-- Description:	This procedure is used to get Inquiry Type like Buyer ,seller
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiryTypeSelect_12Apr]
AS
BEGIN	
	SELECT TC_InquiryTypeId,InquiryType FROM TC_InquiryType  WITH(NOLOCK)  WHERE IsActive=1
END


