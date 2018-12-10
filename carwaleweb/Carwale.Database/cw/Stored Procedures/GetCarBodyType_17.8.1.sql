IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarBodyType_17]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarBodyType_17]
GO

	

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Chetan Thambad
-- Create date: 25 july 2015
-- Description:	This Sp Returns body type of car EXEC [CW].[GetCarBodyType_17.8.1]
--MOdified By Rakesh Yadav on 15 Oct 2015, Added filter IsBodyStyleActive=1
-- =============================================
CREATE PROCEDURE [cw].[GetCarBodyType_17.8.1]	
AS
BEGIN
	SELECT BS.ID AS Id, BS.Name, BS.HostURL, BS.ImageUrl 
	FROM CarBodyStyles BS WITH(NOLOCK)  WHERE IsBodyStyleActive = 1
END