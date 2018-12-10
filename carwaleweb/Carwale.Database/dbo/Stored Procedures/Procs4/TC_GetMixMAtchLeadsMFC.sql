IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetMixMAtchLeadsMFC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetMixMAtchLeadsMFC]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 06-02-2015
-- Description:	Get Mix Match Leads for MFC
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetMixMAtchLeadsMFC]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT TOP 15 ID AS Id,
	  CwCustomersId,
	  SellInquiryId AS SellInquiryId,
	  StockId AS StockId,
	  MFCDealerId AS BranchId,
	  CustomerName,
	  CustomerMobile,
	  CustomerEmail,
	  AreaNames AS CustomerLocation,
	  InquiryId
	FROM TC_MixMatchLeadsMFC WITH(NOLOCK)
	   
END
