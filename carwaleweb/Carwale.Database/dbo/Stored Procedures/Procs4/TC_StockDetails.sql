IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockDetails]
GO

	 


-- =============================================
-- Author:		Nilesh Utture
-- Create date: 5th September, 2012
-- Description:	Edit Stock Details
-- Modified By: Tejashree Patil on 26 Oct 2012 at 3 pm Desc: Added Tcs.Id, in SELECT QUERRY
-- modified By Vivek Gupta on 22-01-2014, Added select query for getting all active offers of stock
-- modified By Vivek Gupta on 05-12-2014, Added select query for getting addition information of stock
-- Modified By Vivek Gupta on 20-12-2014, Fetched FreeRSADetails from TC_CarAdditionalInformation
-- EXEC TC_StockDetails 611363,5
-- Modified by : Kritika Choudhary on 25h feb 2016, added join with Dealers table
-- =============================================
CREATE PROCEDURE [dbo].[TC_StockDetails] @StockId BIGINT
	,@BranchId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT St.*
		,Cc.*
		,Tcs.STATUS
		,Ma.Id MakeId
		,Mo.Id ModelId
		,Ve.Id VersionId
		,Ma.NAME Makename
		,Mo.NAME ModelName
		,D.CertificationId DealerCertificationId
	FROM TC_Stock St WITH (NOLOCK)
	INNER JOIN TC_CarCondition Cc WITH (NOLOCK) ON Cc.StockId = St.Id
	LEFT JOIN TC_StockStatus Tcs WITH (NOLOCK) ON Tcs.Id = St.StatusId
	LEFT JOIN CarVersions Ve WITH (NOLOCK) ON Ve.Id = St.VersionId
	LEFT JOIN CarModels Mo WITH (NOLOCK) ON Mo.Id = Ve.CarModelId
	LEFT JOIN CarMakes Ma WITH (NOLOCK) ON Ma.Id = Mo.CarMakeId
	LEFT JOIN Dealers D WITH (NOLOCK) ON D.ID = St.BranchId
	WHERE St.Id = @StockId
		AND St.BranchId = @BranchId
		--AND D.IsDealerActive=1 AND D.IsDealerDeleted=0  Modified by : Kritika Choudhary on 14th march 2016
	SELECT STATUS
		,Id
	FROM TC_StockStatus WITH (NOLOCK)
	WHERE IsActive = 1

	-- added By Vivek Gupta on 22-01-2014,
	SELECT TC_UsedCarOfferId
	FROM TC_MappingOfferWithStock WITH (NOLOCK)
	WHERE StockId = @StockId
		AND IsActive = 1

	-- added By Vivek Gupta on 03-12-2014
	SELECT StockId
		,IsCarInWarranty
		,WarrantyValidTill
		,WarrantyProvidedBy
		,ThirdPartyWarrantyProviderName
		,WarrantyDetails
		,HasExtendedWarranty
		,ExtendedWarrantyValidFor
		,ExtendedWarrantyProviderName
		,ExtendedWarrantyDetails
		,HasAnyServiceRecords
		,ServiceRecordsAvailableFor
		,HasRSAAvailable
		,RSAValidTill
		,RSAProviderName
		,RSADetails
		,HasFreeRSA
		,FreeRSAValidFor
		,FreeRSAProvidedBy
		,FreeRSADetails
	FROM TC_CarAdditionalInformation WITH (NOLOCK)
	WHERE StockId = @StockId
END

/****** Object:  StoredProcedure [dbo].[TC_StockDetailsUpdate]    Script Date: 12/18/2014 11:15:35 ******/
 

