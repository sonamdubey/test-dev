IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SMBE_SaveVariantDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SMBE_SaveVariantDetails]
GO

	-- =============================================
-- Author:	Mihir Chheda
-- Create date: 19-Dec-2013
-- Description:	Saves variant details 
-- =============================================
CREATE PROCEDURE OLM_SMBE_SaveVariantDetails @Id NUMERIC OUTPUT
	,@Color VARCHAR(50)
	,@ColorCode VARCHAR(15)
	,@Price VARCHAR(50)
	,@VersionId INT
	,@VariantCode VARCHAR(20)
AS
BEGIN
	UPDATE OLM_BookingData
	SET Color = @Color
		,ColorCode = @ColorCode
		,Price = @Price
		,VersionId = @VersionId
		,VariantCode = @VariantCode
	WHERE Id = @Id
END
