IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDiscontinuedModelReplacement]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDiscontinuedModelReplacement]
GO

	-- =============================================
-- Author:		Vikas
-- Create date: 26-11-2012
-- Description:	To get the model with has replaced a discontinued model (if any)
-- Modified By: Akansha on 10.4.2014
-- Description : Added Masking Name Column
-- =============================================
CREATE PROCEDURE [dbo].[GetDiscontinuedModelReplacement] 
	-- Add the parameters for the stored procedure here
	@ModelId INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ReplacementID INT
	SET @ReplacementID = (SELECT ISNULL(ReplacedByModelId, 0) From CarModels Where ID = @ModelId)
    -- Insert statements for procedure here
	IF @ReplacementID != 0 
		BEGIN 
			SELECT 
				@ReplacementID As ReplacedByModelId, Cma.Name As ReplacementMakeName, Cmo.Name As ReplacementModelName,Cmo.MaskingName as ReplacementMaskingName
			FROM 
				CarModels Cmo 
				INNER JOIN CarMakes Cma On Cma.ID = Cmo.CarMakeId
			WHERE 
				Cmo.ID = @ReplacementID
		END
END
