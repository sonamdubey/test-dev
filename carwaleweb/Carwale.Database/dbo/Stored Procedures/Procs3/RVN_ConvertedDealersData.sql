IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_ConvertedDealersData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_ConvertedDealersData]
GO

	-- =============================================
-- Author	:	Sachin Bharti(11th June 2014)
-- Description	:	Get converted dealers data from DCRM_SalesDealer
-- Modifier	:	Sachin Bharti(1st Aug 2014)
-- Purpose	:	Added two more columns PANNumber , TANNumber
-- =============================================
CREATE PROCEDURE [dbo].[RVN_ConvertedDealersData] 
	
	@SalesId	INT
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT 
			D.Id AS DealerId,
			D.Organization AS DealerName,
			D.TC_DealerTypeId ,
			DT.DealerType ,
			S.ID AS StateId,
			S.Name AS State,
			C.ID AS CityId,
			C.Name AS City,
			PK.Id AS SubProductId,
			PK.Name AS SubProductName,
			IPC.Id AS ProductId,
			IPC.Name AS ProductName,
			DSD.BOExecutive	,
			DSD.EntryDate	,
			DSD.ClosingDate	,
			D.TC_DealerTypeId AS DealerType	,
			ISNULL(DSD.ClosingAmount,0)	AS ClosingAmount,
			ISNULL(DSD.DiscountAmount,0)AS	DiscountAmount,
			ISNULL(DSD.ProductAmount,0) AS 	ProductAmount,
			ISNULL(DSD.ServiceTax,0)	AS 	ServiceTax,
			ISNULL(DSD.TotalAmount,0)	AS	TotalAmount,
			ISNULL(DSD.IsTDSGiven,0)	AS	TDSGiven,
			ISNULL(DSD.TDSAmount,0)		AS	TDSAmount,
			ISNULL(DSD.PANNumber,'')		AS	PANNumber,
			ISNULL(DSD.TANNumber,'')		AS	TANNumber,
			ISNULL(DSD.Quantity,0)		AS	RSAQuantity
			
	FROM 
			DCRM_SalesDealer DSD(NOLOCK)
			INNER JOIN Dealers	D(NOLOCK)	ON D.ID = DSD.DealerId AND DSD.Id = @SalesId 
			INNER JOIN States	S(NOLOCK)	ON S.ID = D.StateId 
			INNER JOIN Cities	C(NOLOCK)	ON D.CityId = C.ID
			INNER JOIN Packages PK(NOLOCK)	ON PK.Id = DSD.PitchingProduct
			INNER JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = PK.InqPtCategoryId
			INNER JOIN TC_DealerType DT(NOLOCK)		ON D.TC_DealerTypeId = DT.TC_DealerTypeId 
			
END
