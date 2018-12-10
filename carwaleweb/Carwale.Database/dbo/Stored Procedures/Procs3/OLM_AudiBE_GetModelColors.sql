IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GetModelColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GetModelColors]
GO

	
-- =============================================================================================
-- Author:		Ashish G. Kamble
-- Create date: 28 July 2013
-- Description:	SP to get the color details for a particular model
-- exec OLM_AudiBE_GetModelColors 133
-- =============================================================================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_GetModelColors]
	@TransactionId BIGINT
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT C.Id AS ModelColorId, T.Name AS ColorType, CLR.Name AS ColorName, CLR.HashCode AS ColorHashCode
	FROM OLM_AudiBE_Transactions TR
	LEFT JOIN OLM_AudiBE_ModelColors C ON C.ModelId = TR.ModelId AND C.IsActive = 1
	LEFT JOIN OLM_AudiBE_colorTypes T ON T.Id = C.ColorTypeId AND T.IsActive = 1
	LEFT JOIN OLM_AudiBE_colors  CLR ON CLR.Id = C.ColorId AND CLR.IsActive = 1
	WHERE TR.Id = @TransactionId AND C.ColorForId = 1
	
END


