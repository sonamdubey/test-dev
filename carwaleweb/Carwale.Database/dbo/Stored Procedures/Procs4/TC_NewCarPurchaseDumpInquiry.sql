IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NewCarPurchaseDumpInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NewCarPurchaseDumpInquiry]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 03- March-2014
-- Description:	Dump 6 days data from NewCarPurchase table to TC_NewCarPurchaseLead table.
--Note -- Change the condition AND  NCP.RequestDateTime >= (GETDATE()-@NoOfDaysData) as AND  NCP.RequestDateTime >= (GETDATE()- 1) after first run of this Sp. This will insert one day data in table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_NewCarPurchaseDumpInquiry] 
	
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN 
	--DELETE OLD DATA
	TRUNCATE TABLE TC_NewCarPurchaseLead

	--Insert Customer and PQ details into temp table
	SELECT DISTINCT CW.Name  AS Name, CW.Mobile AS Moblie, CW.email AS Email,
				VW.Version, VW.Model, VW.Make, NCP.BuyTime as BuyTime, 
				NCP.RequestDateTime as ReqDate, Vw.MakeId as MakeId , VW.ModelId, vw.VersionId, CW.CityId, NCP.Id AS NewCarPuchaseId,
				NCP.CustomerId AS CWCustomerId 
	INTO #TempCustDetails
	FROM vwMMV AS VW WITH (NOLOCK)
		INNER JOIN NewCarPurchaseInquiries as NCP WITH (NOLOCK) ON  VW.VersionId = NCP.CarVersionId
		INNER JOIN Customers AS CW  WITH (NOLOCK) ON CW.Id = NCP.CustomerId
		INNER JOIN TC_DealerMakes CV WITH (NOLOCK) ON CV.MakeId = VW.MakeId
	WHERE  NCP.ForwardedLead = 1 AND NCP.RequestDateTime BETWEEN CONVERT(DATE, GETDATE()-7) AND CONVERT(DATE, GETDATE()-1)

	
	--Dump data  into TC_NewCarPurchaseLead 
	INSERT INTO [dbo].[TC_NewCarPurchaseLead]
           ([CustomerName]
           ,[Mobile]
           ,[Email]
           ,[CarVersion]
           ,[CarModel]
           ,[CarMake]
           ,[BuyTime]
           ,[ReqDate]
           ,[MakeId]
           ,[ModelId]
           ,[VersionId]
           ,[CityId]
           ,[NewCarPurchaseId]
           ,[CWCustomerId],
		   [RowNo])	 
	    SELECT Name, Moblie, Email,
				Version, Model, Make,BuyTime, 
				ReqDate, MakeId , ModelId, VersionId, CityId, NewCarPuchaseId,
				CWCustomerId
				,ROW_NUMBER() OVER(PARTITION BY  CWCustomerId ORDER BY ReqDate DESC ) AS CusIdRowNo 
		FROM (
		SELECT DISTINCT Name, Moblie, Email,
				Version, Model, Make,BuyTime, 
				ReqDate, MakeId , ModelId, VersionId, CityId, NewCarPuchaseId,
				CWCustomerId
		FROM  #TempCustDetails AS CW 
			INNER JOIN CV_MobileEmailPair CV WITH (NOLOCK) ON CV.MobileNo = CW.Moblie) AS Tab
		
		SELECT * FROM TC_NewCarPurchaseLead

		--Drop the temp table
		DROP TABLE #TempCustDetails
		


END

END