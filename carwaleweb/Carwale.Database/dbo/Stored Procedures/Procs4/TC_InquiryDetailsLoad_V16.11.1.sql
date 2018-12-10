IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryDetailsLoad_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryDetailsLoad_V16]
GO

	
-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,09/01/2013>
-- Description:	<Description,gives inquiry details based on type>
-- EXEC TC_InquiryDetailsLoad 5,7778,3
-- Modified By: Nilesh Utture on 29th March, 2013 Added fields in select statement
-- Modified By: Tejashree on 07-06-2013
-- Modified By: Nilesh Utture on 10th June, 2013 Done changes in select for loose buyer
-- Modified By: Vivek Gupta on 24th sep, 2013 , Done changes in inquirytype = 3, added variables in select to retrieve car exchange details
-- Modified By Vivek Gupta on 24-12-2013, changes done for retrieving test drive and booking details for a buyer inquiry.
--[dbo].[TC_InquiryDetailsLoad] 5,18477,3
-- Modified by : Kritika Choudhary on 15th Feb 2016, changes in inqType=3 for the case PQRequest
-- Modified By : Ashwini Dhamankar On 29th March 2016,changed FinalAmount to OnRoadPrice.
-- Modified By : Ruchira Patil on 27th Apr 2016 Description: Join with CustomerSellInquiryDetails to fetch owners
-- Modified By : Tejashree Patil on 12 May 2016, Fetch url from comment.
-- Modified By : Nilima More On 16th June 2016,commented Ex_showroom price and On road price .
-- Modified By : Nilima More On 22nd June 2016,Fetch Ex_showroom price as coloumn name.
-- Modified By : Ashwini Dhamankar on Oct 5,2016 (Added inquiryType = 5 (advantage))
-- Modified by : Kritika Choudhary on 4th Nov 2016, commented join with NewCarShowroomPrices and one condition in where clause for inqtype=3

-- =============================================

