IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerModelBodyStyles]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerModelBodyStyles]
GO

	
-- =============================================
-- Author:		Kritika Choudhary
-- Create date: 12 June 2015
-- Description: Returns all the active dealer Car Model Body styles
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerModelBodyStyles] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Select BodyStyleName AS Text, ID AS Value 
	From Microsite_ModelBodyStyles
	WHERE IsActive=1
END


