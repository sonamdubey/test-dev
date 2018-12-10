IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_OpenPitchingProduct]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_OpenPitchingProduct]
GO

	----------------------------------------------------------------------------------------------------------------------------------

-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	16th Dec 2013
-- Description	:	Get existing open pitching products
--				:	against the Dealer
-- Modified ON	:	Sachin Bharti(22th Dec. 2013)
-- Modifier		:	Sachin Bharti(5th May 2015)
-- Purpose		:	Make left join OprUsers
-- Modifier		: Kartik Rathod on 26 Nov 2015, fetched Comments 
-- Modifier     :  Vinay kumar prajapati  8th jan 2016 For Open product Api Required more Data 
-- Modifier		: Amit Yadav on 04-04-2016 If a null value is received then replace it with "0"
-- Modifier		: KARTIK RATHOD on 6 April 2016,fetched isActive column
-- Modified By : Komal Manjare on(07-May-2016)
-- Desc : get model,exceptionmodel and leadperday for product
-- [M_OpenPitchingProduct] 3377
-- =============================================
CREATE PROCEDURE [dbo].[M_OpenPitchingProduct]
	@DealerId	Numeric(18,0)
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT 
        IP.Id AS	ProductId, --- Added By Vinay Kumar Prajapati  For Api Requirment 
        IP.Name AS  ProductName, 
		ISNULL(DSD.ProductStatus,3) AS ProductClassificationId, --As Per UI basis impliment in APP Lavel
		DSD.LeadStatus AS ProductStatusId,  ---As Per UI basis impliment in APP Lavel
		DSD.StartMeetingId AS SalesMeetingId,
		P.Name AS Package,P.ID as PackageID , DSD.ID AS SalesId,ISNULL(DSD.ClosingAmount,0) AS ClosingAmount,--Added By Amit Yadav on 04-04-2016
		DSD.PitchDuration,ISNULL(DSD.NoOfLeads,1) AS NoOfLeads,DSD.ClosingProbability,OU.UserName AS InitiatedBy,
		IP.Id AS InquiryPointId,ISNULL(DSD.Quantity,-1) AS RSAQuantity ,
		CONVERT(CHAR,DSD.ClosingDate,106) AS ExpctdClsngDate ,	
		DAY(CONVERT(CHAR,DSD.ClosingDate,106)) AS DayClosDate,
		MONTH(CONVERT(CHAR,DSD.ClosingDate,106)) AS MonthClosDate,
		YEAR(CONVERT(CHAR,DSD.ClosingDate,106)) AS YearClosDate,
		ISNULL(DSD.PercentageSlab,0) AS WarPerSlab,
		ISNULL(DSD.ProductStatus,3) AS ProductStatus,-- 3 - New - Dealer take this product for first time
		CASE WHEN (IP.Id = 24 OR IP.Id = 32) THEN DSD.NoOfLeads WHEN (IP.Id = 33 OR IP.Id = 37) THEN DSD.Quantity ELSE DSD.PitchDuration END AS Duration,
		CASE WHEN (IP.Id = 24 OR IP.Id = 32) THEN 'Leads' WHEN IP.Id = 33 THEN 'RSA' WHEN IP.Id = 37 THEN 'Warranty' ELSE 'Days' END AS ProductType,
		ISNULL(DSD.Comments,'-') AS Comments,ISNULL(DSD.ContractType,0) AS ContractType,
		ISNULL(DAY(CONVERT(CHAR,DSD.StartDate,106)),0) AS DayStDate,
		ISNULL(MONTH(CONVERT(CHAR,DSD.StartDate,106)),0) AS MonthStDate,
		ISNULL(YEAR(CONVERT(CHAR,DSD.StartDate,106)),0) AS YearStDate,
		CONVERT(CHAR,DSD.StartDate,106) AS StartDate,
		P.IsActive,DSD.Model,DSD.ExceptionModel,DSD.LeadPerDay --Added By:Komal Manjare(07-April-2016) -get model details
	FROM 
		DCRM_SalesDealer DSD (NOLOCK) 
		INNER JOIN Packages P (NOLOCK) ON DSD.PitchingProduct = P.Id 
		LEFT JOIN OprUsers OU (NOLOCK) ON OU.Id = DSD.InitiatedBy 
		INNER JOIN InquiryPointCategory IP(NOLOCK) ON IP.Id = P.InqPtCategoryId
	WHERE 
		DSD.LeadStatus = 1 AND DSD.DealerId = @DealerId
	ORDER BY DSD.UpdatedOn DESC
	
END