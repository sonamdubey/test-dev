IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchNIMailerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchNIMailerData]
GO

	

-- =============================================
-- Author      : Deepak
-- Create date : 13 Feb 2014
-- Description : To Fetch Mailer Leads.
-- Modifier	   : Ruchira Patil on 14 Feb 2014 (query to fetch the nummber of cars between min price and max price)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_FetchNIMailerData] --EXEC CRM_FetchNIMailerData

AS

BEGIN
	DECLARE @tblCustomerData table (LeadId BIGINT, CustName VARCHAR(200),Email VARCHAR(200),CarName VARCHAR(200),CityId INT,MinPrice INT,MaxPrice INT)
	DECLARE @tblCustomer table (id int identity(1,1),LeadId BIGINT, CustName VARCHAR(200),Email VARCHAR(200))
	DECLARE @tblCustCars table (LeadId BIGINT,CarName VARCHAR(500))
	DECLARE @tblBudgetCarCnt table (LeadId BIGINT,NoOfCars INT)
	DECLARE @tblFinalCustData table (CustName VARCHAR(200),Email VARCHAR (200),CarName VARCHAR(200),NoOfCars INT)

	INSERT INTO @tblCustomerData (LeadId , CustName ,Email,CarName ,CityId ,MinPrice ,MaxPrice ) 
	SELECT DISTINCT VOL.LeadId, CUS.FirstName+ ' ' + CUS.LastName,CUS.Email,(VM.Make + ' ' + VM.Model),CUS.CityId,
	MIN(NCP.Price) MinPrice, MAX(NCP.Price) MaxPrice
	FROM CRM_VerificationOthersLog VOL
	INNER JOIN CRM_Leads CL ON CL.ID = VOL.LeadId
	INNER JOIN CRM_Customers CUS ON CUS.ID = CL.CNS_CustId
	INNER JOIN CRM_CarBasicData CBD ON CBD.LeadId = CL.ID
	INNER JOIN vwMMV VM ON CBD.VersionId = VM.VersionId
	INNER JOIN NewCarShowroomPrices NCP ON NCP.CarVersionId = CBD.VersionId AND NCP.CityId = CBD.CityId
	WHERE (VOL.UnavailabilityReason = 35 OR VOL.NotIntReason IN (78,79,20,57))
	AND CONVERT(DATE,VOL.UpdatedOn) = CONVERT(DATE,GETDATE())
	GROUP BY VOL.LeadId, CUS.FirstName, CUS.LastName,CUS.Email, VM.Make, VM.Model,CUS.CityId

	INSERT INTO @tblCustomer (LeadId,CustName,Email)
	SELECT DISTINCT LeadId,CustName,Email FROM @tblCustomerData

	DECLARE @whileloopcontrol INT=1
	DECLARE @totalwhileloopcount INT
	DECLARE @LeadId  INT
	DECLARE @cars VARCHAR(MAX)=NULL
	DECLARE @minprice INT
	DECLARE @maxprice INT
	DECLARE @cntcars INT

	SELECT @totalwhileloopcount = COUNT(LeadId) FROM @tblCustomer
 
	WHILE (@whileloopcontrol<=@totalwhileloopcount)
		BEGIN
		
			--GET LeadId
			SELECT @LeadId=LeadId FROM @tblCustomer WHERE id=@whileloopcontrol 

			--GET All Cars against that leadId
			SELECT @cars=coalesce(@cars+',','') + Car 
			FROM(
				SELECT DISTINCT CarName AS Car
				FROM @tblCustomerData
				WHERE LeadId =@LeadId
			) AllCars
			
			INSERT INTO @tblCustCars VALUES (@LeadId,@cars)
			
			--Get Price Range of lead
			SELECT @minprice = MIN(MinPrice),@maxprice = MAX(MaxPrice) 
			FROM  @tblCustomerData CD
			WHERE LeadId=@LeadId
			
			--Get Cars within that price range
			SELECT @cntcars=count(DISTINCT NCSP.CarModelId)
			FROM NewCarShowroomPrices NCSP
			WHERE PRICE BETWEEN (@minprice) AND (@maxprice)
			
			IF @cntcars = 1
				SET @cntcars = @cntcars  + 1
				
			INSERT INTO @tblBudgetCarCnt VALUES (@LeadId,@cntcars)
			
			SET @cars=null
			SET @cntcars = null
			SET @whileloopcontrol=@whileloopcontrol+1
		END 
	
	INSERT INTO @tblFinalCustData(CustName,Email,CarName,NoofCars)
	SELECT DISTINCT C.CUSTNAME,C.Email,CC.CarName,BCC.NoOfCars
	FROM @tblCustomer C
	JOIN @tblCustCars CC ON CC.LeadId = C.LeadId
	JOIN @tblBudgetCarCnt BCC ON BCC.LeadId = C.LeadId
	
	SELECT * FROM @tblFinalCustData

END






