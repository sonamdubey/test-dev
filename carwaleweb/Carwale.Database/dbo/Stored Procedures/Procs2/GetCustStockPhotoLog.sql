IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustStockPhotoLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustStockPhotoLog]
GO

	-- =============================================
-- Author:		Garule Prabhudas
-- Create date: 26th oct,2016
-- Description:	Get Individual Photo log (edit/create of photos for individual cars)
-- =============================================
CREATE PROCEDURE [dbo].[GetCustStockPhotoLog]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id,InquiryId,EntryDate from CustStockPhotoLog WITH(NOLOCK)
END

