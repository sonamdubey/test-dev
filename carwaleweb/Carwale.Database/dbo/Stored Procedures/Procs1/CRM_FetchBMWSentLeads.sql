IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchBMWSentLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchBMWSentLeads]
GO

	
-- =============================================
-- Author      : Chetan Navin		
-- Create date : 11 Sep 2013
-- Description : To Fetch BMW sent leads.
-- Modification : Changed Alias of VersionName from OtherCars to OtherConsideredCars on 28th Oct 2013 By Ruchira
-- =============================================
CREATE PROCEDURE [dbo].[CRM_FetchBMWSentLeads]
	--Parameters
	@StartDate	DATETIME,
	@EndDate	DATETIME
AS

BEGIN
	DECLARE @tblVersionList table (CWCustomerId INT,VersionName varchar(max))
	DECLARE @tblCWCustomer table (id int identity(1,1), CWCUSTID int)
	DECLARE @whileloopcontrol int=1
	DECLARE @totalwhileloopcount int
	DECLARE @CWCustomerId  int
	DECLARE @version varchar(max)=null

	INSERT INTO @tblCWCustomer (CWCUSTID)
	SELECT DISTINCT C.CWCUSTID  from CRM_BMW_APIData AS B
	JOIN CRM_CarBasicData AS CRM WITH (NOLOCK) ON B.CBDId=CRM.ID
	JOIN CRM_Leads AS L  WITH (NOLOCK)  ON L.ID=CRM.LEADID
	JOIN CRM_CUSTOMERS AS C  WITH (NOLOCK)  ON C.ID=L.CNS_CUSTID
	WHERE C.ID<>-1
	AND b.Id<>-1
	AND c.CWCustId<>-1

	SELECT @totalwhileloopcount=COUNT(*) FROM @tblCWCustomer
 

     WHILE (@whileloopcontrol<=@totalwhileloopcount)
     BEGIN

			SELECT @CWCustomerId=CWCUSTID from @tblCWCustomer WHERE id=@whileloopcontrol

			SELECT DISTINCT @version=coalesce(@version+',','') + car  
			FROM NewCarPurchaseInquiries as n WITH (NOLOCK) 
			join vwmmv as v on  n.carversionid=v.VersionId  WHERE  n.CustomerId=@CWCustomerId
			and v.MakeId not in (1,51)
			and RequestDateTime>=(GETDATE()-7)
			and n.CustomerId<>-1
			
			SELECT @version=coalesce(@version+',','') + car 
			FROM(
					SELECT DISTINCT (v.Make + ' ' + v.Model)  AS Car
					FROM NewCarPurchaseInquiries as n WITH (NOLOCK) 
					join vwmmv as v on  n.carversionid=v.VersionId  WHERE  n.CustomerId=@CWCustomerId
					and v.MakeId not in (1,51)
					and RequestDateTime>=(GETDATE()-7)
					and n.CustomerId<>-1
				) AllCars

			INSERT INTO @tblVersionList values (@CWCustomerId,@version)
			SET @version=null

			SET @whileloopcontrol=@whileloopcontrol+1
    END 
    
    --Changed Alias of VersionName from OtherCars to OtherConsideredCars on 28th Oct 2013 By Ruchira
	SELECT VB.*, VM.Make, VM.Model, VM.Version, REPLACE(ND.Name, 'OEM-', '') AS Dealer, VL.VersionName AS OtherConsideredCars
	FROM CRM_BMW_APIData VB WITH (NOLOCK)
		INNER JOIN vwMMV VM WITH (NOLOCK) ON VB.VersionId = VM.VersionId
		INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON VB.DealerId = ND.ID
		INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON VB.CBDId = CBD.Id 
		INNER JOIN CRM_Leads AS CL WITH (NOLOCK) ON CL.ID = CBD.LeadId
		INNER JOIN CRM_Customers CC WITH (NOLOCK) ON CL.CNS_CustId = CC.ID
		LEFT JOIN @tblVersionList VL ON CC.CWCustId = VL.CWCustomerId
	WHERE VB.CreatedOn BETWEEN @StartDate AND @EndDate
END

