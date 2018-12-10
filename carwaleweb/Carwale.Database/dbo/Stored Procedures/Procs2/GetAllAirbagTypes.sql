IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllAirbagTypes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllAirbagTypes]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 28/06/2016
-- Description:	To get all the types of airbags
-- =============================================
CREATE PROCEDURE [dbo].[GetAllAirbagTypes]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT UserDefinedId AS Value
		,NAME AS Text
	FROM [CD].[UserDefinedMaster] WITH (NOLOCK)
	WHERE CD.UserDefinedMaster.ItemMasterId = 155
		AND CD.UserDefinedMaster.IsActive = 1
END

