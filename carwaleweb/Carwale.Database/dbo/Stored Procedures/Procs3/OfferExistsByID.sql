IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OfferExistsByID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OfferExistsByID]
GO

	-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 27-11-2014
-- Description:	Input ModelId or VersionId ,Output 1 / 0 
-- =============================================
CREATE PROCEDURE [dbo].[OfferExistsByID] --[dbo].[OfferExistsByID] 457
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT ModelId FROM ModelOffers
END

