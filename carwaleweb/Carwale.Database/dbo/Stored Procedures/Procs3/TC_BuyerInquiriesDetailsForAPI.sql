IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BuyerInquiriesDetailsForAPI]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BuyerInquiriesDetailsForAPI]
GO

	-- =======================================================================
-- Created By: Nilesh Utture
-- Created On: 30th April, 2013
-- Description:This SP will give Loose buyer Inquiry details for API 
-- EXEC TC_BuyerInquiriesDetailsForAPI 7315,1265,400
-- Modified By: Nilesh Utture on 6th May, 2013 Reffered NewCarInquiriesId as Id
-- Modified By: Nilesh Utture on 1st September, 2013 Added COLUMN NS_ShowClose
-- Modified By Vivek Gupta on 30-09-2015, Added MostInterested for inquiries
-- Modified By :Khushaboo Patil on 17th March , Added columns InquiriesLeadId,LeadInquiryTypeId
-- =======================================================================
CREATE PROCEDURE [dbo].[TC_BuyerInquiriesDetailsForAPI] @TC_LeadId INT,@BranchId INT,@TC_UserId INT
AS
BEGIN
DECLARE @BuyerInquiryDetail TABLE (TC_BuyerInquiriesId INT,Type VARCHAR(25),NS_ShowClose BIT, CreatedOn DATETIME,Source VARCHAR(100),CarDetails VARCHAR(MAX),BodyStyles VARCHAR(200),FuelTypes VARCHAR(200), PriceRange VARCHAR(100), MakeYear VARCHAR(50), MostInterested BIT,	InquiriesLeadId INT,LeadInquiryTypeId INT)

 SELECT TC_InquiriesLeadId,
		BranchId,
		TC_CustomerId,
		TC_UserId,
		IsActive,
		TC_LeadId,
		TC_LeadInquiryTypeId,
		TC_LeadDispositionID
	  INTO #TC_InquiriesLead	
	  FROM TC_InquiriesLead as L1 WITH (NOLOCK) 
	  WHERE L1.BranchId=@BranchId ANd L1.TC_LeadId=@TC_LeadId

--select * from #TC_InquiriesLead	  
	  
	DECLARE @TC_BuyerInquiries TABLE (ID SMALLINT IDENTITY(1,1),TC_BuyerInquiriesID INT)
	DECLARE @TotalWhileLoop SMALLINT
    DECLARE @WhileLoopControl SMALLINT=1
    DECLARE @TC_BuyerInquiriesID INT
	  
	  
	 INSERT INTO @TC_BuyerInquiries (TC_BuyerInquiriesID )
	 SELECT DISTINCT TC_BuyerInquiriesID FROM 	 TC_BuyerInquiries AS B WITH (NOLOCK)
	 JOIN #TC_InquiriesLead AS T ON T.TC_InquiriesLeadId=B.TC_InquiriesLeadId
	 
