IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FillMakeModelforUsedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FillMakeModelforUsedCars]
GO

	-- =============================================
-- Author:		Akansha Shrivastava
-- Create date: 2/10/2013 10:17:54 PM
-- Description:	To fill make model at used car default page
-- Modified By : Ashish G. Kamble on 12 Feb 2013
-- Modified : Select clause changed
-- =============================================
CREATE  PROCEDURE   [dbo].[FillMakeModelforUsedCars]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT MA.Name + '_' + Convert(Varchar,MA.ID) + '_' + MO.Name + '_' + Convert(Varchar,MO.ID) AS Value, MA.Name + ' ' + MO.Name as Text
	FROM CarModels MO
	INNER JOIN CarMakes MA WITH (NOLOCK) ON MA.ID = MO.CarMakeId
	INNER JOIN LiveListings LL WITH (NOLOCK) ON LL.ModelId = MO.ID
	WHERE MA.IsDeleted = 0 AND MO.IsDeleted = 0
	ORDER BY Text
END
