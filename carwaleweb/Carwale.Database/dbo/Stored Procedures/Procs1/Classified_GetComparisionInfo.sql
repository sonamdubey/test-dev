IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetComparisionInfo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetComparisionInfo]
GO

	-- =============================================
-- Author:		SATISH SHARMA
-- Create date: 22nd Dec 2011 5:57 PM
-- Description:	
-- THIS SP WILL RETURN BASIC INFORMATION OF SELECTED CARS FOR COMPARISION. 
-- BASIC INFORMATION WILL INCLUDE FOUR BASIC PARAMETERS LIKE CAR NAME, PRICE, KMS DONE, YEAR
-- THIS USED AT FLLOWING PAGES
--	- USED CARS SEARCH PAGE
--	- USED CARS PROFILE PAGE
--	Modified By : Ashish G. Kamble on 11 Feb 2013
-- Added : MakeName, ModelName, CityName
-- =============================================
CREATE  PROCEDURE   [dbo].[Classified_GetComparisionInfo]
	-- Add the parameters for the stored procedure here
	@ProfileId1	VARCHAR(15),
	@ProfileId2	VARCHAR(15),
	@ProfileId3	VARCHAR(15) = NULL,
	@ProfileId4	VARCHAR(15) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT LL.ProfileId, (LL.MakeName +' '+ LL.ModelName +' '+ LL.VersionName) Car, LL.Price, LL.Kilometers, Year(LL.MakeYear) [Year],
			LL.MakeName, LL.ModelName, C.Name AS CityName
	FROM LiveListings LL
	INNER JOIN Cities AS C ON C.ID = LL.CityId
	WHERE LL.ProfileId IN(@ProfileId1, @ProfileId2, @ProfileId3, @ProfileId4)
	
END
