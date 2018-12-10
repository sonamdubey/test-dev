IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetIndividualListingDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetIndividualListingDetails_15]
GO

	
-- Author:		<Reshma Shetty>
-- Create date: <28/03/2013>
-- Description:	Gets the details of the cars listed by a customer.
-- Avishkar 20-5-2013 Added to show package details
-- Amit 20-5-2013 Added @DefaultPackageId i/p parameter
-- EXEC GetIndividualListingDetails 1535
-- Ashish G. Kamble on 20 Nov 2013	-- Added condition IsArchived = 0
--Modified by Reshma Shetty 26/11/2013 Added Free and Paid count based on CustomerId and Customer Mobile combination
-- Modified by Shikhar Maheshwari on 17 July, 2014 -- Added the column Package Expiry date
-- Modified by Manish on 18-07-2014 added WITH (NOLOCK) keyword
-- =============================================
CREATE  PROCEDURE   [dbo].[GetIndividualListingDetails_15.5.2] 

	-- Add the parameters for the stored procedure here
	@CustomerId INT,
	@DefaultPackageId INT = NULL -- Amit 20-5-2013 Added @DefaultPackageId i/p parameter
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DECLARE @Mobile VARCHAR(20) 
	--DECLARE @InquiryId VARCHAR(20)
	
	--SELECT @Mobile=CustomerMobile, @InquiryId=ID 
	--FROM CustomerSellInquiries WITH(NOLOCK) 
	--WHERE CustomerId = @CustomerId

    -- Insert statements for procedure here
	SELECT CSI.ID
		,MA.NAME + ' ' + MO.NAME + ' ' + VE.NAME AS CarName
		,VE.ID AS VersionId
		,CSI.CarRegNo
		,CSI.EntryDate
		,CSI.Price
		,CSI.MakeYear
		,CSI.Kilometers
		,CSI.Color
		,CSI.Comments
		,CSI.IsApproved
		,CSI.IsFake
		,IQS.NAME AS Status
		,CSI.LastBidDate
		,CSI.ClassifiedExpiryDate
		,CSI.PackageExpiryDate
		,CSI.IsShowContact
		,CP.ImageUrlThumbSmall AS smallPic
		,CP.HostURL 
		,CP.DirectoryPath
		,(select count(CP.Id)
		from CarPhotos AS CP WITH (NOLOCK)
		where  CP.InquiryId=CSI.ID AND CP.IsDealer=0 and CP.IsActive=1 and CP.IsApproved=1) AS  PhotoCount		
		,Count(DISTINCT CR.CustomerId) AS TotalInq
		,Count(DISTINCT CASE WHEN CIV.CarId IS NULL THEN CR.CustomerId END) AS NotReadInq
		,CSI.PackageType
		,CSI.IsPremium
		,CSI.CurrentStep
		,VE.CarModelId AS CarModelId
		-- Avishkar 20-5-2013 Added to show package details
		,(CASE when CSI.PackageId is null then @DefaultPackageId else CSI.PackageId end) as PackageId
		,(CASE when P.Name is null then (select Name from Packages WITH (NOLOCK)  where ID = @DefaultPackageId) else P.Name end) as Package
		,(CASE when P.Amount is null then (select Amount from Packages WITH (NOLOCK) where ID = @DefaultPackageId) else P.Amount end) as PackageAmount
		, LL.Inquiryid AS LLInquiryId
		,CSI.IsListingCompleted
		,CSI.PaymentMode
		,isnull(LC.Free,0) Free
		,isnull(LC.Paid,0) Paid
	FROM CustomerSellInquiries AS CSI  WITH (NOLOCK)
		INNER JOIN InquiryStatus AS IQS  WITH (NOLOCK) ON IQS.ID = CSI.StatusId
		INNER JOIN CarVersions AS VE WITH (NOLOCK)  ON VE.ID = CSI.CarVersionId
		INNER JOIN CarModels AS MO WITH (NOLOCK) ON MO.ID = VE.CarModelId
		INNER JOIN CarMakes AS MA WITH (NOLOCK) ON MA.ID = MO.CarMakeId
		--Modified by Reshma Shetty 26/11/2013 Free and Paid count based on CustomerId and Customer Mobile combination 
		/*Counts the number of free and paid listing that are live*/
		LEFT JOIN (    
					 SELECT CustomerId,CustomerMobile,
						sum(case LL.PackageType when 30 then 1 else 0 end) 'Free',
						sum(case LL.PackageType when 31 then 1 else 0 end) 'paid'
						FROM CustomerSellInquiries CS WITH(NOLOCK) 
						LEFT JOIN livelistings LL WITH(NOLOCK) ON LL.Inquiryid = CS.ID
						WHERE CS.PackageType IN (30,31)  and cs.CustomerId=@CustomerId
						group by CustomerId,CustomerMobile
	             ) AS LC ON LC.CustomerId  = CSI.CustomerId and LC.CustomerMobile=CSI.CustomerMobile
		LEFT JOIN  ClassifiedRequests AS CR WITH (NOLOCK) ON CR.SellInquiryId = CSI.ID AND CR.IsActive = 1 AND CR.CustomerId <> - 1
		LEFT JOIN  CarPhotos AS CP WITH (NOLOCK) ON CP.InquiryId=CSI.ID AND CP.IsDealer=0 and CP.IsMain=1 and CP.IsActive=1 and CP.IsApproved=1
		LEFT JOIN  ConsumerInquiriesViewable CIV WITH (NOLOCK)  ON ConsumerType = 2 AND CIV.CarId = CSI.ID
		LEFT JOIN Packages as P WITH (NOLOCK)  on P.Id=CSI.PackageId  -- Avishkar 20-5-2013 Added to show package details
		LEFT JOIN livelistings AS LL WITH (NOLOCK)  ON LL.Inquiryid = CSI.ID

	WHERE CSI.CustomerId = @CustomerId
		--AND IQS.IsActive = 1	-- Commented By Ashish on 29 Nov 2013
		AND CSI.IsFake = 0 AND CSI.IsArchived = 0
	GROUP BY CSI.ID,MA.NAME + ' ' + MO.NAME + ' ' + VE.NAME ,VE.ID ,CSI.CarRegNo,CSI.EntryDate,CSI.Price,CSI.MakeYear
		,CSI.Kilometers,CSI.Color,CSI.Comments,CSI.IsApproved,CSI.IsFake,IQS.IsActive,IQS.NAME,CSI.LastBidDate
		,CSI.ClassifiedExpiryDate,CSI.PackageExpiryDate,CP.ImageUrlThumbSmall,CP.HostURL,CP.DirectoryPath ,CSI.PackageType,VE.CarModelId ,P.Name,P.Amount,CSI.PackageId,CSI.IsPremium,CSI.CurrentStep
		,LL.Inquiryid, CSI.IsListingCompleted, CSI.PaymentMode,CSI.IsShowContact
		,LC.Free,LC.Paid
	ORDER BY IQS.IsActive DESC-- packagepriority desc,stepid desc
		,CSI.IsApproved DESC
		,CSI.IsFake ASC
		,CSI.ClassifiedExpiryDate DESC

END