CREATE PROCEDURE   [dbo].[TC_InquiryDetailsLoad_V16.11.1]
	-- Add the parameters for the stored procedure here
	@BranchId INT,
	@InquiryId BIGINT,
	@InquiryType TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @ModelName VARCHAR(500)
	DECLARE @BodyStyle VARCHAR(100)
	DECLARE @FuelType VARCHAR(100)
	DECLARE @Quote CHAR 
	
	SET @Quote = '';


    -- Insert statements for procedure here
    IF(@InquiryType = 1)
		BEGIN
			IF((SELECT StockId FROM TC_BuyerInquiries WITH (NOLOCK) WHERE TC_BuyerInquiriesId = @InquiryId) IS NOT NULL)
				BEGIN
					SELECT SRC.Source AS 'Source:', S.Id AS 'StockId:', REPLACE(RIGHT(CONVERT(VARCHAR(9), S.MakeYear, 6), 6), ' ', '-') AS 'Year:', S.Colour AS 'Color:', S.Price AS 'Price:', 
					S.Kms AS 'Distance:', S.RegNo + '(' + CC.RegistrationPlace + ')' AS 'Registration:', 
					(CASE CC.Owners 
					WHEN 1 THEN 'First' 
					WHEN 2 THEN 'Second' 
					WHEN 3 THEN 'Third' 
					WHEN 4 THEN 'Fourth' 
					WHEN 5 THEN 'More than four' END) 
					AS 'Owner:', CC.Insurance AS 'Insurance:', CC.OneTimeTax AS 'Lifetime Tax:', CC.InsuranceExpiry AS 'Insurance Expiry:',
					CC.CarDriven AS 'Car Driven In:',
					(CASE CC.Accidental 
					WHEN 0 THEN 'No' 
					WHEN 1 THEN 'Yes' 
					END) AS  'Accidental:', 
					(CASE CC.FloodAffected 
					WHEN 0 THEN 'No' 
					WHEN 1 THEN 'Yes' 
					END) AS  'Flood Affected:' ,
					B.Comments AS 'Comments:',
					(CASE B.IsTDRequested -- Modified By Vivek Gupta on 24-12-2013
					WHEN 0 THEN 'No' 
					WHEN 1 THEN 'Yes ,' + ' Date :' + CONVERT(varchar,B.TDRequestedDate)
					END) AS  'TD Request:',
					(CASE B.BookingRequested 
					WHEN 0 THEN 'No' 
					WHEN 1 THEN 'Yes ,' + ' Date :' +CONVERT(varchar, B.BookingRequestedDate)
					END) AS  'Booking Request:'
					FROM TC_BuyerInquiries B WITH (NOLOCK)
					INNER JOIN TC_Stock S WITH (NOLOCK) ON B.StockId = S.Id
					INNER JOIN TC_CarCondition CC WITH (NOLOCK) ON CC.StockId = S.Id
					INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = B.TC_InquirySourceId
					WHERE B.TC_BuyerInquiriesId = @InquiryId AND S.BranchId = @BranchId
				END
			ELSE
				BEGIN
					SELECT @ModelName = COALESCE(@ModelName+', ' ,' ') + CONVERT(VARCHAR,MK.Name + ' ' + MD.Name ) 
						FROM TC_PrefModelMake PM WITH (NOLOCK)
						INNER JOIN CarModels MD WITH (NOLOCK) ON PM.ModelId=MD.ID 
						INNER JOIN CarMakes MK WITH (NOLOCK) ON MK.Id=MD.CarMakeId 
						WHERE PM.TC_BuyerInquiriesId=@InquiryId
					SELECT @BodyStyle = COALESCE(@BodyStyle+', ' ,' ') + CONVERT(VARCHAR,CB.Name) 
						FROM TC_PrefBodyStyle PB WITH (NOLOCK)
						INNER JOIN CarBodyStyles CB WITH (NOLOCK) ON PB.BodyType = CB.ID 
						WHERE PB.TC_BuyerInquiriesId=@InquiryId
					SELECT @FuelType = COALESCE(@FuelType+', ' ,' ') + CONVERT(VARCHAR,CF.FuelType) 
						FROM TC_PrefFuelType PF WITH (NOLOCK)
						INNER JOIN CarFuelType CF WITH (NOLOCK) ON PF.FuelType = CF.FuelTypeId 
						WHERE PF.TC_BuyerInquiriesId=@InquiryId
					SELECT SRC.Source AS 'Source:', ISNULL(@ModelName,'') AS 'Interested In:', '' AS 'StockId:', 'Rs. ' + CONVERT(VARCHAR,B.PriceMin) + ' - ' + CONVERT(VARCHAR,B.PriceMax) AS 'Price Range:',CONVERT(VARCHAR,B.MakeYearFrom) + ' - ' + CONVERT(VARCHAR,B.MakeYearTo) AS 'Make Year:', ISNULL(@BodyStyle,'') AS 'Body Style:', ISNULL(@FuelType,'') AS 'Fuel Type:',
						  (CASE  -- Modified By: Nilesh Utture on 10th June, 2013
							  WHEN B.Comments LIKE 'http://%' 
							  THEN '<a onclick="window.open(''play.html?recUrl=' + B.Comments + ''',''newwindow'',''toolbar=no,resizable=yes,scrollbars=no,location=no,menubar=no,width=330,height=80'')">Click here</a> to listen call recording' 
							  ELSE B.Comments 
						  END) AS 'Comments:'
						FROM TC_BuyerInquiries B WITH (NOLOCK)
						INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = B.TC_InquirySourceId
						WHERE B.TC_BuyerInquiriesId = @InquiryId 
				END
		END
	ELSE IF(@InquiryType = 2)
		BEGIN
			SELECT SRC.Source AS 'Source:', ISNULL(CONVERT(VARCHAR,CWInquiryId),'') AS 'Profile Id:',REPLACE(RIGHT(CONVERT(VARCHAR(9), S.MakeYear, 6), 6), ' ', '-') AS 'Year:', S.Colour AS 'Color:', S.Price AS 'Price:', 
					S.Kms AS 'Distance:', S.RegNo + '(' + S.RegistrationPlace + ')' AS 'Registration:', 
			
			
			(CASE CASE WHEN S.CWInquiryId IS NOT NULL THEN ISNULL(CSD.Owners,0) ELSE ISNULL(S.Owners,0)END 
			WHEN 1 THEN 'First' 
			WHEN 2 THEN 'Second' 
			WHEN 3 THEN 'Third' 
			WHEN 4 THEN 'Fourth' 
			WHEN 5 THEN 'More than four' END) 
			AS 'Owner:', S.Insurance AS 'Insurance:', ISNULL(CONVERT(VARCHAR,S.InsuranceExpiry,106),'') AS 'Insurance Expiry:', S.Tax AS 'Tax:',S.CarDriven AS 'Car Driven In:',
			(CASE S.Accidental 
			WHEN 0 THEN 'No' 
			WHEN 1 THEN 'Yes' 
			END) AS  'Accidental:', 
			(CASE S.FloodAffected 
			WHEN 0 THEN 'No' 
			WHEN 1 THEN 'Yes' 
			END) AS  'Flood Affected:' 
			FROM TC_SellerInquiries S WITH (NOLOCK)
			INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = S.TC_InquirySourceId 
			LEFT JOIN CustomerSellInquiryDetails CSD WITH(NOLOCK) ON CSD.InquiryId = S.CWInquiryId
			WHERE S.TC_SellerInquiriesId =@InquiryId
		END
	ELSE IF(@InquiryType = 3 OR @InquiryType = 5)  -- Modified By : Ashwini Dhamankar on Oct 5,2016
		BEGIN 
			DECLARE @ColorList VARCHAR(500)
			DECLARE @CarExchange VARCHAR(5) -- Modified By: Vivek Gupta on 24th sep, 2013

			SELECT @ColorList =COALESCE(@ColorList+', ' ,'') + convert(VARCHAR,VC.Color) 
			FROM TC_NewCarInquiries NI WITH (NOLOCK)
			LEFT OUTER JOIN TC_PrefNewCarColour PNC  WITH (NOLOCK) ON PNC.TC_NewCarInquiriesId = NI.TC_NewCarInquiriesId
			JOIN VersionColors VC  WITH (NOLOCK) ON VC.ID = PNC.VersionColorsId
			WHERE NI.TC_NewCarInquiriesId = @InquiryId	
			

 ---------------------------------------------------------------------------------------------------------------------
			-- This block added by Vivek Gupta on 24th sep, 2013 for implementing car exchange information
			-- Modified By: Vivek Gupta on 24th sep, 2013
			SELECT  @CarExchange = CASE NI.IsExchange WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE NULL END 
			FROM TC_NewCarInquiries NI WITH (NOLOCK)
			WHERE NI.TC_NewCarInquiriesId = @InquiryId

			DECLARE @Kms INT = NULL
			DECLARE @MakeYear VARCHAR(25) = NULL
			DECLARE @ExpectedPrice INT = NULL
			DECLARE @CarName VARCHAR(80) = NULL	
           
			IF(@CarExchange IS NOT NULL)-- If block added by Vivek Gupta on 24th sep, 2013, Added if to retrieve details
			BEGIN				
				SELECT @Kms=ENC.Kms, 
					   @MakeYear = CONVERT(VARCHAR(11),ENC.MakeYear, 106), 
					   @ExpectedPrice = ENC.ExpectedPrice,
					   @CarName=V.Car
                  FROM TC_ExchangeNewCar ENC  WITH (NOLOCK)
				    JOIN VWMMV  AS V  WITH (NOLOCK) ON ENC.CarVersionId=V.VersionId 
			    WHERE TC_NewCarInquiriesId = @InquiryId
			END

------------------------------------------------------------------------------------------------------------------------------
			
			SELECT SRC.Source AS 'Source:',
			@ColorList AS 'Colors:',
			-- Commented By Nilima More On 16th June 2016,all commented details will fetch from carwale Api.
			--'Rs. ' + CASE WHEN ISNULL(NI.PQStatus,0)=25 THEN CONVERT(VARCHAR,PQR.ExShowRoomPrice) ELSE ISNULL(CONVERT(VARCHAR,NP.Price),'-') END 

			-- Modified By : Nilima More On 22nd June 2016,Fetch Ex_showroom price as coloumn name.
			'' as 'Ex Showroom Price:', 
			-- 'Rs. ' + CASE WHEN ISNULL(NI.PQStatus,0)=25 THEN CONVERT(VARCHAR,PQR.Insurance) ELSE ISNULL(CONVERT(VARCHAR,NP.Insurance),'-') END 
			'' as 'Insurance:',
			-- CASE WHEN NI.PQStatus=25 THEN PQR.RTO ELSE ISNULL(CONVERT(VARCHAR,NP.RTO),'-') END 'RTO:',
			--'Rs. ' + CASE WHEN NI.PQStatus=25 THEN CONVERT(VARCHAR,PQR.OnRoadPrice) ELSE CONVERT(VARCHAR,(ISNULL(NP.Price,0)+ISNULL(NP.Insurance,0))) END 
			'' as 'On Road Price:'
			, NI.Comments AS 'Comments:',
			 @CarExchange AS 'Car Exchange:',@CarName AS 'Car Name:', @Kms AS 'Kilometers:', @MakeYear AS 'Make Year:', @ExpectedPrice AS 'Expected Price:', -- Modified By: Vivek Gupta on 24th sep, 2013,Added variables in select statement
			(CASE  -- Modified By: Tejashree Patil on 12 May 2016, Fetch url from comment.
							  WHEN NI.Comments LIKE 'http://%' 
							  THEN '<a onclick="window.open(''play.html?recUrl=' + NI.Comments + ''',''newwindow'',''toolbar=no,resizable=yes,scrollbars=no,location=no,menubar=no,width=330,height=80'')">Click here</a> to listen call recording' 
							  ELSE NI.Comments 
						  END) AS 'Comments:'
			FROM TC_NewCarInquiries NI WITH (NOLOCK)
			--INNER JOIN NewCarShowroomPrices NP WITH (NOLOCK) ON NI.VersionId = NP.CarVersionId --Modified by : kritika Choudhary on 4th Nov 2016,commented code
			INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = NI.TC_InquirySourceId
			LEFT OUTER JOIN TC_PrefNewCarColour PNC WITH(NOLOCK) ON PNC.TC_NewCarInquiriesId = NI.TC_NewCarInquiriesId
			LEFT JOIN TC_PriceQuoteRequests PQR WITH (NOLOCK) ON PQR.TC_InquiriesId = NI.TC_NewCarInquiriesId AND PQR.TC_InquiriesId = @InquiryId --AND PQC.IsActive=1
			--LEFT JOIN TC_PQRequestComponent PQ WITH (NOLOCK) ON  PQ.TC_PriceQuoteRequestsId = PQR.Id
			--LEFT JOIN TC_PQComponents PQC WITH (NOLOCK) ON PQC.TC_PQComponentsId = PQ.TC_PQComponentId
			WHERE NI.TC_NewCarInquiriesId = @InquiryId --AND NP.CityId = (SELECT CityId FROM Dealers WITH(NOLOCK) WHERE Id=@BranchId)--Modified by : kritika Choudhary on 4th Nov 2016,commented and condition
			      

				 

	        IF EXISTS(SELECT TC_PQComponentId FROM TC_PQRequestComponent WITH(NOLOCK) WHERE TC_PriceQuoteRequestsId = (SELECT Id FROM TC_PriceQuoteRequests  WITH(NOLOCK) WHERE TC_InquiriesId = @InquiryId))
	        BEGIN
				SELECT PQR.Amount,PQC.Name
				FROM TC_PQRequestComponent  PQR WITH(NOLOCK) JOIN TC_PQComponents PQC WITH(NOLOCK) ON PQC.TC_PQComponentsId = PQR.TC_PQComponentId
				WHERE TC_PriceQuoteRequestsId = (SELECT Id FROM TC_PriceQuoteRequests  WITH (NOLOCK) WHERE TC_InquiriesId = @InquiryId) and PQC.IsActive=1
		    END
		END
END

