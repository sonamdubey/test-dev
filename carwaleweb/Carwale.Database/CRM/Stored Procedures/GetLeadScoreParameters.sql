IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetLeadScoreParameters]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetLeadScoreParameters]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 02/07/2013
-- Description:	Fetches all the paramaters required for lead score calculation
--Modified By Reshma Shetty 30/07/2013 LeadId has been changed from i/p to o/p parameter and the if there is no active lead for a customer then return -1
-- Avishkar 04-09-2013 Removed Left Join on NewCarShowroomPrices
--Modified By Avishkar 20/11/2013 @AvgPrice Datatype has been changed from INT to FLOAT
--Modified by Reshma Shetty 2/12/2013 Changed the time frame for the data from last 6 months to 22nd Oct onwards and modification in Buytime calculation
--Basic condition is put to ignore test customer Ids(using NoLeadScoreCustIds table) and Parameters are returned for every PQ but LeadId is set to -1 in case of no lead or inactive lead
--modified by chetan on 02-07-2014 for email and mobile verification 
/*
DECLARE @LeadId INT , 
	@AvgPrice INT,
	@latest_BT SMALLINT, -- BT: BuyTime
	@dist_BT SMALLINT,
	@vintage SMALLINT, --difference between the first price quote taken date and last price quote taken date
	@dist_BS SMALLINT,  -- BS: BodyStyle
	@Prev_leads SMALLINT,
	@Prev_verified SMALLINT ,
	@Price_HF SMALLINT,
	@Cookie VARCHAR(100) 
EXEC [CRM].[GetLeadScoreParameters] 6690631,'raghu@carwale.com','8879957590',8206,1
-- Avishkar modified  13-06-2014 to use LeadstatusId=2

,--1237,'blabla@gmail.com','8967564565',3476,1,(no active lead)
	@LeadId  OUTPUT, 
	@AvgPrice  OUTPUT,
	@latest_BT OUTPUT, -- BT: BuyTime
	@dist_BT OUTPUT,
	@vintage OUTPUT, --difference between the first price quote taken date and last price quote taken date
	@dist_BS OUTPUT,  -- BS: BodyStyle
	@Prev_leads OUTPUT,
	@Prev_verified OUTPUT,
	@Price_HF OUTPUT, -- Herfind,
	@Cookie  OUTPUT
	
SELECT @LeadId , 
	@AvgPrice ,
	@latest_BT , -- BT: BuyTime
	@dist_BT ,
	@vintage , --difference between the first price quote taken date and last price quote taken date
	@dist_BS ,  -- BS: BodyStyle
	@Prev_leads ,
	@Prev_verified ,
	@Price_HF ,
	@Cookie

*/
-- =============================================
CREATE PROCEDURE [CRM].[GetLeadScoreParameters]
	-- Add the parameters for the stored procedure here
	@CWCustomerId NUMERIC,
	@Email VARCHAR(200),
	@Mobile	VARCHAR(15),
	@PQId INT ,
	@CityId INT,
	@LeadId INT = -1 OUTPUT, --Modified By Reshma Shetty 30/07/2013 LeadId has been changed from i/p to o/p parameter
	@AvgPrice FLOAT = 0 OUTPUT, --Modified By Avishkar 20/11/2013 @AvgPrice Datatype has been changed from INT to FLOAT
	@latest_BT INT = 0 OUTPUT, -- BT: BuyTime
	@dist_BT INT = 0 OUTPUT,
	@vintage INT = 0 OUTPUT, --difference between the first price quote taken date and last price quote taken date
	@dist_BS INT = 0 OUTPUT,  -- BS: BodyStyle
	@Prev_leads INT = 0 OUTPUT,
	@Prev_verified INT = 0 OUTPUT,
	@Price_HF INT = 0 OUTPUT ,-- Herfindahl index of the price segments in which price quotes have taken
	@Cookie VARCHAR(100)  OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
      
		DECLARE @price_3 INT -- Price Quotes in less than 3 lakhs category
		DECLARE @price_3_6 INT -- Price Quotes in 3 to 6 lakhs category
		DECLARE @price_6_12 INT -- Price Quotes in 6 to 12 lakhs category
		DECLARE @price_12_18 INT -- Price Quotes in 12 to 18 lakhs category
		DECLARE @price_18 INT -- Price Quotes greater than 18 lakhs category
		DECLARE @max_PQId BIGINT 
		DECLARE @max_LeadId INT
		DECLARE @BuyTime VARCHAR(20)
        
        --Modified By Reshma Shetty 30/07/2013 If there is no active lead for a customer then return -1
        DECLARE @CustomerId NUMERIC
        DECLARE @IsActive	BIT
        DECLARE @Name varchar(100)--added by chetan 7/2/2014
        --Commented By Deepak on 2nd Jan 2014, Not needed in SP
		--DECLARE @activeLeadId		Numeric
		--DECLARE @IsVerified			Bit
		--DECLARE @IsFake				Bit
		--DECLARE @ActiveLeadDate		VARCHAR(50)
		--DECLARE @ActiveLeadGroupId	INT
		--DECLARE @ActiveLeadGroupType INT		
		
		
		--IF (@CWCustomerId=4048562)
		--SET @LeadId =-1
		--ELSE
		--BEGIN		
		
			--SELECT @LeadId=ActiveLeadId 
			--FROM CRM_Customers CC WITH (NOLOCK)
			--	INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID=CC.ActiveLeadId
			--WHERE LeadStageId < 3 and CC.CWCustId=@CustomerId

			--SELECT @LeadId

			--IF(@LeadId >-1 AND @IsActive=1) --AND @IsVerified=1 AND @IsFake=0 AND @IsActive=1)
			--Get Customer Make
			 
	          IF NOT EXISTS (SELECT CustomerId FROM NoLeadScoreCustIds WHERE CustomerId=@CWCustomerId) --Modified by Reshma Shetty 2/12/2013 Basic condition is put to ignore test customer Ids and Parameters are returned for every PQ 
				BEGIN
				
					--Modified By Deepak on 2nd Jan 2013
					--Changed the query and created SP.
					SELECT @LeadId = ISNULL(NPI.CRM_LeadId, -1) FROM NewCarPurchaseInquiries NPI WITH (NOLOCK) WHERE NPI.Id = @PQId
					EXEC CRM_FetchLeadScoreLeadDetails @LeadId, @CustomerId OUTPUT, @IsActive OUTPUT
					
					SELECT @dist_BT=COUNT(DISTINCT BuyTime)
						,@dist_BS=COUNT(DISTINCT BodyStyleId)
						,@vintage=DATEDIFF(DAY,MIN(RequestDateTime),MAX(RequestDateTime))
						,@AvgPrice=AVG(Price)
						,@price_3=SUM(CASE WHEN Price < 300000 THEN 1 ELSE 0 END)
						,@price_3_6=SUM(CASE WHEN Price BETWEEN 300001 and 600000 THEN 1 ELSE 0 END)
						,@price_6_12=SUM(CASE WHEN Price BETWEEN 600001 and 1200000 THEN 1 ELSE 0 END)
						,@price_12_18=SUM(CASE WHEN Price BETWEEN 1200001 and 1800000 THEN 1 ELSE 0 END)
						,@price_18=SUM(CASE WHEN Price > 1800000 THEN 1 ELSE 0 END)
						,@max_PQId=MAX(NCP.Id)
					FROM NewCarPurchaseInquiries NCP WITH (NOLOCK) 
						INNER JOIN CarVersions CV ON CV.ID=NCP.CarVersionId
						INNER JOIN NewCarShowroomPrices CL WITH (NOLOCK) ON CL.CarVersionId=CV.ID AND CityId=@CityId -- Avishkar 04-09-2013 Removed Left Join 
					WHERE NCP.Id <= @PQId and CustomerId = @CWCustomerId 
						--AND NCP.RequestDateTime>=CONVERT(DATE,DATEADD(MONTH,-6,GETDATE())) 
						AND NCP.RequestDateTime>='2012-10-22 00:00:00.000'-- Modified by Reshma Shetty 2/12/2013 Changed the time frame for the data from last 6 months to 22nd Oct onwards
						
				 
						
			--SELECT @dist_BT
			--	,@dist_BS
			--	,@vintage
			--	,@AvgPrice
			--	,@price_3
			--	,@price_3_6
			--	,@price_6_12
			--	,@price_12_18
			--	,@price_18
			--	,@max_PQId

					
	                
					SELECT @BuyTime=Buytime
					FROM NewCarPurchaseInquiries WITH (NOLOCK) 
					WHERE Id = @max_PQId
					
										
					SET @Price_HF=dbo.CalculateHerfindahlIndex(@price_3,@price_3_6,@price_6_12,@price_12_18,@price_18)

					SET @latest_BT = CASE WHEN @BuyTime LIKE '%1%week' THEN 1
											 WHEN @BuyTime LIKE '%2%week' THEN 2
											 WHEN @BuyTime LIKE '%1%month' THEN 3
											 WHEN @BuyTime LIKE '2%month%' THEN 4
											 WHEN @BuyTime LIKE '_2%month%' THEN 4 -- Modified by Reshma Shetty 2/12/2013 Added to consider " 2 months" with space in the beginning and to avoid more than 2 months in this group
											 ELSE 5
										END
					
					
					
					SELECT @Prev_leads=COUNT(CL.ID)
						,@max_LeadId=MAX(CL.ID)
					FROM CRM_Leads CL WITH (NOLOCK)
						--INNER JOIN CRM_Customers CC WITH (NOLOCK) ON CC.ID=CL.CNS_CustId 
					WHERE CL.ID < @LeadId AND CL.CNS_CustId=@CustomerId    --Modified By Reshma Shetty 30/07/2013 Count the leads as per CRM_CustomerId and not CWCustId

						                
	                -- Avishkar modified  13-06-2014 to use LeadstatusId=2
					--SELECT @Prev_verified=CASE WHEN LeadstatusId=1 THEN 1 ELSE 0 END
					--FROM CRM_Leads WITH (NOLOCK)
					--WHERE ID=@max_LeadId
					
					SELECT @Prev_verified=CASE WHEN LeadstatusId=2 THEN 1 ELSE 0 END
					FROM CRM_Leads WITH (NOLOCK)
					WHERE ID=@max_LeadId
					
					

					--SELECT @dist_BT AS dist_BT,
					--@dist_BS AS dist_BS,
					--@vintage AS vintage,
					--@AvgPrice AS AvgPrice,
					--@Prev_leads AS Prev_leads,
					--@Price_HF AS Price_HF,
					--@Prev_verified AS Prev_verified,
					--@latest_BT AS latest_BT
				--END
				--ELSE

				IF(@IsActive=0) --Modified by Reshma Shetty 2/12/2013 LeadId is set to -1 in case of no lead or inactive lead
				SET @LeadId=-1
				 
			--END
			--Avishkar 04-09-2013 Added  to get cookie value from PQ_ClientInfo
			SELECT @Cookie=isnull(CWCookieValue,'Not Available')
			FROM PQ_ClientInfo with(nolock)
			WHERE PQId=@PQId 

			--added by chetan on 02/07/2014
			SELECT @NAME=NAME FROM NewPurchaseCities WITH (NOLOCK) WHERE InquiryId = @PQId 
                                       EXEC [CRM].[ValidateEMailMobileAndName] @PQId, @Mobile, @Name, @Email

			END
END
/****** Object:  StoredProcedure [dbo].[WA_UpComingCarDetails]    Script Date: 7/7/2014 8:29:41 AM ******/
SET ANSI_NULLS ON
