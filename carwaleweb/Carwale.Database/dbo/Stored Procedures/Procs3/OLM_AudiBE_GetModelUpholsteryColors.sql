IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GetModelUpholsteryColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GetModelUpholsteryColors]
GO

	
-- =============================================
-- Author:		Supriya K
-- Create date: 10/8/2013
-- Description:	SP to get the upholstery color details for a particular model
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_GetModelUpholsteryColors] 
	@TransactionId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT C.Id AS ModelColorId, T.Name AS ColorType, CLR.Name AS ColorName, CLR.HashCode AS ColorHashCode
	FROM OLM_AudiBE_Transactions TR
	LEFT JOIN OLM_AudiBE_ModelColors C ON C.ModelId = TR.ModelId AND C.IsActive = 1
	LEFT JOIN OLM_AudiBE_colorTypes T ON T.Id = C.ColorTypeId AND T.IsActive = 1
	LEFT JOIN OLM_AudiBE_colors  CLR ON CLR.Id = C.ColorId AND CLR.IsActive = 1
	WHERE TR.Id = @TransactionId AND C.ColorForId = 3
END

