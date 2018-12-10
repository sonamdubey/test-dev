IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetProductsForDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetProductsForDealer]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 23-03-2016
-- Description:	to get all the product in open/close/live status 
-- =============================================
CREATE PROCEDURE [dbo].[M_GetProductsForDealer] 
	@DealerId	INT =5
AS
BEGIN

	SET NOCOUNT ON;
    
	EXEC M_GetAllConvertedProducts @DealerId 
	EXEC M_OpenPitchingProduct @DealerId
	EXEC M_DealerDetail @DealerId ,NULL, NULL
	EXEC M_GetAllPendingApprovalProducts @DealerId
END



SET ANSI_NULLS ON
