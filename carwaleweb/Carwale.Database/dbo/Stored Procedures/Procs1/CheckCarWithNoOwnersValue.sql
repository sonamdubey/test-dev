IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckCarWithNoOwnersValue]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckCarWithNoOwnersValue]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 24-05-2013
-- Description:	Alert if any Car having value of owners in inquiry detail table and not updated on livelisting table
-- =============================================
CREATE PROCEDURE [dbo].[CheckCarWithNoOwnersValue]	
AS
BEGIN

	SELECT ProfileId [ProfileId whose owner value is not present] FROM 
	livelistings as LL WITH (NOLOCK)
	JOIN SellInquiriesDetails  as SID WITH (NOLOCK)
	ON SID.SellInquiryId=LL.Inquiryid
	AND LL.SellerType=1
	AND SID.Owners IS NOT NULL
	AND LL.Owners IS NULL
	UNION ALL
	SELECT ProfileId [ProfileId whose owner value is not present] FROM 
	livelistings as LL WITH (NOLOCK)
	JOIN CustomerSellInquiryDetails  as SID WITH (NOLOCK)
	ON SID.InquiryId=LL.Inquiryid
	AND LL.SellerType=2
	AND SID.Owners IS NOT NULL
	AND LL.Owners IS NULL

END
