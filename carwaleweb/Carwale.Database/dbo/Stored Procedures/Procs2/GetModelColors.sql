IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelColors]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 11/07/14
-- Description:	Gets the Model colors based on ModelId passed 
-- Modified By : Shalini 
--				added distinct keyword
--removed distinct keyword
-- =============================================
CREATE PROCEDURE [dbo].[GetModelColors] -- Exec GetModelColors 2
	-- Add the parameters for the stored procedure here
	@ModelId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT Color
		,HexCode
	FROM ModelColors WITH (NOLOCK)
	WHERE CarModelID = @ModelId
	AND IsActive = 1
END