--select * from @TC_BuyerInquiries

	 SELECT @TotalWhileLoop=COUNT(*) FROM @TC_BuyerInquiries
				
				DECLARE @ModelNames TABLE ( TC_BuyerInquiriesId INT,ModelName VARCHAR(500))
                DECLARE  @ModelName VARCHAR(500)
                DECLARE @BodyStyle VARCHAR(100)
	            DECLARE @FuelType VARCHAR(100)
	            
                     WHILE @WhileLoopControl<=@TotalWhileLoop
                     BEGIN
                SELECT @TC_BuyerInquiriesID=TC_BuyerInquiriesID,@ModelName=NULL FROM @TC_BuyerInquiries WHERE ID=@WhileLoopControl
                 --INSERT INTO @ModelNames (TC_BuyerInquiriesId,ModelName)
								
				SELECT   @FuelType = COALESCE(@FuelType+', ' ,' ') + CONVERT(VARCHAR,CF.FuelType) 
					 FROM       #TC_InquiriesLead   L  
					 INNER JOIN TC_BuyerInquiries   B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId   
					 INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = B.TC_InquirySourceId
					 LEFT JOIN  TC_PrefFuelType PM WITH (NOLOCK) ON PM.TC_BuyerInquiriesId = B.TC_BuyerInquiriesId
						INNER JOIN CarFuelType CF WITH (NOLOCK) ON PM.FuelType = CF.FuelTypeId 
					 WHERE L.BranchId=@BranchId
					   AND B.StockId IS NULL  
					   AND L.TC_UserId = @TC_UserId
					   ANd L.TC_LeadId=@TC_LeadId
					   AND (B.TC_LeadDispositionId IS NULL OR B.TC_LeadDispositionId = 4)	   
					AND B.TC_BuyerInquiriesId=@TC_BuyerInquiriesId
							
								
				SELECT   @BodyStyle = COALESCE(@BodyStyle+', ' ,' ') + CONVERT(VARCHAR,CB.Name) 
					 FROM       #TC_InquiriesLead   L  
					 INNER JOIN TC_BuyerInquiries   B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId   
					 INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = B.TC_InquirySourceId
					 LEFT JOIN  TC_PrefBodyStyle PM WITH (NOLOCK) ON PM.TC_BuyerInquiriesId = B.TC_BuyerInquiriesId
						INNER JOIN CarBodyStyles CB WITH (NOLOCK) ON PM.BodyType = CB.ID 
					 WHERE L.BranchId=@BranchId
					   AND B.StockId IS NULL  
					   AND L.TC_UserId = @TC_UserId
					   ANd L.TC_LeadId=@TC_LeadId
					   AND (B.TC_LeadDispositionId IS NULL OR B.TC_LeadDispositionId = 4)	   
					AND B.TC_BuyerInquiriesId=@TC_BuyerInquiriesId
					
				SELECT    @ModelName= COALESCE(@ModelName+',' ,'') + CONVERT(VARCHAR,MK.Name + ' ' + MD.Name)
					 FROM       #TC_InquiriesLead   L  
					 INNER JOIN TC_BuyerInquiries   B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId   
					 INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = B.TC_InquirySourceId
					 LEFT JOIN TC_PrefModelMake PM WITH (NOLOCK) ON PM.TC_BuyerInquiriesId = B.TC_BuyerInquiriesId
					 LEFT JOIN CarModels MD WITH (NOLOCK) ON PM.ModelId=MD.ID 
					 LEFT JOIN CarMakes MK WITH (NOLOCK) ON MK.Id=MD.CarMakeId 
					WHERE L.BranchId=@BranchId
					   AND B.StockId IS NULL  
					   AND L.TC_UserId = @TC_UserId
					   ANd L.TC_LeadId=@TC_LeadId
					   AND (B.TC_LeadDispositionId IS NULL OR B.TC_LeadDispositionId = 4)	   
					AND B.TC_BuyerInquiriesId=@TC_BuyerInquiriesId
				
			--	SELECT @ModelName
			    INSERT INTO @BuyerInquiryDetail
					 SELECT B.TC_BuyerInquiriesId AS Id,
			 		        (CASE L.TC_LeadInquiryTypeId WHEN 1 THEN 'Buyer' END),
			 		        (CASE B.BookingStatus WHEN 34 THEN 'false' ELSE 'true' END),
			 		        B.CreatedOn,
			 		        SRC.Source AS Source,
			 		       @ModelName,
			 		       @BodyStyle,
			 		       @FuelType,
			 		       'Rs. ' + CONVERT(VARCHAR,B.PriceMin) + ' - ' + CONVERT(VARCHAR,B.PriceMax) AS PriceRange,
			 		       CONVERT(VARCHAR,B.MakeYearFrom) + ' - ' + CONVERT(VARCHAR,B.MakeYearTo) AS MakeYear,
						   ISNULL(B.MostInterested,0) AS MostInterested,
						   	L.TC_InquiriesLeadId AS InquiriesLeadId,L.TC_LeadInquiryTypeId AS LeadInquiryTypeId
			 		        FROM       #TC_InquiriesLead   L  
					 INNER JOIN TC_BuyerInquiries   B WITH (NOLOCK)  ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId   
					 INNER JOIN TC_InquirySource SRC WITH (NOLOCK) ON SRC.Id = B.TC_InquirySourceId
					 WHERE L.BranchId=@BranchId
					   AND B.StockId IS NULL  
					   AND L.TC_UserId = @TC_UserId
					   ANd L.TC_LeadId=@TC_LeadId
					   and B.TC_BuyerInquiriesId=@TC_BuyerInquiriesID
					   AND (B.TC_LeadDispositionId IS NULL OR B.TC_LeadDispositionId = 4)	   
					   SET @WhileLoopControl=@WhileLoopControl+1
					   END 
					   SELECT TC_BuyerInquiriesId AS Id, Type,NS_ShowClose, CreatedOn, Source, CarDetails, BodyStyles, FuelTypes, PriceRange, MakeYear, MostInterested,	InquiriesLeadId,LeadInquiryTypeId FROM @BuyerInquiryDetail
					   
					   DROP TABLE #TC_InquiriesLead
END					   
	
