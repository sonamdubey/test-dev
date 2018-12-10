IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[RecommendCarAlertForSecondMail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[RecommendCarAlertForSecondMail]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 27-06-2013
-- Description:	Populate Profiles matching with used Car customer search criteria
--Modified By: Reshma Shetty 05/07/2013 Added new field Rank to also capture the rank of the matching profiles
-- Modified By: Manish on 08-07-2013 for selecting only 5 car for recommendation and the last day customer who didn't take any inquiry before seven day since job execute today.
-- Modified By: Manish on 29-07-2013 for correcting second mail insertion.
-- Modified By: Manish on 30-12-2013 added try block for handling the issue when any record contains error than other insertion should not affect.
--- Modified by: Manish on 28-02-2014 for implementation of the logic give by Moupiya/Sathish mail dated 24-02-2014
-- Modified by: Manish on 20-03-2014 for implementation of the second algorithm for Used car email alert
-- Modified by: Manish on 07-04-2014 changing the value of fronImagePath : Replace(LL.FrontImagePath,'80x60.JPG','300x225.jpg')
-- Modified by: Manish on 16-04-2014 for sending the mail third day also.
-- Modified by: Manish on 23-04-2014 for addition of the column IsFirstmail in UCAlert.RecommendUsedCarAlert table and sending mail to 10,15 and 20 day also.
-- Modified by : Kundan on 21-03-2016  changed column refernce FrontImagePath to  OriginalImgPath
-- =============================================

CREATE PROCEDURE [UCAlert].[RecommendCarAlertForSecondMail]
	-- Add the parameters for the stored procedure here
AS
BEGIN

    
     
    DECLARE  @Day TINYINT=6      ----How many days old inquiries
    DECLARE  @TotalMatchingCar TINYINT =4  --Maximum recommend car return by query
    DECLARE  @NoOfResp TINYINT=1    --- No. of inquiry of customer
	DECLARE  @NoOfRecommendCarForSecondAlgo TINYINT =4 

    DECLARE  @TblInquiryId  TABLE (ID INT IDENTITY(1,1),
                                   InquiryId INT, 
								   SellerType TINYINT,
								   ModelId INT,
								   Price  INT,
								   ModelYear DATETIME,
								   Kms   INT,
								   Owners   INT,
								   FuelTypeId TINYINT,
								   CityId    INT,
								   Email  VARCHAR(100),
								   MakeId INT,
								   BuyerCustomerId INT,
								   BuyerCustomerName VARCHAR(150),
								   ImageUrl VARCHAR(200),
								   MakeName VARCHAR(80),
								   ModelName VARCHAR(100),
								   VersionId INT)

	 CREATE TABLE #TblForSecondAlgo (ProfileId VARCHAR(50),
	                                 CarName VARCHAR(120),
									 FrontImagePath VARCHAR(500),
									 InquiryId INT,
									 SellerType SMALLINT,
									 Price  INT,
									 MakeYear DATETIME,
									 Kilometers INT,
									 PhotoCount INT ,
									 Score  FLOAT)
								  
    DECLARE  @ModelId   INT
    DECLARE  @FuelTypeId   INT
    DECLARE  @Price   INT
    DECLARE  @ModelYear   DATETIME
    DECLARE  @Kms   INT
    DECLARE  @Owners   INT
    DECLARE  @CityId   INT
    DECLARE  @CustomerId   INT
    DECLARE  @WhileLoopControl INT=1
    DECLARE  @WhileLoopCount INT
    DECLARE  @MakeId INT
    DECLARE  @Email  VARCHAR(100)
    DECLARE  @InquiryId INT
    DECLARE  @SellerType INT
    DECLARE  @CustomerName VARCHAR(150)
    DECLARE  @ImageUrl VARCHAR(200)  
    DECLARE  @MakeName VARCHAR(80)
    DECLARE  @ModelName VARCHAR(100)
	DECLARE  @VersionId INT ; 
     --------------------------------------------------------------------------------------------------------------------------   
        
    WITH CTE1 AS
				(SELECT  LL.Inquiryid AS InquiryId , ---capturing cardetails of the inquiries for dealer car
						LL.SellerType AS SellerType ,
						LL.ModelId AS ModelId,
						LL.Price AS Price,
						LL.MakeYear AS ModelYear,
						LL.Kilometers AS Kms,
						SID.Owners AS Owners,
						CV.CarFuelType AS FuelTypeId,
						LL.CityId AS CityId,
						Cu.Email AS Email,
						LL.MakeId AS MakeId,
						UCPI.CustomerID AS BuyerCustomerId,
						CU.Name  AS BuyerCustomerName,
						CASE WHEN  LL.OriginalImgPath IS NULL OR LL.OriginalImgPath ='' THEN '/used/no-car.jpg'  ELSE   LL.OriginalImgPath END AS ImageUrl,  -- Kundan on 21-03-2016  changed column refernce FrontImagePath to  OriginalImgPath
						LL.MakeName AS MakeName,
						LL.ModelName AS ModelName,
						UCPI.RequestDateTime AS RequestDateTime,
						LL.VersionId AS VersionId   ----------- Added by Manish on 20-03-2014 
					 FROM  UsedCarPurchaseInquiries AS UCPI WITH (NOLOCK)
					 JOIN  LiveListings AS LL WITH (NOLOCK) ON UCPI.SellInquiryId=LL.Inquiryid 
					 JOIN  SellInquiriesDetails AS SID WITH (NOLOCK) ON SID.SellInquiryId=LL.Inquiryid 
					 JOIN  UCAlert.UserCarAlerts  AS C WITH (NOLOCK) ON C.CustomerId=UCPI.CustomerID
					 JOIN  Customers  AS CU WITH (NOLOCK) on C.CustomerId=CU.Id
					 JOIN  CarVersions AS CV WITH (NOLOCK) ON LL.VersionId=CV.ID
					 WHERE LL.SellerType=1 
					-- AND   CONVERT(DATE,UCPI.RequestDateTime)>=CONVERT(DATE,GETDATE()-5)  -- modified by manish on 29-07-2013 from 6 to 5 
					 AND CU.IsFake=0
					 AND C.IsAutomated=1
					 AND C.IsActive=1
					 AND (    C.EntryDateTime = CONVERT(DATE,GETDATE()-4)  
					       OR C.EntryDateTime = CONVERT(DATE,GETDATE()-2)  --Modified by Manish on 16-04-2014 since mail should send on third day also
						   OR C.EntryDateTime = CONVERT(DATE,GETDATE()-9)
						   OR C.EntryDateTime = CONVERT(DATE,GETDATE()-14)
						   OR C.EntryDateTime = CONVERT(DATE,GETDATE()-19)
						 )
					 UNION ALL
				SELECT  LL.Inquiryid AS InquiryId,  ---capturing cardetails of the inquiries for individual car
						LL.SellerType AS SellerType,
						LL.ModelId AS ModelId,
						LL.Price AS Price,
						LL.MakeYear AS ModelYear,
						LL.Kilometers AS Kms,
						SID.Owners AS Owners,
						CV.CarFuelType AS FuelTypeId,
						LL.CityId AS CityId,
						Cu.Email AS Email,
						LL.MakeId AS MakeId,
						UCPI.CustomerID AS BuyerCustomerId,
						CU.Name AS BuyerCustomerName,
						CASE WHEN  LL.OriginalImgPath IS NULL OR LL.OriginalImgPath ='' THEN '/used/no-car.jpg'  ELSE  LL.OriginalImgPath  END AS ImageUrl, -- Kundan on 21-03-2016  changed column refernce FrontImagePath to  OriginalImgPath
						LL.MakeName AS MakeName,
						LL.ModelName AS ModelName,
						UCPI.RequestDateTime AS RequestDateTime,
						LL.VersionId AS VersionId   ----------- Added by Manish on 20-03-2014 
					 FROM  ClassifiedRequests AS UCPI WITH (NOLOCK)
					 JOIN  LiveListings AS LL WITH (NOLOCK) ON UCPI.SellInquiryId=LL.Inquiryid 
					 JOIN  CustomerSellInquiryDetails AS SID WITH (NOLOCK) ON SID.InquiryId=LL.Inquiryid 
					 JOIN  UCAlert.UserCarAlerts  AS C WITH (NOLOCK) ON C.CustomerId=UCPI.CustomerID
					 JOIN  Customers  AS CU WITH (NOLOCK) on C.CustomerId=CU.Id
					 JOIN  CarVersions AS CV WITH (NOLOCK) ON LL.VersionId=CV.ID
					 WHERE LL.SellerType=2
					-- AND   CONVERT(DATE,UCPI.RequestDateTime)>=CONVERT(DATE,GETDATE()-5) -- modified by manish on 29-07-2013 from 6 to 5 
					 AND CU.IsFake=0
					 AND C.IsAutomated=1
					 AND C.IsActive=1
					 AND  (     C.EntryDateTime = CONVERT(DATE,GETDATE()-4)  
					         OR C.EntryDateTime = CONVERT(DATE,GETDATE()-2)
						     OR C.EntryDateTime = CONVERT(DATE,GETDATE()-9)
							 OR C.EntryDateTime = CONVERT(DATE,GETDATE()-14)
							 OR C.EntryDateTime = CONVERT(DATE,GETDATE()-19)
						  ) --Modified by Manish on 16-04-2014 since mail should send on third day also
					),CTE2 AS ( SELECT *,ROW_NUMBER() OVER (PARTITION BY BuyerCustomerId ORDER BY  RequestDateTime DESC) RowNum FROM CTE1
				           )
					 INSERT INTO @TblInquiryId (InquiryId ,   
								    SellerType ,
								    ModelId ,
								    Price  ,
								    ModelYear ,
								    Kms   ,
								    Owners,
								    FuelTypeId,
								    CityId,
								    Email ,
								    MakeId,
								    BuyerCustomerId,
								    BuyerCustomerName,
								    ImageUrl,
								    MakeName,
								    ModelName ,
									VersionId  )
						SELECT      InquiryId ,   
								    SellerType ,
								    ModelId ,
								    Price  ,
								    ModelYear ,
								    Kms   ,
								    Owners,
								    FuelTypeId,
								    CityId,
								    Email ,
								    MakeId,
								    BuyerCustomerId,
								    BuyerCustomerName,
								    ImageUrl,
								    MakeName,
								    ModelName,
									VersionId 
						FROM CTE2 WHERE RowNum=1;
         
      SELECT @WhileLoopCount=COUNT(ID) FROM @TblInquiryId
      
      
      WHILE (@WhileLoopControl<=@WhileLoopCount)
      BEGIN
	         
			BEGIN TRY       --- Try Block added by manish on 30-12-2013 for resolving the issue when any record contains error than other insertion should not affect.  
			 
			  SELECT  @ModelId=ModelId,         --process each inquiry id for its similar car 
					  @FuelTypeId=FuelTypeId,
					  @Price =Price,
					  @ModelYear=ModelYear,
					  @Kms =  Kms,
					  @Owners=Owners,
					  @CityId=CityId,
					  @FuelTypeId=FuelTypeId,
					  @Email=Email,
					  @MakeId=MakeId,
					  @InquiryId=InquiryId,
					  @SellerType=sellerType,
					  @CustomerId=BuyerCustomerId,
					  @CustomerName=BuyerCustomerName,
					  @ImageUrl=ImageUrl,
					  @MakeName=MakeName,
					  @ModelName=ModelName,
					  @VersionId=VersionId   ----------- Added by Manish on 20-03-2014 
			 FROM  @TblInquiryId
			 WHERE ID=@WhileLoopControl;
			 
			 WITH CTE1 AS 
			( SELECT 
				@CustomerId AS  CustomerId,
				@CustomerName AS CustomerName,
				@Email  AS   CustomerAlert_Email,
				LL.ProfileId  AS  ProfileId,
				LL.Inquiryid  AS   Car_SellerId,
				LL.SellerType  AS  Car_SellerType,
				LL.CityId  AS  Car_CityId,
				LL.CityName  AS  Car_City,
				LL.Price  AS  Car_Price,
				LL.MakeId  AS  Car_MakeId,
				LL.MakeName  AS Car_Make,
				LL.ModelId  AS Car_ModelId,
				LL.ModelName  AS Car_Model,
				LL.Kilometers  AS Car_Kms,
				LL.VersionName  AS Car_Version,
				LL.MakeYear  AS Car_Year,
				LL.Color  AS Car_Color,
				CASE WHEN LL.PhotoCount>0 THEN 1 ELSE 0 END  AS Car_HasPhoto,
				LL.LastUpdated AS Car_LastUpdated,
				0 AS Is_Mailed,
				'make=' + CONVERT(VARCHAR,LL.MakeId)
                + '&budget='  +  convert (varchar,(SELECT TOP 1 BudgetId FROM UCAlert.Budget WITH (NOLOCK) WHERE LL.Price Between LowerVal and UpperVal))
                + '&city='  +  CONVERT(VARCHAR,LL.CityId) 
                + '&dist=50'
                /*+ '&kms=' +  CONVERT(VARCHAR,(SELECT TOP 1 CarkmId FROM UCAlert.CarKms WHERE LL.Kilometers Between LowerVal and UpperVal))*/ AS alertUrl,
				GETDATE() AS CreatedOn,
				CASE WHEN  LL.OriginalImgPath IS NULL OR LL.OriginalImgPath ='' THEN '/used/no-car.jpg'  ELSE ll.OriginalImgPath END AS ImageUrl,  --  Kundan on 21-03-2016  changed column refernce FrontImagePath to  OriginalImgPath
				@MakeName AS CarMake,
				@ModelName  As CarModel,
			 CASE WHEN CV.CarFuelType=@FuelTypeId THEN '1' --Priority of the rank given to similar fuel car the petrol, diesel etc
			      WHEN CV.CarFuelType=1  THEN '2'
			      WHEN CV.CarFuelType=2  THEN '3'
			      WHEN CV.CarFuelType=3  THEN '4'
			      WHEN CV.CarFuelType=4  THEN '5'
			      WHEN CV.CarFuelType=5  THEN '6' END
			 +     
			 CASE WHEN ABS(@Price-Price)<= (@Price*5/100) THEN '1'
			      WHEN ABS(@Price-Price)<= (@Price*10/100)  THEN '2'
			      WHEN ABS(@Price-Price)<= (@Price*15/100)  THEN '3'
			      WHEN ABS(@Price-Price)<= (@Price*20/100)  THEN '4'
			      WHEN ABS(@Price-Price)<= (@Price*25/100)  THEN '5'
			      WHEN ABS(@Price-Price)<= (@Price*30/100)  THEN '6' END
			  +     
			 CASE WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=1  THEN '1'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=2  THEN '2'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=3 THEN '3'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=4  THEN '4'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=5  THEN '5'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=6  THEN '6'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))>6  THEN '7' END
			  +    
			 CASE WHEN ABS(@Kms- Kilometers) <=2000  THEN '1'
			      WHEN ABS(@Kms- Kilometers) <=4000  THEN '2'
			      WHEN ABS(@Kms- Kilometers) <=6000  THEN '3'
			      WHEN ABS(@Kms- Kilometers) <=8000   THEN '4'
			      WHEN ABS(@Kms- Kilometers) <=10000   THEN '5'
			      WHEN ABS(@Kms- Kilometers) <=15000   THEN '6'
			      WHEN ABS(@Kms- Kilometers) <=20000   THEN '7'
			      WHEN ABS(@Kms- Kilometers) <=25000   THEN '8'
			      WHEN ABS(@Kms- Kilometers) >25000   THEN '9' END
			 +
			    CASE LL.Owners
							WHEN 'First Owner '   THEN '1'
							WHEN 'Second Owner '  THEN '2'
							WHEN 'Third Owner '	  THEN '3'
							WHEN 'Fourth Owner'   THEN '4'
							WHEN 'More than 4 owners' THEN '5' END
                 +
                 CASE 
							WHEN PhotoCount>=5   THEN '1'
							WHEN PhotoCount=1  THEN '3'
							WHEN PhotoCount=0	  THEN '4'
							ELSE '2' END   AS [Rank] -- If PhotoCount between 2 and 4 
             FROM  livelistings AS LL WITH (NOLOCK)
             JOIN  CarVersions AS CV WITH (NOLOCK) ON CV.ID=LL.VersionId
			 WHERE ModelId=@ModelId 
			-- AND CV.CarFuelType=@FuelTypeId
			 AND CityId=@CityId
			 AND (Inquiryid<>@InquiryId  OR SellerType<>@SellerType)
			 AND ABS(@Price-Price)<= (@Price*30/100) 
			 )
			 INSERT INTO UCAlert.RecommendUsedCarAlert 
			          (CustomerId,
			            CustomerName,
						CustomerAlert_Email,
						ProfileId,
						Car_SellerId,
						Car_SellerType,
						Car_CityId,
						Car_City,
						Car_Price,
						Car_MakeId,
						Car_Make,
						Car_ModelId,
						Car_Model,
						Car_Kms,
						Car_Version,
						Car_Year,
						Car_Color,
						Car_HasPhoto,
						Car_LastUpdated,
						Is_Mailed,
						alertUrl,
						CreatedOn,
						ImageUrl,
						CustomerAlert_Make,
						CustomerAlert_Model,
						[Rank],
						UsedCarAlertAlgoTypeId,
						IsFirstMail)
			 SELECT    Top  (@TotalMatchingCar)   CustomerId,
			            CustomerName,
						CustomerAlert_Email,
						ProfileId,
						Car_SellerId,
						Car_SellerType,
						Car_CityId,
						Car_City,
						Car_Price,
						Car_MakeId,
						Car_Make,
						Car_ModelId,
						Car_Model,
						Car_Kms,
						Car_Version,
						Car_Year,
						Car_Color,
						Car_HasPhoto,
						Car_LastUpdated,
						Is_Mailed,
						alertUrl,
						CreatedOn,
						ImageUrl,
						CarMake,
						CarModel,
					   ROW_NUMBER() OVER(ORDER BY [RANK] DESC)	[Rank],
					   1 ,   -------------First algo added on 20-03-2014 by manish 
					   0     ---- added by manish on 23-04-2014 for first mail
				FROM CTE1 ;
				/*ORDER BY [RANK] ASC;*/
				
				
		
		        DECLARE @TotalRecordInserted TINYINT =@@ROWCOUNT;
		        
		        -- If there are not sufficient cars in above set		
		        
	       IF (@TotalRecordInserted<@TotalMatchingCar)
		        BEGIN 
		        
		        WITH CTE1 AS 
			( SELECT 
				@CustomerId AS  CustomerId,
				@CustomerName AS CustomerName,
				@Email  AS   CustomerAlert_Email,
				LL.ProfileId  AS  ProfileId,
				LL.Inquiryid  AS   Car_SellerId,
				LL.SellerType  AS  Car_SellerType,
				LL.CityId  AS  Car_CityId,
				LL.CityName  AS  Car_City,
				LL.Price  AS  Car_Price,
				LL.MakeId  AS  Car_MakeId,
				LL.MakeName  AS Car_Make,
				LL.ModelId  AS Car_ModelId,
				LL.ModelName  AS Car_Model,
				LL.Kilometers  AS Car_Kms,
				LL.VersionName  AS Car_Version,
				LL.MakeYear  AS Car_Year,
				LL.Color  AS Car_Color,
				CASE WHEN LL.PhotoCount>0 THEN 1 ELSE 0 END  AS Car_HasPhoto,
				LL.LastUpdated AS Car_LastUpdated,
				0 AS Is_Mailed,
				'make=' + CONVERT(VARCHAR,LL.MakeId)
                + '&budget='  +  convert (varchar,(SELECT TOP 1 BudgetId FROM UCAlert.Budget WITH (NOLOCK) WHERE LL.Price Between LowerVal and UpperVal))
                + '&city='  +  CONVERT(VARCHAR,LL.CityId) 
                + '&dist=50'
                /*+ '&kms=' +  CONVERT(VARCHAR,(SELECT TOP 1 CarkmId FROM UCAlert.CarKms WHERE LL.Kilometers Between LowerVal and UpperVal))*/ AS alertUrl,
				GETDATE() AS CreatedOn,
				CASE WHEN  LL.OriginalImgPath IS NULL OR LL.OriginalImgPath ='' THEN '/used/no-car.jpg'  ELSE LL.OriginalImgPath END AS ImageUrl, --  Kundan on 21-03-2016  changed column refernce FrontImagePath to  OriginalImgPath
				@MakeName AS CarMake,
				@ModelName  As CarModel,
			 CASE WHEN CV.CarFuelType=@FuelTypeId THEN '1'
			      WHEN CV.CarFuelType=1  THEN '2'
			      WHEN CV.CarFuelType=2  THEN '3'
			      WHEN CV.CarFuelType=3  THEN '4'
			      WHEN CV.CarFuelType=4  THEN '5'
			      WHEN CV.CarFuelType=5  THEN '6' END
			 +     
			 CASE WHEN ABS(@Price-Price)<= (@Price*5/100) THEN '1'
			      WHEN ABS(@Price-Price)<= (@Price*10/100)  THEN '2'
			      WHEN ABS(@Price-Price)<= (@Price*15/100)  THEN '3'
			      WHEN ABS(@Price-Price)<= (@Price*20/100)  THEN '4'
			      WHEN ABS(@Price-Price)<= (@Price*25/100)  THEN '5'
			      WHEN ABS(@Price-Price)<= (@Price*30/100)  THEN '6' END
			  +     
			 CASE WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=1  THEN '1'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=2  THEN '2'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=3 THEN '3'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=4  THEN '4'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=5  THEN '5'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))=6  THEN '6'
			      WHEN ABS(YEAR(@ModelYear)-YEAR(MakeYear))>6  THEN '7' END
			  +    
			 CASE WHEN ABS(@Kms- Kilometers) <=2000  THEN '1'
			      WHEN ABS(@Kms- Kilometers) <=4000  THEN '2'
			      WHEN ABS(@Kms- Kilometers) <=6000  THEN '3'
			      WHEN ABS(@Kms- Kilometers) <=8000   THEN '4'
			      WHEN ABS(@Kms- Kilometers) <=10000   THEN '5'
			      WHEN ABS(@Kms- Kilometers) <=15000   THEN '6'
			      WHEN ABS(@Kms- Kilometers) <=20000   THEN '7'
			      WHEN ABS(@Kms- Kilometers) <=25000   THEN '8'
			      WHEN ABS(@Kms- Kilometers) >25000   THEN '9' END
			 +
			  CASE WHEN CV.CarFuelType=@FuelTypeId THEN '1'
			      WHEN CV.CarFuelType=1  THEN '2'
			      WHEN CV.CarFuelType=2  THEN '3'
			      WHEN CV.CarFuelType=3  THEN '4'
			      WHEN CV.CarFuelType=4  THEN '5'
			      WHEN CV.CarFuelType=5  THEN '6' END
			  +
			    CASE LL.Owners
							WHEN 'First Owner '   THEN '1'
							WHEN 'Second Owner '  THEN '2'
							WHEN 'Third Owner '	  THEN '3'
							WHEN 'Fourth Owner'   THEN '4'
							WHEN 'More than 4 owners' THEN '5' END
                 +
                 CASE 
							WHEN PhotoCount>=5   THEN '1'
							WHEN PhotoCount=1  THEN '3'
							WHEN PhotoCount=0	  THEN '4'
							ELSE '2' END   AS [Rank]
             FROM  livelistings AS LL  WITH (NOLOCK)
             JOIN  CarVersions AS CV  WITH (NOLOCK) ON CV.ID=LL.VersionId
			 WHERE ModelId<>@ModelId 
			 AND MakeId=@MakeId
			-- AND CV.CarFuelType=@FuelTypeId
			 AND CityId=@CityId
			-- AND (Inquiryid<>@InquiryId  OR SellerType<>@SellerType)
			 AND ABS(@Price-Price)<= (@Price*30/100) 
			 ) 			 
			 INSERT INTO UCAlert.RecommendUsedCarAlert 
			          (CustomerId,
			            CustomerName,
						CustomerAlert_Email,
						ProfileId,
						Car_SellerId,
						Car_SellerType,
						Car_CityId,
						Car_City,
						Car_Price,
						Car_MakeId,
						Car_Make,
						Car_ModelId,
						Car_Model,
						Car_Kms,
						Car_Version,
						Car_Year,
						Car_Color,
						Car_HasPhoto,
						Car_LastUpdated,
						Is_Mailed,
						alertUrl,
						CreatedOn,
						ImageUrl,
						CustomerAlert_Make,
						CustomerAlert_Model,
						[Rank],
						UsedCarAlertAlgoTypeId,
						IsFirstMail)
			 SELECT    Top (@TotalMatchingCar-@TotalRecordInserted)  CustomerId,
			            CustomerName,
						CustomerAlert_Email,
						ProfileId,
						Car_SellerId,
						Car_SellerType,
						Car_CityId,
						Car_City,
						Car_Price,
						Car_MakeId,
						Car_Make,
						Car_ModelId,
						Car_Model,
						Car_Kms,
						Car_Version,
						Car_Year,
						Car_Color,
						Car_HasPhoto,
						Car_LastUpdated,
						Is_Mailed,
						alertUrl,
						CreatedOn,
						ImageUrl,
						CarMake,
						CarModel,
				       @TotalRecordInserted+ROW_NUMBER() OVER(ORDER BY [RANK] DESC)	[Rank],
					   1,              -------------First algo added on 20-03-2014 by manish 
					   0     ---- added by manish on 23-04-2014 for first mail
				FROM CTE1 ;
				--ORDER BY [RANK] ASC ;
		        
		        
		        END 
			
			INSERT INTO #TblForSecondAlgo (ProfileId,CarName,FrontImagePath,InquiryId,SellerType,Price,MakeYear,Kilometers,PhotoCount,Score )
			EXEC [UCAlert].[RecommendedUsedCarAlertAlgo]  @Price  =@Price,
                                                          @CarVersionId =@VersionId,
                                                          @CityId =@CityId,
                                                          @NoOfRecommendCar=@NoOfRecommendCarForSecondAlgo 

       	  INSERT INTO UCAlert.RecommendUsedCarAlert 
							  (CustomerId,
								CustomerName,
								CustomerAlert_Email,
								ProfileId,
								Car_SellerId,
								Car_SellerType,
								Car_CityId,
								Car_City,
								Car_Price,
								Car_MakeId,
								Car_Make,
								Car_ModelId,
								Car_Model,
								Car_Kms,
								Car_Version,
								Car_Year,
								Car_Color,
								Car_HasPhoto,
								Car_LastUpdated,
								Is_Mailed,
								alertUrl,
								CreatedOn,
								ImageUrl,
								CustomerAlert_Make,
								CustomerAlert_Model,
								[Rank],
								UsedCarAlertAlgoTypeId,
								IsFirstMail)
						SELECT 
								@CustomerId AS  CustomerId,
								@CustomerName AS CustomerName,
								@Email  AS   CustomerAlert_Email,
								LL.ProfileId  AS  ProfileId,
								LL.Inquiryid  AS   Car_SellerId,
								LL.SellerType  AS  Car_SellerType,
								LL.CityId  AS  Car_CityId,
								LL.CityName  AS  Car_City,
								LL.Price  AS  Car_Price,
								LL.MakeId  AS  Car_MakeId,
								LL.MakeName  AS Car_Make,
								LL.ModelId  AS Car_ModelId,
								LL.ModelName  AS Car_Model,
								LL.Kilometers  AS Car_Kms,
								LL.VersionName  AS Car_Version,
								LL.MakeYear  AS Car_Year,
								LL.Color  AS Car_Color,
								CASE WHEN LL.PhotoCount>0 THEN 1 ELSE 0 END  AS Car_HasPhoto,
								LL.LastUpdated AS Car_LastUpdated,
								0 AS Is_Mailed,
								 'budget='  +  convert (varchar,(SELECT TOP 1 BudgetId FROM UCAlert.Budget WITH (NOLOCK) WHERE LL.Price Between LowerVal and UpperVal))
                                 + '&city='  +  CONVERT(VARCHAR,LL.CityId) 
                                + '&dist=50' AS alertUrl,
								GETDATE() AS CreatedOn,
								CASE WHEN  LL.OriginalImgPath IS NULL OR LL.OriginalImgPath ='' THEN '/used/no-car.jpg'  ELSE ll. OriginalImgPath END AS ImageUrl, -- Kundan on 21-03-2016  changed column refernce FrontImagePath to  OriginalImgPath
								@MakeName AS CarMake,
								@ModelName  As CarModel,
								ROW_NUMBER() OVER (PARTITION BY @CustomerId ORDER BY T.Score Desc) AS [Rank],
								2,
								0     ---- added by manish on 23-04-2014 for first mail
						FROM livelistings AS LL WITH (NOLOCK)
						JOIN #TblForSecondAlgo AS T ON LL.Inquiryid=T.InquiryId 
						                             AND LL.SellerType=T.SellerType
		  
		  SET @WhileLoopControl=@WhileLoopControl+1


		  TRUNCATE TABLE #TblForSecondAlgo

		 END TRY
		 BEGIN CATCH
		
	   	    SET @WhileLoopControl=@WhileLoopControl+1

			 INSERT INTO ScheduledJobExceptions (
			                                JobName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('UsedCarRecommendationDailyInsertJob',
									        'UCAlert.RecommendCarAlertForSecondMail',
											 ERROR_MESSAGE(),
											 NULL,
											 NULL,
											 GETDATE()
                                            )
		
		 END CATCH
      
      END

	  DROP TABLE #TblForSecondAlgo
END
