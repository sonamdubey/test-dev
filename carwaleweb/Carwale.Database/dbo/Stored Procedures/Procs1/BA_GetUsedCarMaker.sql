IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetUsedCarMaker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetUsedCarMaker]
GO

	-- =============================================
-- Author:		Ranjeet
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetUsedCarMaker] 
	
AS
BEGIN
	SET NOCOUNT ON;

	select CM.ID, CM.Name, CM.LogoUrl, CM.HostURL from CarMakes AS CM WITH (NOLOCK) where CM.Used = 1 AND  CM.IsDeleted =0
END
