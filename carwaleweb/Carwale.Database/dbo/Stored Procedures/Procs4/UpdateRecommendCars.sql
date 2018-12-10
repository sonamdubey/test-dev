IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateRecommendCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateRecommendCars]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 04/07/2013
-- Description:	Update fields GearBox,ChildSafety,Powerwindows,ABS,CentralLocking,AirBags,Displacement in RecommendCars for a particular version
-- Modified by Reshma 09-10-2013 The itemmasterid of ABS has been changed from 55 to 260
-- =============================================
CREATE PROCEDURE [dbo].[UpdateRecommendCars]
	-- Add the parameters for the stored procedure here
	@CarVersionId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE RecommendCars
    SET GearBox=tab.GearBox
       ,ChildSafety=tab.ChildSafety
       ,Powerwindows=tab.Powerwindows
       ,[ABS]=tab.[ABS]
       ,CentralLocking=tab.CentralLocking
       ,AirBags=tab.AirBags
       --,Displacement=tab.Displacement
       --,[Power]=tab.[Power]
    FROM(
	SELECT DISTINCT CarVersionId
		,MAX(CASE IV.ItemMasterId WHEN 29 THEN CONVERT(VARCHAR, UDF.Name) ELSE NULL END)   AS GearBox
		,MAX(CASE IV.ItemMasterId WHEN 150 THEN CONVERT(VARCHAR, ItemValue) ELSE NULL END) AS ChildSafety
		,MAX(CASE IV.ItemMasterId WHEN 81 THEN CONVERT(VARCHAR, UDF.Name) ELSE NULL END)   AS Powerwindows
		,MAX(CASE IV.ItemMasterId WHEN 260 THEN CONVERT(VARCHAR, ItemValue) ELSE NULL END)  AS [ABS] ---- Modified by Reshma 10-09-2013 The itemmasterid of ABS has been changed from 55 to 260
		,MAX(CASE IV.ItemMasterId WHEN 149 THEN CONVERT(VARCHAR, UDF.Name) ELSE NULL END)  AS CentralLocking
		,MAX(CASE IV.ItemMasterId WHEN 155 THEN CONVERT(VARCHAR, UDF.Name) ELSE NULL END)  AS AirBags
		--,MAX(CASE IV.ItemMasterId WHEN 14 THEN CONVERT(VARCHAR, ItemValue) ELSE NULL END)  AS Displacement
		--,MAX(CASE IV.ItemMasterId WHEN 249 THEN CONVERT(VARCHAR, REPLACE(REPLACE(CustomText,'~',''),'@','')) ELSE NULL END) AS [Power]
	FROM CD.ItemValues IV WITH(NOLOCK)
		LEFT JOIN CD.UserDefinedMaster UDF WITH(NOLOCK) ON UDF.UserDefinedId = IV.UserDefinedId
	WHERE  CarVersionId=@CarVersionId 
	GROUP BY CarVersionId) AS tab
	WHERE Versionid=CarVersionId
	
END
