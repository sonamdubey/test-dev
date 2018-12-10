IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NewCarPurchasesShowLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NewCarPurchasesShowLeads]
GO

	-- =============================================
-- Author:		Ranjeet kumar
-- Create date: 18-Feb-2014
-- Description:	Get the all new car purchase Lead, matching for a dealer.
-- Table : TC_DealerMakes, NewCarPurchaseInquiries, TC_DealerCities
-- =============================================
CREATE PROCEDURE [dbo].[TC_NewCarPurchasesShowLeads] 
	@DealerId INT,
	@FromDate DATETIME,
	@ToDate DATETIME,
	@FromIndex  INT, 
	--@BuyTime VARCHAR(20) = NULL,
	@ToIndex    INT
	AS
	BEGIN
 DECLARE
	@MakeId INT =  NULL,
	@CityId INT = NULL,
	@NoOfDayOldCustomer INT  = 30 

	DECLARE @TempCustomer TABLE (
	
	Name VARCHAR(50),
	Mobile VARCHAR(20),
	Email VARCHAR(30),
	BuyTime VARCHAR(MAX),
	ReqDate DATETIME,
	ModelId INT,
	CarMake VARCHAR(30),
	CarModel VARCHAR(30),
	CarVersion VARCHAR(30),
	NewCarPuchaseId BIGINT,
	CWCustomerId BIGINT

);
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--Get the City ID and the Make ID 
	SELECT @MakeId = D.MakeId  FROM TC_DealerMakes as D WITH (NOLOCK) WHERE  D.DealerId = @DealerId 
	
	
	--IF NOT (@MakeId IS NULL) AND NOT (@CityId IS NULL)
	BEGIN
	INSERT INTO @TempCustomer  
	SELECT NCPL.CustomerName,('xxxxxxx'+ RIGHT(NCPL.Mobile,3)) AS Mobile , 
	('xxxx'+ RIGHT(NCPL.Email,(LEN(NCPL.Email)-CHARINDEX('@',NCPL.Email) + 1))) AS Email,
	NCPL.BuyTime,NCPL.Reqdate,NCPL.ModelId,NCPL.CarMake,NCPL.CarModel,  NCPL.CarVersion
	, NCPL.NewCarPurchaseId, NCPL.CWCustomerId
	FROM TC_NewCarPurchaseLead AS NCPL --WHERE
	LEFT JOIN TC_CustomerDetails AS TC WITH (NOLOCK) ON TC.Mobile = NCPL.Mobile AND TC.BranchId = @DealerId
	LEFT JOIN TC_InquiriesLead AS TIL WITH (NOLOCK)  ON TIL.TC_CustomerId= TC.Id   AND TIL.LatestInquiryDate > (GETDATE()-@NoOfDayOldCustomer)
		 WHERE NCPL.MakeId = @MakeId AND NCPL.ReqDate >= @FromDate AND NCPL.ReqDate <= @ToDate 
	AND NCPL.CityId  IN (SELECT DISTINCT CityId FROM TC_DealerCities  as D WITH (NOLOCK) WHERE  D.DealerId = @DealerId  )
	AND ISNULL(TIL.TC_InquiriesLeadId, -1) = -1;
	 --AND (@BuyTime IS NULL OR NCPL.BuyTime = @BuyTime); 
				
	WITH cte1 AS (
			
				SELECT DISTINCT TmC.Name AS Name, TmC.Mobile as Mobile, TmC.email as Email, 
				(Tmc.CarMake + ' ' + Tmc.CarModel + ' ' + TmC.CarVersion) AS CarDetails  , Tmc.BuyTime, 
				Tmc.ReqDate,Tmc.ModelId as ModelId ,Tmc.NewCarPuchaseId,Tmc.CWCustomerId,
				ROW_NUMBER() OVER( PARTITION BY  Tmc.CWCustomerId ORDER BY  Tmc.ReqDate  DESC ) AS CwRowNo 
				FROM
				 @TempCustomer AS TmC 
				)
						
				--Dump data in the temp table
				SELECT cte1.Name AS Name, cte1.Mobile as Mobile, cte1.email as Email, cte1.CarDetails, cte1.BuyTime, 
				cte1.ReqDate,  cte1.ModelId as ModelId,cte1.NewCarPuchaseId,cte1.CWCustomerId,
				ROW_NUMBER() OVER( ORDER BY  cte1.CWCustomerId  DESC ) AS RowNo
				INTO   #TblTempInventory 
				FROM   cte1  WHERE CTE1.CwRowNo =1		
				--------------------
	

				SELECT   *
				FROM   #TblTempInventory AS TIV
				WHERE  RowNo BETWEEN @FromIndex AND @ToIndex 
				ORDER BY  TIV.ReqDate DESC

			
				
				SELECT  COUNT(CWCustomerId) AS RecordCount 
				FROM   #TblTempInventory 
				--Delete temp table
				DROP TABLE #TblTempInventory 
				
		END	


END
 
