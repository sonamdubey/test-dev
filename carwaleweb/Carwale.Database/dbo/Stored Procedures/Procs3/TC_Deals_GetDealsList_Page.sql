IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetDealsList_Page]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetDealsList_Page]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 4th Jan 2016
-- Description:	To fetch deals according to the status
-- @Status = 1	Unapproved
-- @Status = 2	Active
-- @Status = 3	Rejected Stock
-- @Status = 4	Blocked Online(Open)
-- @Status = 5	Blocked Online(Confirmed)
-- @Status = 6	Car Booked
-- @Status = 7	Car Delivered
-- @Status = 8	Booking Cancelled
-- @Status = 9	Blocked Online(Cancelled)
-- @Status = 11	Dealer Blocked
-- @Status = 12	Unavailable
-- exec TC_Deals_GetDealsList.V1 5,null,null,1,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetDealsList_Page] 
	 @BranchId BIGINT
	,@MakeId INT = NULL
	,@ModelId INT = NULL
	,@Status TINYINT 
	,@PageNumber TINYINT = 1
	,@PageSize INT=NULL OUTPUT
	,@SelectedRows INT=NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
   
   DECLARE @TempTbl TABLE
	(
		Id INT,
		CityPrice VARCHAR(MAX)
	)

	--to get cities and price comma separated from TC_Deals_StockPrices Table in case of Unapproved,active,rejected,dealer blocked,unavailable
	-- (CITY)-ActualOnroadPrice/DiscountedPrice
	IF(@Status IN (1,2,3,11,12))
	BEGIN
		INSERT INTO @TempTbl(Id,CityPrice)
		SELECT  DS.ID
			   ,STUFF((SELECT ', ' + (CT.Name + ' - ' +  CAST(DSP.ActualOnroadPrice AS VARCHAR(50)) +  '/' + CAST(DSP.DiscountedPrice AS VARCHAR(50)))
				 FROM TC_Deals_StockPrices DSP WITH (NOLOCK)
		INNER JOIN Cities CT WITH (NOLOCK) ON CT.ID = DSP.CityId
		WHERE dsp.TC_Deals_StockId = ds.id
				 FOR XML PATH(''), TYPE)
				.value('.','NVARCHAR(MAX)'),1,2,' ')
		FROM TC_Deals_Stock ds WITH (NOLOCK)
		LEFT JOIN TC_Deals_StockPrices DSP WITH (NOLOCK) ON DSP.TC_Deals_StockId = DS.Id
		WHERE DS.BranchId = @BranchId	
		GROUP BY DS.ID
	END;

	SELECT DISTINCT DSV.TC_DealsStockVINId TC_DealsStockVINId, DSV.VINNo VIN,DS.Id AS DealStockId,(CMK.Name + ' ' + CM.Name + ' ' + CV.Name) Car,
	--(DATENAME(month ,DS.MakeYear) + ' ' + YEAR(DS.MakeYear)) MakeYear, 
	DS.MakeYear MakeYear,
	dbo.Titlecase(C.Color) Colour,DS.InteriorColor InteriorColor,
	DS.Offers Offers
	,CASE @Status WHEN 1 THEN DS.EnteredOn ELSE DS.LastUpdatedOn END AS ActionPerformedOn
	,CASE WHEN @Status IN (1,2,3,11,12) THEN TT.CityPrice ELSE (CT.Name + ' - ' +  CAST(DSP.ActualOnroadPrice AS VARCHAR(50)) +  '/' + CAST(NCB.Price AS VARCHAR(50))) END CityPrice
	,NCI.TC_NewCarInquiriesId TC_NewCarInquiriesId,IL.TC_UserId LeadOwnerId,IL.TC_CustomerId CustId,
	CASE DSV.Status WHEN 14 THEN 'Dealer Booked' WHEN 6 THEN 'Online Booked' ELSE NULL END BookedMedium
	,ROW_NUMBER() OVER(ORDER BY DS.Id,(CMK.Name + ' ' + CM.Name + ' ' + CV.Name))
		   AS ROWNUMBER
	INTO #TableForPaging
	FROM TC_Deals_Stock DS WITH (NOLOCK)
	INNER JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = DS.BranchId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.Id = DS.CarVersionId
	INNER JOIN CarModels CM WITH (NOLOCK) ON CM.ID = CV.CarModelId AND (@ModelId IS NULL OR CM.ID = @ModelId)
	INNER JOIN CarMakes CMK WITH (NOLOCK) ON CMK.ID = CM.CarMakeId AND (@MakeId IS NULL OR CMK.ID = @MakeId)
	INNER JOIN VersionColors C WITH (NOLOCK) ON C.ID = DS.VersionColorId
	LEFT JOIN TC_Deals_StockVIN DSV WITH (NOLOCK) ON DSV.TC_Deals_StockId = DS.Id
	LEFT JOIN TC_Deals_StockPrices DSP WITH (NOLOCK) ON DSP.TC_Deals_StockId = DS.Id
	LEFT JOIN TC_NewCarInquiries NCI WITH (NOLOCK) ON NCI.TC_DealsStockVINId = DSV.TC_DealsStockVINId
	LEFT JOIN TC_InquiriesLead IL WITH (NOLOCK) ON IL.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
	LEFT JOIN TC_NewCarBooking NCB WITH (NOLOCK) ON NCB.TC_Deals_StockVINId = DSV.TC_DealsStockVINId
	LEFT JOIN Cities CT WITH (NOLOCK) ON CT.ID = NCI.CityId
	LEFT JOIN @TempTbl TT ON TT.Id = DS.Id
	WHERE DS.BranchId = @BranchId AND DD.IsDealerDealActive = 1
	AND
	(
		(@Status = 1 AND DSV.Status = 1)
		OR (@Status = 2 AND DSV.Status = 2)
		OR (@Status = 3 AND DSV.Status = 3)
		OR (@Status = 4 AND DSV.Status = 4)
		OR (@Status = 5 AND DSV.Status = 5)
		OR (@Status = 6 AND DSV.Status IN (6,14)) -- car booked count includes the car booked online and dealer booked
		OR (@Status = 7 AND DSV.Status = 7)
		OR (@Status = 8 AND DSV.Status = 8)
		OR (@Status = 9 AND DSV.Status = 9)
		OR (@Status = 11 AND DSV.Status = 11)
		OR (@Status = 12 AND DSV.Status = 12)
	)

	DECLARE @FirstRow INT, @LastRow INT--To Calculate paging parameters
	SET @PageSize = 20
	SET @FirstRow = ((@PageNumber - 1) * @PageSize) + 1
	SET @LastRow = @FirstRow + @PageSize -1

	SELECT @SelectedRows = COUNT(*) FROM #TableForPaging

	SELECT DISTINCT (DealStockId),TBLPG.ROWNUMBER,TC_DealsStockVINId, VIN,Car,MakeYear,Colour,InteriorColor,
	Offers, ActionPerformedOn,CityPrice,TC_NewCarInquiriesId,LeadOwnerId,CustId,BookedMedium
	FROM #TableForPaging TBLPG WITH(NOLOCK) 
	WHERE TBLPG.ROWNUMBER BETWEEN @FirstRow AND @LastRow  
	ORDER BY TBLPG.ROWNUMBER
 	
	DROP TABLE #TableForPaging

END


