IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetOfferTypes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetOfferTypes]
GO

	 -- =============================================
-- Author:                Suresh Prajapati
-- Create date: 03rd Nov 2014
-- Description:        To Get Offer Types as text, value pair to bind drop downs.
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetOfferTypes]
AS
BEGIN
       SET NOCOUNT ON;

       SELECT OC.Id AS Value
               ,OC.OfferType AS Text
       FROM BW_PQ_OfferCategories AS OC WITH (NOLOCK)
	   WHERE IsActive = 1
       ORDER BY OC.OfferType
END

